﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kingo.Threading;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Kingo.Messaging.Validation
{
    [TestClass]
    public sealed class MessageValidationPipelineTest
    {
        #region [====== Messages ======]

        private static readonly ErrorInfo _Errors = new ErrorInfo(Enumerable.Empty<KeyValuePair<string, string>>(), "Some error.");

        private abstract class MessageToValidate : IMessage
        {
            protected abstract bool IsValid
            {
                get;
            }

            public ErrorInfo Validate(bool haltOnFirstError = false)
            {
                if (IsValid)
                {
                    return ErrorInfo.Empty;
                }
                return _Errors;
            }
        }

        private sealed class SomeCommand : MessageToValidate
        {
            public SomeCommand(bool isValid)
            {
                IsValid = isValid;
            }

            protected override bool IsValid
            {
                get;
            }
        }

        private sealed class SomeEvent : MessageToValidate
        {
            public SomeEvent(bool isValid)
            {
                IsValid = isValid;
            }

            protected override bool IsValid
            {
                get;
            }
        }

        private sealed class ObjectValidator : IMessageValidator<object>
        {
            private readonly bool _isValid;

            public ObjectValidator(bool isValid)
            {
                _isValid = isValid;
            }

            public ErrorInfo Validate(object message, bool haltOnFirstError = false)
            {
                if (_isValid)
                {
                    return ErrorInfo.Empty;
                }
                return _Errors;
            }
        }

        #endregion

        private MessageValidationPipeline _pipeline;
        private MicroProcessorSpy _processor;

        [TestInitialize]
        public void Setup()
        {
            _pipeline = new MessageValidationPipeline();
            _processor = new MicroProcessorSpy();
            _processor.Add(_pipeline);
        }

        [TestMethod]
        public void Handle_HandlesCommand_IfCommandIsValid()
        {
            var someEvent = new object();

            AssertContains(_processor.Handle(new SomeCommand(true), (message, context) =>
            {
                context.OutputStream.Publish(someEvent);
            }), someEvent);
        }

        [TestMethod]
        [ExpectedException(typeof(BadRequestException))]
        public void Handle_ThrowsBadRequestException_IfCommandIsNotValid()
        {
            var someCommand = new SomeCommand(false);

            try
            {
                _processor.Handle(someCommand, (message, context) => { });
            }
            catch (BadRequestException exception)
            {
                Assert.AreSame(someCommand, exception.FailedMessage);

                var invalidMessageException = exception.InnerException as InvalidMessageException;

                Assert.IsNotNull(invalidMessageException);                
                Assert.AreEqual("Message of type 'SomeCommand' is not valid: 1 validation error(s) found.", invalidMessageException.Message);
                throw;
            }
        }

        [TestMethod]
        public void Handle_HandlesEvent_IfEventIsValid()
        {
            var someEvent = new object();

            AssertContains(_processor.Handle(new SomeEvent(true), (message, context) =>
            {
                context.OutputStream.Publish(someEvent);
            }), someEvent);
        }

        [TestMethod]
        [ExpectedException(typeof(InternalServerErrorException))]
        public void Handle_ThrowsInternalServerErrorsException_IfCommandIsNotValid()
        {
            var someEvent = new SomeEvent(false);

            try
            {
                _processor.Handle(someEvent, (message, context) => { });
            }
            catch (InternalServerErrorException exception)
            {
                Assert.AreSame(someEvent, exception.FailedMessage);

                var invalidMessageException = exception.InnerException as InvalidMessageException;

                Assert.IsNotNull(invalidMessageException);
                Assert.AreEqual("Message of type 'SomeEvent' is not valid: 1 validation error(s) found.", invalidMessageException.Message);
                throw;
            }
        }

        [TestMethod]
        public void Handle_HandlesMessage_IfMessageDoesNotImplementIMessageInterface_And_NoValidatorForTypeHasBeenRegistered()
        {
            var someEvent = new object();

            AssertContains(_processor.Handle(new object(), (message, context) =>
            {
                context.OutputStream.Publish(someEvent);
            }), someEvent);
        }

        [TestMethod]
        public void Handle_HandlesMessage_IfMessageDoesNotImplementIMessageInterface_And_ValidatorReturnsNoErrors()
        {
            _pipeline.Register(new ObjectValidator(true));

            var someEvent = new object();

            AssertContains(_processor.Handle(new object(), (message, context) =>
            {
                context.OutputStream.Publish(someEvent);
            }), someEvent);
        }

        [TestMethod]
        [ExpectedException(typeof(InternalServerErrorException))]
        public void Handle_ThrowsInternalServerErrorsException_IfMessageDoesNotImplementIMessageInterface_And_ValidatorReturnsErrors()
        {            
            _pipeline.Register(new ObjectValidator(false));

            var someMessage = new object();

            try
            {
                _processor.Handle(someMessage, (message, context) => { });
            }
            catch (InternalServerErrorException exception)
            {
                Assert.AreSame(someMessage, exception.FailedMessage);

                var invalidMessageException = exception.InnerException as InvalidMessageException;

                Assert.IsNotNull(invalidMessageException);
                Assert.AreEqual("Message of type 'Object' is not valid: 1 validation error(s) found.", invalidMessageException.Message);
                throw;
            }
        }

        private static void AssertContains(IMessageStream stream, params object[] events)
        {
            Assert.IsNotNull(stream);
            Assert.AreEqual(events.Length, stream.Count);

            for (int index = 0; index < events.Length; index++)
            {
                Assert.AreSame(events[index], stream[index]);
            }
        }
    }
}
