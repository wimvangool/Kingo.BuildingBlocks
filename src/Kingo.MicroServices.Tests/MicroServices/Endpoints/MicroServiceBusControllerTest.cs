﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Kingo.MicroServices.Endpoints
{
    [TestClass]
    public sealed class MicroServiceBusControllerTest
    {
        #region [====== MicroServiceBusControllerStub ======]

        private sealed class MicroServiceBusControllerStub : MicroServiceBusController
        {
            private readonly IEnumerator<HostedEndpoint> _endpoints;

            public MicroServiceBusControllerStub(params HostedEndpoint[] endpoints) :
                base(new MicroProcessorStub(endpoints.Length))
            {
                _endpoints = endpoints.AsEnumerable().GetEnumerator();
            }

            protected override bool TryCreateEndpointFor(IMessageHandlerEndpoint messageHandler, out HostedEndpoint endpoint)
            {
                if (_endpoints.MoveNext())
                {
                    endpoint = _endpoints.Current;
                    return true;
                }
                endpoint = null;
                return false;
            }                
        }

        #endregion

        #region [====== MicroProcessorStub ======]

        private sealed class MicroProcessorStub : MicroProcessor
        {
            private readonly int _numberOfEndpoints;

            public MicroProcessorStub(int numberOfEndpoints)
            {
                _numberOfEndpoints = numberOfEndpoints;
            }

            public override IEnumerable<IMessageHandlerEndpoint> CreateEndpoints() =>
                Enumerable.Repeat(new Mock<IMessageHandlerEndpoint>().Object, _numberOfEndpoints);
        }

        #endregion

        #region [====== EndpointStubs ======]

        private class EndpointStub : HostedEndpoint
        {
            public int StartCount
            {
                get;
                private set;
            }

            public int StopCount
            {
                get;
                private set;
            }

            public int DisposeCount
            {
                get;
                private set;
            }

            public override Task StartAsync(CancellationToken cancellationToken)
            {
                StartCount++;
                return base.StartAsync(cancellationToken);
            }                

            public override Task StopAsync(CancellationToken cancellationToken)
            {
                StopCount++;
                return base.StopAsync(cancellationToken);
            }

            protected override void Dispose(bool disposing)
            {
                DisposeCount++;
                base.Dispose(disposing);
            }

            protected override Task ConnectAsync(CancellationToken cancellationToken) =>
                Task.CompletedTask;

            protected override Task DisconnectAsync(CancellationToken cancellationToken) =>
                Task.CompletedTask;
        }

        private sealed class EndpointThatThrowsExceptionStub : EndpointStub
        {
            private readonly Exception _exception;
            private readonly bool _throwOnStop;

            public EndpointThatThrowsExceptionStub(Exception exception, bool throwOnStop = false)
            {
                _exception = exception;
                _throwOnStop = throwOnStop;
            }

            public override async Task StartAsync(CancellationToken cancellationToken)
            {
                await base.StartAsync(cancellationToken);

                if (_throwOnStop)
                {
                    return;
                }
                throw _exception;
            }

            public override async Task StopAsync(CancellationToken cancellationToken)
            {
                await base.StopAsync(cancellationToken);

                if (_throwOnStop)
                {
                    throw _exception;
                }
            }
        }

        private sealed class EndpointThatCancelsOperationStub : EndpointStub
        {
            private readonly CancellationTokenSource _tokenSource;

            public EndpointThatCancelsOperationStub(CancellationTokenSource tokenSource)
            {
                _tokenSource = tokenSource;
            }

            public override async Task StartAsync(CancellationToken cancellationToken)
            {
                await base.StartAsync(cancellationToken);

                _tokenSource.Cancel();
            }
        }

        #endregion

        #region [====== StartAsync ======]

        [TestMethod]
        public async Task StartAsync_DoesNothing_IfNoEndpointsAreCreated()
        {
            var controller = new MicroServiceBusControllerStub();

            await controller.StartAsync(CancellationToken.None);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public async Task StartAsync_Throws_IfEndpointIsAlreadyStarted()
        {
            var controller = new MicroServiceBusControllerStub();

            await controller.StartAsync(CancellationToken.None);
            await controller.StartAsync(CancellationToken.None);
        }

        [TestMethod]
        public async Task StartAsync_StartsAllEndpoints_IfMultipleEndpointsAreCreated()
        {
            var endpointA = new EndpointStub();
            var endpointB = new EndpointStub();
            var controller = new MicroServiceBusControllerStub(endpointA, endpointB);

            await controller.StartAsync(CancellationToken.None);

            Assert.AreEqual(1, endpointA.StartCount);
            Assert.AreEqual(0, endpointA.StopCount);
            Assert.AreEqual(0, endpointA.DisposeCount);

            Assert.AreEqual(1, endpointB.StartCount);
            Assert.AreEqual(0, endpointB.StopCount);
            Assert.AreEqual(0, endpointB.DisposeCount);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public async Task StartAsync_DoesNotStartNextEndpoint_IfFirstEndpointThrowsException()
        {
            var exceptionToThrow = new InvalidOperationException();
            var endpointA = new EndpointThatThrowsExceptionStub(exceptionToThrow);
            var endpointB = new EndpointStub();
            var controller = new MicroServiceBusControllerStub(endpointA, endpointB);

            try
            {
                await controller.StartAsync(CancellationToken.None);                
            }
            catch (InvalidOperationException exception)
            {
                Assert.AreSame(exceptionToThrow, exception);
                throw;
            }
            finally
            {
                Assert.AreEqual(1, endpointA.StartCount);
                Assert.AreEqual(0, endpointA.StopCount);
                Assert.AreEqual(0, endpointA.DisposeCount);

                Assert.AreEqual(0, endpointB.StartCount);
                Assert.AreEqual(0, endpointB.StopCount);
                Assert.AreEqual(0, endpointB.DisposeCount);
            }            
        }

        [TestMethod]        
        public async Task StartAsync_DoesNotStartAnyEndpoint_IfStartupIsCancelledImmediately()
        {
            using (var tokenSource = new CancellationTokenSource())
            {
                tokenSource.Cancel();

                var endpointA = new EndpointStub();
                var endpointB = new EndpointStub();
                var controller = new MicroServiceBusControllerStub(endpointA, endpointB);

                await controller.StartAsync(tokenSource.Token);

                Assert.AreEqual(0, endpointA.StartCount);
                Assert.AreEqual(0, endpointA.StopCount);
                Assert.AreEqual(0, endpointA.DisposeCount);

                Assert.AreEqual(0, endpointB.StartCount);
                Assert.AreEqual(0, endpointB.StopCount);
                Assert.AreEqual(0, endpointB.DisposeCount);
            }                
        }

        [TestMethod]
        public async Task StartAsync_DoesNotStartSecondEndpoint_IfStartupIsCancelledAfterStartingFirstEndpoint()
        {
            using (var tokenSource = new CancellationTokenSource())
            {                
                var endpointA = new EndpointThatCancelsOperationStub(tokenSource);
                var endpointB = new EndpointStub();
                var controller = new MicroServiceBusControllerStub(endpointA, endpointB);

                await controller.StartAsync(tokenSource.Token);

                Assert.AreEqual(1, endpointA.StartCount);
                Assert.AreEqual(1, endpointA.StopCount);
                Assert.AreEqual(1, endpointA.DisposeCount);

                Assert.AreEqual(0, endpointB.StartCount);
                Assert.AreEqual(0, endpointB.StopCount);
                Assert.AreEqual(0, endpointB.DisposeCount);
            }
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public async Task StartAsync_StopsFirstEndpoint_IfSecondEndpointThrowsException()
        {
            using (var tokenSource = new CancellationTokenSource())
            {
                var exceptionToThrow = new InvalidOperationException();
                var endpointA = new EndpointStub();
                var endpointB = new EndpointThatThrowsExceptionStub(exceptionToThrow);
                var controller = new MicroServiceBusControllerStub(endpointA, endpointB);

                try
                {
                    await controller.StartAsync(tokenSource.Token);
                }
                catch (InvalidOperationException exception)
                {
                    Assert.AreSame(exceptionToThrow, exception);
                    throw;
                }                
                finally
                {
                    Assert.AreEqual(1, endpointA.StartCount);
                    Assert.AreEqual(1, endpointA.StopCount);
                    Assert.AreEqual(1, endpointA.DisposeCount);

                    Assert.AreEqual(1, endpointB.StartCount);
                    Assert.AreEqual(0, endpointB.StopCount);
                    Assert.AreEqual(0, endpointB.DisposeCount);
                }                
            }
        }

        #endregion

        #region [====== StopAsync ======]

        [TestMethod]
        public async Task StopAsync_DoesNothing_IfNoEndpointsAreCreated()
        {            
            var controller = new MicroServiceBusControllerStub();

            await controller.StartAsync(CancellationToken.None);
            await controller.StopAsync(CancellationToken.None);
        }

        [TestMethod]
        public async Task StopAsync_DoesNothing_IfEndpointsIsAlreadyInStoppedState()
        {
            var controller = new MicroServiceBusControllerStub();

            await controller.StopAsync(CancellationToken.None);
            await controller.StopAsync(CancellationToken.None);
        }

        [TestMethod]
        public async Task StopAsync_StopsAllEndpoints_IfMultipleEndpointsAreCreated()
        {
            var endpointA = new EndpointStub();
            var endpointB = new EndpointStub();
            var controller = new MicroServiceBusControllerStub(endpointA, endpointB);

            await controller.StartAsync(CancellationToken.None);
            await controller.StopAsync(CancellationToken.None);

            Assert.AreEqual(1, endpointA.StartCount);
            Assert.AreEqual(1, endpointA.StopCount);
            Assert.AreEqual(1, endpointA.DisposeCount);

            Assert.AreEqual(1, endpointB.StartCount);
            Assert.AreEqual(1, endpointB.StopCount);
            Assert.AreEqual(1, endpointB.DisposeCount);
        }

        [TestMethod]        
        public async Task StopAsync_StopsAllEndpoints_IfOneEndpointThrowsExceptionWhenStopped()
        {
            var exceptionToThrow = new InvalidOperationException();
            var endpointA = new EndpointThatThrowsExceptionStub(exceptionToThrow, true);
            var endpointB = new EndpointStub();            
            var controller = new MicroServiceBusControllerStub(endpointA, endpointB);

            await controller.StartAsync(CancellationToken.None);
            await controller.StopAsync(CancellationToken.None);

            Assert.AreEqual(1, endpointA.StartCount);
            Assert.AreEqual(1, endpointA.StopCount);
            Assert.AreEqual(1, endpointA.DisposeCount);

            Assert.AreEqual(1, endpointB.StartCount);
            Assert.AreEqual(1, endpointB.StopCount);
            Assert.AreEqual(1, endpointB.DisposeCount);                                   
        }      

        #endregion
    }
}
