﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Kingo.Messaging
{
    /// <summary>
    /// Represents a basic implementation of the <see cref="IMicroProcessor" /> interface.
    /// </summary>
    public class MicroProcessor : IMicroProcessor
    {             
        private readonly Lazy<MessageHandlerFactory> _messageHandlerFactory;
        private readonly Lazy<IMicroProcessorPipeline[]> _processorPipeline;

        /// <summary>
        /// Initializes a new instance of the <see cref="MicroProcessor" /> class.
        /// </summary>
        protected MicroProcessor()
        {
            _messageHandlerFactory = new Lazy<MessageHandlerFactory>(BuildMessageHandlerFactory, true);
            _processorPipeline = new Lazy<IMicroProcessorPipeline[]>(() => CreateProcessorPipeline().WhereNotNull().ToArray(), true);
        }

        #region [====== Command & Events ======]   

        /// <summary>
        /// Returns the <see cref="MessageHandlerFactory" /> of this processor.
        /// </summary>
        protected internal MessageHandlerFactory MessageHandlerFactory =>
            _messageHandlerFactory.Value;

        private MessageHandlerFactory BuildMessageHandlerFactory()
        {                        
            var factory = CreateMessageHandlerFactory();

            factory.RegisterSingleton(typeof(IMicroProcessor), this);
            factory.RegisterSingleton(typeof(IMicroProcessorContext), MicroProcessorContext.Current);
            factory.RegisterMessageHandlers(CreateMessageHandlerTypeSet(TypeSet.Empty));

            return factory;
        }

        /// <summary>
        /// Creates and returns a <see cref="MessageHandlerFactory" /> for this processor.
        /// </summary>        
        /// <returns>A new <see cref="MessageHandlerFactory" /> to be used by this processor.</returns>
        protected virtual MessageHandlerFactory CreateMessageHandlerFactory() =>
            new UnityFactory();

        /// <summary>
        /// Returns a <see cref="TypeSet"/> that will be scanned by the <see cref="MessageHandlerFactory" /> of this processor
        /// to locate <see cref="IMessageHandler{T}"/> classes and auto-register these classes so that instances of them
        /// can be resolved at run-time.
        /// </summary>
        /// <param name="typeSet">A set of types that can be used to build the set to return.</param>
        /// <returns></returns>
        protected virtual TypeSet CreateMessageHandlerTypeSet(TypeSet typeSet) =>
            typeSet;

        /// <inheritdoc />
        public virtual Task<IMessageStream> HandleStreamAsync(IMessageStream inputStream, CancellationToken? token = null)
        {
            if (inputStream == null)
            {
                throw new ArgumentNullException(nameof(inputStream));
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

        /// <summary>
        /// Returns all pipeline segments in the order they are invoked at run-time for each message.
        /// </summary>
        protected internal IReadOnlyList<IMicroProcessorPipeline> ProcessorPipeline =>
            _processorPipeline.Value;       

        /// <summary>
        /// Creates and returns a collection of <see cref="IMicroProcessorPipeline" /> segments which will be assembled into a single
        /// pipeline to handle every message or query.
        /// </summary>
        /// <returns>A collection of <see cref="IMicroProcessorPipeline" /> instances.</returns>
        protected internal virtual IEnumerable<IMicroProcessorPipeline> CreateProcessorPipeline() =>
            Enumerable.Empty<IMicroProcessorPipeline>();

        #endregion
    }
}