﻿using System.Threading.Tasks;

namespace Kingo.MicroServices.Controllers
{
    /// <summary>
    /// Serves as a base-class for all test's that handle a message and return the resulting event stream.
    /// </summary>
    /// <typeparam name="TMessage">Type of the message that is handled by this test.</typeparam>
    /// <typeparam name="TEventStream">Type of the resulting event stream.</typeparam>
    public abstract class HandleMessageTest<TMessage, TEventStream> : MicroProcessorOperationTest, IHandleMessageTest<TMessage, TEventStream>
        where TEventStream : EventStream
    {
        Task IHandleMessageTest<TMessage, TEventStream>.WhenAsync(IMessageProcessor<TMessage> processor, MicroProcessorOperationTestContext context) =>
            WhenAsync(processor, context);

        /// <summary>
        /// Executes this test by handling a specific message using the specified <paramref name="processor"/>.
        /// </summary>
        /// <param name="processor">The processor to handle the message with.</param>
        /// <param name="context">The context in which the test is running.</param> 
        protected abstract Task WhenAsync(IMessageProcessor<TMessage> processor, MicroProcessorOperationTestContext context);

        void IHandleMessageTest<TMessage, TEventStream>.Then(TMessage message, IHandleMessageResult<TEventStream> result, MicroProcessorOperationTestContext context) =>
            Then(message, result, context);

        /// <summary>
        /// Verifies the <paramref name="result"/> of this test.
        /// </summary>
        /// <param name="message">The message that was handled by this test.</param>        
        /// <param name="result">The result of this test.</param>
        /// <param name="context">The context in which the test is running.</param>                
        protected abstract void Then(TMessage message, IHandleMessageResult<TEventStream> result, MicroProcessorOperationTestContext context);
    }
}