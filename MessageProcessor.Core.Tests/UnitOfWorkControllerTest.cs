﻿using System;
using System.Threading;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace YellowFlare.MessageProcessing
{
    [TestClass]
    public sealed class UnitOfWorkControllerTest
    {
        private sealed class FlushThreadChecker : IUnitOfWork
        {
            private readonly bool _expectSameThread;
            private readonly Thread _creationThread;   
            private Thread _flushThread;

            public FlushThreadChecker(bool expectSameThread)
            {
                _expectSameThread = expectSameThread;
                _creationThread = Thread.CurrentThread;
            }

            public string FlushGroup
            {
                get { return null; }
            }

            public bool CanBeFlushedAsynchronously
            {
                get { return true; }
            }

            public bool RequiresFlush()
            {
                return true;
            }

            public void Flush()
            {
                _flushThread = Thread.CurrentThread;                
            }

            public void AssertFlushedOnExpectedThread()
            {
                if (_flushThread == null)
                {
                    Assert.Fail("UnitOfWork was not flushed at all.");
                }
                if (_expectSameThread && _flushThread != _creationThread)
                {
                    Assert.Fail("UnitOfWork was expected to be flushed on same context.");
                }
                else if (!_expectSameThread && _flushThread == _creationThread)
                {
                    Assert.Fail("UnitOfWork was expected to be flushed on different context.");
                }
            }

            public void AssertFlushedOnSameThreadAs(FlushThreadChecker other)
            {
                Assert.AreSame(_flushThread, other._flushThread, "Resources should have been flushed on the same context.");
            }            
        }

        private UnitOfWorkController _controller;

        [TestInitialize]
        public void Setup()
        {
            _controller = new UnitOfWorkController();
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Register_Throws_IfUnitOfWorkIsNull()
        {
            _controller.Enlist(null);
        }        

        [TestMethod]
        public void Flush_FlushesLastItemAlwaysSynchronously()
        {
            var checker = new FlushThreadChecker(true);

            _controller.Enlist(new UnitOfWorkWithAttributeOneAsync(checker));            
            _controller.Flush();

            checker.AssertFlushedOnExpectedThread();
        }

        [TestMethod]
        public void Flush_FlushesAllResourcesSynchronously_IfSpecifiedResourceForcesSynchronousFlush()
        {
            var checkerA = new FlushThreadChecker(true);
            var checkerB = new FlushThreadChecker(true);

            // NB: Because checker A and B will end up in one group, and one forces a synchronous flush,
            // both will be flushed synchronously.
            _controller.Enlist(new UnitOfWorkWithAttributeOneSync(checkerA));
            _controller.Enlist(new UnitOfWorkWithAttributeOneAsync(checkerB));            
            _controller.Flush();

            checkerA.AssertFlushedOnExpectedThread();            
            checkerA.AssertFlushedOnSameThreadAs(checkerB);
        }
        
        [TestMethod]
        public void Flush_FlushesParticularResourceItemsAsynchronously_IfTheseItemsAllowIt()
        {
            var checkerA = new FlushThreadChecker(true);
            var checkerB = new FlushThreadChecker(false);

            _controller.Enlist(new UnitOfWorkWithAttributeTwoSync(checkerA));
            _controller.Enlist(new UnitOfWorkWithAttributeOneAsync(checkerB));            
            _controller.Flush();

            checkerA.AssertFlushedOnExpectedThread();
            checkerB.AssertFlushedOnExpectedThread();
        }
        
        [TestMethod]
        public void Flush_FlushesParticularResourceGroupsAsynchronously_IfTheseGroupsAllowIt()
        {
            var checkerA = new FlushThreadChecker(false);
            var checkerB = new FlushThreadChecker(true);
            var checkerC = new FlushThreadChecker(false);

            _controller.Enlist(new UnitOfWorkWithAttributeOneAsync(checkerA));
            _controller.Enlist(new UnitOfWorkWithAttributeTwoSync(checkerB));
            _controller.Enlist(new UnitOfWorkWithAttributeOneAsync(checkerC));            
            _controller.Flush();

            checkerA.AssertFlushedOnSameThreadAs(checkerC);
            checkerB.AssertFlushedOnExpectedThread();          
        }             
    }
}
