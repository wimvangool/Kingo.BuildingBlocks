﻿using System;
using System.Threading;
using System.Threading.Tasks;
using Kingo.Threading;

namespace Kingo.Messaging
{
    /// <summary>
    /// Represents a basic implementation of the <see cref="IMicroProcessor" /> interface.
    /// </summary>
    public class MicroProcessor : IMicroProcessor
    {             
        private readonly Lazy<MessageHandlerFactory> _messageHandlerFactory;
        private readonly Lazy<MicroProcessorPipeline> _pipeline;

        /// <summary>
        /// Initializes a new instance of the <see cref="MicroProcessor" /> class.
        /// </summary>
        public MicroProcessor()
        {
            _messageHandlerFactory = new Lazy<MessageHandlerFactory>(BuildMessageHandlerFactory, true);  
            _pipeline = new Lazy<MicroProcessorPipeline>(() => BuildPipeline(new MicroProcessorPipeline()), true);
        }

        #region [====== Command & Events ======]   

        /// <summary>
        /// Returns the <see cref="MessageHandlerFactory" /> of this processor.
        /// </summary>
        protected internal MessageHandlerFactory MessageHandlerFactory =>
            _messageHandlerFactory.Value;

        private MessageHandlerFactory BuildMessageHandlerFactory() => CreateMessageHandlerFactory()           
            .RegisterInstance<IMicroProcessor>(this)
            .RegisterInstance(MicroProcessorContext.Current)
            .RegisterMessageHandlers(CreateMessageHandlerTypeSet());

        /// <summary>
        /// When overridden, creates and returns a <see cref="MessageHandlerFactory" /> for this processor.
        /// The default implementation returns <c>null</c>.
        /// </summary>        
        /// <returns>A new <see cref="MessageHandlerFactory" /> to be used by this processor.</returns>
        protected internal virtual MessageHandlerFactory CreateMessageHandlerFactory() =>
            new MessageHandlerFactoryStub();

        /// <summary>
        /// Returns a <see cref="TypeSet"/> that will be scanned by the <see cref="MessageHandlerFactory" /> of this processor
        /// to locate <see cref="IMessageHandler{T}"/> classes and auto-register these classes so that instances of them
        /// can be resolved at run-time.
        /// </summary>        
        /// <returns>A <see cref="TypeSet" /> that contains all message handler types to register.</returns>
        protected virtual TypeSet CreateMessageHandlerTypeSet() =>
            TypeSet.Empty;

        /// <inheritdoc />
        public virtual Task<IMessageStream> HandleStreamAsync(IMessageStream inputStream, CancellationToken? token = null)
        {
            if (inputStream == null)
            {
                throw new ArgumentNullException(nameof(inputStream));
            }
            if (inputStream.Count == 0)
            {
                return AsyncMethod.Value(MessageStream.Empty);
            }
            return HandleInputStreamAsyncMethod.Invoke(this, inputStream, token);
        }

        /// <summary>
        /// Handles the specified <paramref name="metadataStream"/> by invoking the specified <paramref name="handler"/>.
        /// This method can be used to perform all necessary operations before and/or after handling a stream of metadata events,
        /// such as creating or exitting a specific transaction scope.
        /// </summary>
        /// <param name="metadataStream">The stream to handler.</param>
        /// <param name="handler">The handler to invoke.</param>
        /// <returns>A task representing the operation.</returns>
        protected internal virtual Task HandleMetadataStreamAsync(IMessageStream metadataStream, Func<IMessageStream, Task> handler) =>
            handler.Invoke(metadataStream);

        /// <summary>
        /// Determines whether or not the specified message is a Command. By default,
        /// this method returns <c>true</c> when the type-name ends with 'Command'.
        /// </summary>
        /// <param name="message">The message to analyze.</param>
        /// <returns>
        /// <c>true</c> if the specified <paramref name="message"/> is a command; otherwise <c>false</c>.
        /// </returns>
        protected internal virtual bool IsCommand(object message) =>
            NameOf(message.GetType()).EndsWith("Command");

        private static string NameOf(Type messageType) =>
            messageType.IsGenericType ? messageType.Name.RemoveTypeParameterCount() : messageType.Name;

        #endregion

        #region [====== Queries ======]

        /// <inheritdoc />
        public Task<TMessageOut> ExecuteAsync<TMessageOut>(IQuery<TMessageOut> query, CancellationToken? token = null)
        {
            if (query == null)
            {
                throw new ArgumentNullException(nameof(query));
            }
            return ExecuteQueryAsyncMethod<TMessageOut>.Invoke(this, query, token);
        }

        /// <inheritdoc />
        public Task<TMessageOut> ExecuteAsync<TMessageIn, TMessageOut>(TMessageIn message, IQuery<TMessageIn, TMessageOut> query, CancellationToken? token = null)
        {
            if (ReferenceEquals(message, null))
            {
                throw new ArgumentNullException(nameof(message));
            }
            if (query == null)
            {
                throw new ArgumentNullException(nameof(query));
            }
            return ExecuteQueryAsyncMethod<TMessageIn, TMessageOut>.Invoke(this, query, message, token);
        }

        #endregion

        #region [====== MicroProcessorPipeline ======]

        internal MicroProcessorPipeline Pipeline =>
            _pipeline.Value;       

        /// <summary>
        /// When overridden, this method can be used to add global filters and configuration to the pipeline of this processor.
        /// </summary>
        /// <param name="pipeline">The pipeline to configure.</param>
        /// <returns>The configured pipeline.</returns>
        protected virtual MicroProcessorPipeline BuildPipeline(MicroProcessorPipeline pipeline) =>
            pipeline;

        #endregion
    }
}
