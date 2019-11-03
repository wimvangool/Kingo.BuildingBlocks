﻿using System;
using System.Collections.Generic;
using System.Security.Principal;
using System.Threading;
using System.Threading.Tasks;

namespace Kingo.MicroServices
{
    /// <summary>
    /// When implemented by a class, represents a processor that can process commands, events and queries.
    /// </summary>
    public interface IMicroProcessor
    {
        /// <summary>
        /// Returns the name of the service.
        /// </summary>
        string ServiceName
        {
            get;
        }

        /// <summary>
        /// Returns the service provider the processor uses to resolve its dependencies.
        /// </summary>
        IMicroProcessorServiceProvider ServiceProvider
        {
            get;
        }

        /// <summary>
        /// Configures the processor to use the specified <paramref name="user"/> for each operation as long as the
        /// returned scope is active.
        /// </summary>
        /// <param name="user">The principal to use.</param>
        /// <returns>A scope that can be disposed when the principal can be reset to its previous value.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="user"/> is <c>null</c>.
        /// </exception>
        IDisposable AssignUser(IPrincipal user);

        /// <summary>
        /// Creates and returns a new <see cref="IMessageEnvelopeBuilder" /> that can be used to build new messages to process by this processor.
        /// </summary>
        /// <returns>A new <see cref="IMessageEnvelopeBuilder" />.</returns>
        IMessageEnvelopeBuilder CreateMessageBuilder();

        /// <summary>
        /// Creates and returns all endpoints that are configured to handle commands or events from a service bus.
        /// </summary>
        /// <returns>A collection of endpoints.</returns>
        IEnumerable<IMicroServiceBusEndpoint> CreateMicroServiceBusEndpoints();

        #region [====== Commands ======]

        /// <summary>
        /// Executes a command with a specified <paramref name="messageHandler"/>.
        /// </summary>
        /// <typeparam name="TCommand">Type of the command.</typeparam>
        /// <param name="messageHandler">The message handler that will handle the command.</param>
        /// <param name="message">The command to handle.</param>
        /// <param name="token">Optional token that can be used to cancel the operation.</param>
        /// <returns>
        /// The result of the operation, which includes all published events and the number of message handlers that were invoked.
        /// </returns> 
        /// <exception cref="ArgumentNullException">
        /// <paramref name="messageHandler"/> or <paramref name="message"/> is <c>null</c>.
        /// </exception>       
        Task<IMessageHandlerOperationResult> ExecuteCommandAsync<TCommand>(IMessageHandler<TCommand> messageHandler, TCommand message, CancellationToken? token = null);

        /// <summary>
        /// Executes a command with a specified <paramref name="messageHandler"/>.
        /// </summary>
        /// <typeparam name="TCommand">Type of the command.</typeparam>
        /// <param name="messageHandler">The message handler that will handle the command.</param>
        /// <param name="message">The command to handle.</param>
        /// <param name="token">Optional token that can be used to cancel the operation.</param>
        /// <returns>
        /// The result of the operation, which includes all published events and the number of message handlers that were invoked.
        /// </returns> 
        /// <exception cref="ArgumentNullException">
        /// <paramref name="messageHandler"/> or <paramref name="message"/> is <c>null</c>.
        /// </exception>       
        Task<IMessageHandlerOperationResult> ExecuteCommandAsync<TCommand>(IMessageHandler<TCommand> messageHandler, MessageEnvelope<TCommand> message, CancellationToken? token = null);

        #endregion

        #region [====== Events ======]

        /// <summary>
        /// Handles an event with a specified <paramref name="messageHandler"/>.
        /// </summary>
        /// <typeparam name="TEvent">Type of the event.</typeparam>
        /// <param name="messageHandler">The message handler that will handle the event.</param>
        /// <param name="message">The event to handle.</param>
        /// <param name="token">Optional token that can be used to cancel the operation.</param>
        /// <returns>
        /// The result of the operation, which includes all published events and the number of message handlers that were invoked.
        /// </returns> 
        /// <exception cref="ArgumentNullException">
        /// <paramref name="messageHandler"/> or <paramref name="message"/> is <c>null</c>.
        /// </exception>       
        Task<IMessageHandlerOperationResult> HandleEventAsync<TEvent>(IMessageHandler<TEvent> messageHandler, TEvent message, CancellationToken? token = null);

            /// <summary>
        /// Handles an event with a specified <paramref name="messageHandler"/>.
        /// </summary>
        /// <typeparam name="TEvent">Type of the event.</typeparam>
        /// <param name="messageHandler">The message handler that will handle the event.</param>
        /// <param name="message">The event to handle.</param>
        /// <param name="token">Optional token that can be used to cancel the operation.</param>
        /// <returns>
        /// The result of the operation, which includes all published events and the number of message handlers that were invoked.
        /// </returns> 
        /// <exception cref="ArgumentNullException">
        /// <paramref name="messageHandler"/> or <paramref name="message"/> is <c>null</c>.
        /// </exception>       
        Task<IMessageHandlerOperationResult> HandleEventAsync<TEvent>(IMessageHandler<TEvent> messageHandler, MessageEnvelope<TEvent> message, CancellationToken? token = null);

        #endregion

        #region [====== Queries ======]

        /// <summary>
        /// Executes the specified <paramref name="query"/> and returns its result asynchronously.
        /// </summary>
        /// <typeparam name="TResponse">Type of the message returned by the query.</typeparam>        
        /// <param name="query">The query to execute.</param> 
        /// <param name="token">Optional token that can be used to cancel the operation.</param>                        
        /// <returns>The result that carries the response returned by the <paramref name="query"/>.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="query"/> is <c>null</c>.
        /// </exception>         
        Task<IQueryOperationResult<TResponse>> ExecuteQueryAsync<TResponse>(IQuery<TResponse> query, CancellationToken? token = null);

        /// <summary>
        /// Executes the specified <paramref name="query"/> using the specified <paramref name="message"/> and returns its result asynchronously.
        /// </summary>
        /// <typeparam name="TRequest">Type of the message going into the query.</typeparam>
        /// <typeparam name="TResponse">Type of the message returned by the query.</typeparam>
        /// <param name="query">The query to execute.</param>
        /// <param name="message">Message containing the parameters of this query.</param>
        /// <param name="token">Optional token that can be used to cancel the operation.</param>
        /// <returns>The result that carries the response returned by the <paramref name="query"/>.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="query"/> or <paramref name="message"/> is <c>null</c>.
        /// </exception>         
        Task<IQueryOperationResult<TResponse>> ExecuteQueryAsync<TRequest, TResponse>(IQuery<TRequest, TResponse> query, TRequest message, CancellationToken? token = null);

        /// <summary>
        /// Executes the specified <paramref name="query"/> using the specified <paramref name="message"/> and returns its result asynchronously.
        /// </summary>
        /// <typeparam name="TRequest">Type of the message going into the query.</typeparam>
        /// <typeparam name="TResponse">Type of the message returned by the query.</typeparam>
        /// <param name="query">The query to execute.</param>
        /// <param name="message">Message containing the parameters of this query.</param>
        /// <param name="token">Optional token that can be used to cancel the operation.</param>
        /// <returns>The result that carries the response returned by the <paramref name="query"/>.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="query"/> or <paramref name="message"/> is <c>null</c>.
        /// </exception>         
        Task<IQueryOperationResult<TResponse>> ExecuteQueryAsync<TRequest, TResponse>(IQuery<TRequest, TResponse> query, MessageEnvelope<TRequest> message, CancellationToken? token = null);

        #endregion
    }
}