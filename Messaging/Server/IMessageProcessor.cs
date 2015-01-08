﻿using System.Threading;
using System.Threading.Tasks;

namespace System.ComponentModel.Server
{
    /// <summary>
    /// When implemented by a class, represents a handler of any message.
    /// </summary>
    public interface IMessageProcessor
    {
        /// <summary>
        /// Returns the <see cref="IMessageProcessorBus" /> of this processor.
        /// </summary>
        IMessageProcessorBus DomainEventBus
        {
            get;
        }

        /// <summary>
        /// Returns a pointer to the message that is currently being handled by the processor.
        /// </summary>
        MessagePointer MessagePointer
        {
            get;
        }

        #region [====== Commands ======]

        /// <summary>
        /// Executes the specified command by invoking all registered message handlers asynchronously.
        /// </summary>
        /// <typeparam name="TCommand">Type of the message.</typeparam>
        /// <param name="message">Message to handle.</param>  
        /// <returns>The <see cref="Task" /> that is executing the command.</returns>              
        /// <exception cref="ArgumentNullException">
        /// <paramref name="message"/> is <c>null</c>.
        /// </exception>        
        Task ExecuteAsync<TCommand>(TCommand message) where TCommand : class, IRequestMessage<TCommand>; 

        /// <summary>
        /// Executes the specified command by invoking all registered message handlers asynchronously.
        /// </summary>
        /// <typeparam name="TCommand">Type of the message.</typeparam>
        /// <param name="message">Message to handle.</param>        
        /// <param name="token">
        /// Optional token that can be used to cancel the operation.
        /// </param>          
        /// <returns>The <see cref="Task" /> that is executing the command.</returns>  
        /// <exception cref="ArgumentNullException">
        /// <paramref name="message"/> is <c>null</c>.
        /// </exception>               
        Task ExecuteAsync<TCommand>(TCommand message, CancellationToken? token) where TCommand : class, IRequestMessage<TCommand>; 

        /// <summary>
        /// Executes the specified command by invoking the specified delegate asynchronously.
        /// </summary>
        /// <typeparam name="TCommand">Type of the message.</typeparam>
        /// <param name="message">Message to handle.</param>        
        /// <param name="handler">Delegate that will be used to execute the command.</param>  
        /// <returns>The <see cref="Task" /> that is executing the command.</returns>        
        /// <exception cref="ArgumentNullException">
        /// <paramref name="message"/> is <c>null</c>.
        /// </exception>        
        Task ExecuteAsync<TCommand>(TCommand message, Action<TCommand> handler) where TCommand : class, IRequestMessage<TCommand>; 

        /// <summary>
        /// Executes the specified command by invoking the specified delegate asynchronously.
        /// </summary>
        /// <typeparam name="TCommand">Type of the message.</typeparam>
        /// <param name="message">Message to handle.</param>        
        /// <param name="handler">Delegate that will be used to execute the command.</param>
        /// <param name="token">
        /// Optional token that can be used to cancel the operation.
        /// </param>          
        /// <returns>The <see cref="Task" /> that is executing the command.</returns>        
        /// <exception cref="ArgumentNullException">
        /// <paramref name="message"/> is <c>null</c>.
        /// </exception>                
        Task ExecuteAsync<TCommand>(TCommand message, Action<TCommand> handler, CancellationToken? token) where TCommand : class, IRequestMessage<TCommand>; 

        /// <summary>
        /// Executes the specified command by invoking the specified handler asynchronously.
        /// </summary>
        /// <typeparam name="TCommand">Type of the message.</typeparam>
        /// <param name="message">Message to handle.</param>        
        /// <param name="handler">MessageHandler that will be used to execute the command.</param>  
        /// <returns>The <see cref="Task" /> that is executing the command.</returns>        
        /// <exception cref="ArgumentNullException">
        /// <paramref name="message"/> is <c>null</c>.
        /// </exception>        
        Task ExecuteAsync<TCommand>(TCommand message, IMessageHandler<TCommand> handler) where TCommand : class, IRequestMessage<TCommand>; 

        /// <summary>
        /// Executes the specified command by invoking the specified handler asynchronously.
        /// </summary>
        /// <typeparam name="TCommand">Type of the message.</typeparam>
        /// <param name="message">Message to handle.</param>        
        /// <param name="handler">MessageHandler that will be used to execute the command.</param>
        /// <param name="token">
        /// Optional token that can be used to cancel the operation.
        /// </param>      
        /// <returns>The <see cref="Task" /> that is executing the command.</returns>            
        /// <exception cref="ArgumentNullException">
        /// <paramref name="message"/> is <c>null</c>.
        /// </exception>               
        Task ExecuteAsync<TCommand>(TCommand message, IMessageHandler<TCommand> handler, CancellationToken? token) where TCommand : class, IRequestMessage<TCommand>; 

        /// <summary>
        /// Executes the specified command by invoking all registered message handlers.
        /// </summary>
        /// <typeparam name="TCommand">Type of the message.</typeparam>
        /// <param name="message">Message to handle.</param>                
        /// <exception cref="ArgumentNullException">
        /// <paramref name="message"/> is <c>null</c>.
        /// </exception>
        /// <exception cref="FunctionalException">
        /// The <paramref name="message"/> or the sender of the <paramref name="message"/> did not meet
        /// the preconditions that are in effect for this message to process.
        /// </exception>
        void Execute<TCommand>(TCommand message) where TCommand : class, IRequestMessage<TCommand>; 

        /// <summary>
        /// Executes the specified command by invoking all registered message handlers.
        /// </summary>
        /// <typeparam name="TCommand">Type of the message.</typeparam>
        /// <param name="message">Message to handle.</param>        
        /// <param name="token">
        /// Optional token that can be used to cancel the operation.
        /// </param>          
        /// <exception cref="ArgumentNullException">
        /// <paramref name="message"/> is <c>null</c>.
        /// </exception>
        /// <exception cref="FunctionalException">
        /// The <paramref name="message"/> or the sender of the <paramref name="message"/> did not meet
        /// the preconditions that are in effect for this message to process.
        /// </exception>
        /// <exception cref="OperationCanceledException">
        /// <paramref name="token"/> was specified and used to cancel the execution.
        /// </exception>         
        void Execute<TCommand>(TCommand message, CancellationToken? token) where TCommand : class, IRequestMessage<TCommand>; 

        /// <summary>
        /// Executes the specified command by invoking the specified delegate.
        /// </summary>
        /// <typeparam name="TCommand">Type of the message.</typeparam>
        /// <param name="message">Message to handle.</param>        
        /// <param name="handler">Delegate that will be used to execute the command.</param>        
        /// <exception cref="ArgumentNullException">
        /// <paramref name="message"/> is <c>null</c>.
        /// </exception>
        /// <exception cref="FunctionalException">
        /// The <paramref name="message"/> or the sender of the <paramref name="message"/> did not meet
        /// the preconditions that are in effect for this message to process.
        /// </exception>
        void Execute<TCommand>(TCommand message, Action<TCommand> handler) where TCommand : class, IRequestMessage<TCommand>; 

        /// <summary>
        /// Executes the specified command by invoking the specified delegate.
        /// </summary>
        /// <typeparam name="TCommand">Type of the message.</typeparam>
        /// <param name="message">Message to handle.</param>        
        /// <param name="handler">Delegate that will be used to execute the command.</param>
        /// <param name="token">
        /// Optional token that can be used to cancel the operation.
        /// </param>                
        /// <exception cref="ArgumentNullException">
        /// <paramref name="message"/> is <c>null</c>.
        /// </exception>
        /// <exception cref="FunctionalException">
        /// The <paramref name="message"/> or the sender of the <paramref name="message"/> did not meet
        /// the preconditions that are in effect for this message to process.
        /// </exception>
        /// <exception cref="OperationCanceledException">
        /// <paramref name="token"/> was specified and used to cancel the execution.
        /// </exception>         
        void Execute<TCommand>(TCommand message, Action<TCommand> handler, CancellationToken? token) where TCommand : class, IRequestMessage<TCommand>; 

        /// <summary>
        /// Executes the specified command by invoking the specified handler.
        /// </summary>
        /// <typeparam name="TCommand">Type of the message.</typeparam>
        /// <param name="message">Message to handle.</param>        
        /// <param name="handler">MessageHandler that will be used to execute the command.</param>        
        /// <exception cref="ArgumentNullException">
        /// <paramref name="message"/> is <c>null</c>.
        /// </exception>
        /// <exception cref="FunctionalException">
        /// The <paramref name="message"/> or the sender of the <paramref name="message"/> did not meet
        /// the preconditions that are in effect for this message to process.
        /// </exception>
        void Execute<TCommand>(TCommand message, IMessageHandler<TCommand> handler) where TCommand : class, IRequestMessage<TCommand>; 

        /// <summary>
        /// Executes the specified command by invoking the specified handler.
        /// </summary>
        /// <typeparam name="TCommand">Type of the message.</typeparam>
        /// <param name="message">Message to handle.</param>        
        /// <param name="handler">MessageHandler that will be used to execute the command.</param>
        /// <param name="token">
        /// Optional token that can be used to cancel the operation.
        /// </param>                
        /// <exception cref="ArgumentNullException">
        /// <paramref name="message"/> is <c>null</c>.
        /// </exception>
        /// <exception cref="FunctionalException">
        /// The <paramref name="message"/> or the sender of the <paramref name="message"/> did not meet
        /// the preconditions that are in effect for this message to process.
        /// </exception>
        /// <exception cref="OperationCanceledException">
        /// <paramref name="token"/> was specified and used to cancel the execution.
        /// </exception>         
        void Execute<TCommand>(TCommand message, IMessageHandler<TCommand> handler, CancellationToken? token) where TCommand : class, IRequestMessage<TCommand>; 

        #endregion

        #region [====== Queries ======]

        /// <summary>
        /// Executes the specified <paramref name="query"/> using the specified <paramref name="message"/> and returns its result asynchronously.
        /// </summary>
        /// <typeparam name="TMessageIn">Type of the message going into the query.</typeparam>
        /// <typeparam name="TMessageOut">Type of the message returned by the query.</typeparam>
        /// <param name="message">Message containing the parameters of this query.</param>
        /// <param name="query">The query to execute.</param>
        /// <returns>The <see cref="Task{TMessageOut}" /> that is executing the <paramref name="query"/>.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="message"/> or <paramref name="query"/> is <c>null</c>.
        /// </exception>        
        Task<TMessageOut> ExecuteAsync<TMessageIn, TMessageOut>(TMessageIn message, Func<TMessageIn, TMessageOut> query) where TMessageIn : class, IRequestMessage<TMessageIn>;           

        /// <summary>
        /// Executes the specified <paramref name="query"/> using the specified <paramref name="message"/> and returns its result asynchronously.
        /// </summary>
        /// <typeparam name="TMessageIn">Type of the message going into the query.</typeparam>
        /// <typeparam name="TMessageOut">Type of the message returned by the query.</typeparam>
        /// <param name="message">Message containing the parameters of this query.</param>
        /// <param name="query">The query to execute.</param>
        /// <param name="token">
        /// Optional token that can be used to cancel the operation.
        /// </param> 
        /// <returns>The <see cref="Task{TMessageOut}" /> that is executing the <paramref name="query"/>.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="message"/> or <paramref name="query"/> is <c>null</c>.
        /// </exception>                 
        Task<TMessageOut> ExecuteAsync<TMessageIn, TMessageOut>(TMessageIn message, Func<TMessageIn, TMessageOut> query, CancellationToken? token) where TMessageIn : class, IRequestMessage<TMessageIn>;         

        /// <summary>
        /// Executes the specified <paramref name="query"/> using the specified <paramref name="message"/> and returns its result asynchronously.
        /// </summary>
        /// <typeparam name="TMessageIn">Type of the message going into the query.</typeparam>
        /// <typeparam name="TMessageOut">Type of the message returned by the query.</typeparam>
        /// <param name="message">Message containing the parameters of this query.</param>
        /// <param name="query">The query to execute.</param>
        /// <returns>The <see cref="Task{TMessageOut}" /> that is executing the <paramref name="query"/>.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="message"/> or <paramref name="query"/> is <c>null</c>.
        /// </exception>        
        Task<TMessageOut> ExecuteAsync<TMessageIn, TMessageOut>(TMessageIn message, IQuery<TMessageIn, TMessageOut> query) where TMessageIn : class, IRequestMessage<TMessageIn>;            

        /// <summary>
        /// Executes the specified <paramref name="query"/> using the specified <paramref name="message"/> and returns its result asynchronously.
        /// </summary>
        /// <typeparam name="TMessageIn">Type of the message going into the query.</typeparam>
        /// <typeparam name="TMessageOut">Type of the message returned by the query.</typeparam>
        /// <param name="message">Message containing the parameters of this query.</param>
        /// <param name="query">The query to execute.</param>
        /// <param name="token">
        /// Optional token that can be used to cancel the operation.
        /// </param> 
        /// <returns>The <see cref="Task{TMessageOut}" /> that is executing the <paramref name="query"/>.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="message"/> or <paramref name="query"/> is <c>null</c>.
        /// </exception>                
        Task<TMessageOut> ExecuteAsync<TMessageIn, TMessageOut>(TMessageIn message, IQuery<TMessageIn, TMessageOut> query, CancellationToken? token) where TMessageIn : class, IRequestMessage<TMessageIn>;           

        /// <summary>
        /// Executes the specified <paramref name="query"/> using the specified <paramref name="message"/> and returns its result.
        /// </summary>
        /// <typeparam name="TMessageIn">Type of the message going into the query.</typeparam>
        /// <typeparam name="TMessageOut">Type of the message returned by the query.</typeparam>
        /// <param name="message">Message containing the parameters of this query.</param>
        /// <param name="query">The query to execute.</param>
        /// <returns>The result of the <paramref name="query"/>.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="message"/> or <paramref name="query"/> is <c>null</c>.
        /// </exception>
        /// <exception cref="FunctionalException">
        /// The <paramref name="message"/> or the sender of the <paramref name="message"/> did not meet
        /// the preconditions that are in effect for this message to process.
        /// </exception>
        TMessageOut Execute<TMessageIn, TMessageOut>(TMessageIn message, Func<TMessageIn, TMessageOut> query) where TMessageIn : class, IRequestMessage<TMessageIn>;

        /// <summary>
        /// Executes the specified <paramref name="query"/> using the specified <paramref name="message"/> and returns its result.
        /// </summary>
        /// <typeparam name="TMessageIn">Type of the message going into the query.</typeparam>
        /// <typeparam name="TMessageOut">Type of the message returned by the query.</typeparam>
        /// <param name="message">Message containing the parameters of this query.</param>
        /// <param name="query">The query to execute.</param>
        /// <param name="token">
        /// Optional token that can be used to cancel the operation.
        /// </param> 
        /// <returns>The result of the <paramref name="query"/>.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="message"/> or <paramref name="query"/> is <c>null</c>.
        /// </exception>
        /// <exception cref="FunctionalException">
        /// The <paramref name="message"/> or the sender of the <paramref name="message"/> did not meet
        /// the preconditions that are in effect for this message to process.
        /// </exception>
        /// <exception cref="OperationCanceledException">
        /// <paramref name="token"/> was specified and used to cancel the execution.
        /// </exception>  
        TMessageOut Execute<TMessageIn, TMessageOut>(TMessageIn message, Func<TMessageIn, TMessageOut> query, CancellationToken? token) where TMessageIn : class, IRequestMessage<TMessageIn>;

        /// <summary>
        /// Executes the specified <paramref name="query"/> using the specified <paramref name="message"/> and returns its result.
        /// </summary>
        /// <typeparam name="TMessageIn">Type of the message going into the query.</typeparam>
        /// <typeparam name="TMessageOut">Type of the message returned by the query.</typeparam>
        /// <param name="message">Message containing the parameters of this query.</param>
        /// <param name="query">The query to execute.</param>
        /// <returns>The result of the <paramref name="query"/>.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="message"/> or <paramref name="query"/> is <c>null</c>.
        /// </exception>
        /// <exception cref="FunctionalException">
        /// The <paramref name="message"/> or the sender of the <paramref name="message"/> did not meet
        /// the preconditions that are in effect for this message to process.
        /// </exception>
        TMessageOut Execute<TMessageIn, TMessageOut>(TMessageIn message, IQuery<TMessageIn, TMessageOut> query) where TMessageIn : class, IRequestMessage<TMessageIn>;

        /// <summary>
        /// Executes the specified <paramref name="query"/> using the specified <paramref name="message"/> and returns its result.
        /// </summary>
        /// <typeparam name="TMessageIn">Type of the message going into the query.</typeparam>
        /// <typeparam name="TMessageOut">Type of the message returned by the query.</typeparam>
        /// <param name="message">Message containing the parameters of this query.</param>
        /// <param name="query">The query to execute.</param>
        /// <param name="token">
        /// Optional token that can be used to cancel the operation.
        /// </param>  
        /// <returns>The result of the <paramref name="query"/>.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="message"/> or <paramref name="query"/> is <c>null</c>.
        /// </exception>
        /// <exception cref="FunctionalException">
        /// The <paramref name="message"/> or the sender of the <paramref name="message"/> did not meet
        /// the preconditions that are in effect for this message to process.
        /// </exception>
        /// <exception cref="OperationCanceledException">
        /// <paramref name="token"/> was specified and used to cancel the execution.
        /// </exception>  
        TMessageOut Execute<TMessageIn, TMessageOut>(TMessageIn message, IQuery<TMessageIn, TMessageOut> query, CancellationToken? token) where TMessageIn : class, IRequestMessage<TMessageIn>;           

        #endregion

        #region [====== Events ======]

        /// <summary>
        /// Processes the specified message by invoking all registered message handlers asynchronously.
        /// </summary>
        /// <typeparam name="TMessage">Type of the message.</typeparam>
        /// <param name="message">Message to handle.</param>          
        /// <returns>The <see cref="Task" /> that is handling the <paramref name="message"/>.</returns>      
        /// <exception cref="ArgumentNullException">
        /// <paramref name="message"/> is <c>null</c>.
        /// </exception>        
        Task HandleAsync<TMessage>(TMessage message) where TMessage : class, IMessage<TMessage>;

        /// <summary>
        /// Processes the specified message by invoking all registered message handlers asynchronously.
        /// </summary>
        /// <typeparam name="TMessage">Type of the message.</typeparam>
        /// <param name="message">Message to handle.</param>
        /// <param name="validator">Optional validator of the message.</param>  
        /// <returns>The <see cref="Task" /> that is handling the <paramref name="message"/>.</returns>       
        /// <exception cref="ArgumentNullException">
        /// <paramref name="message"/> is <c>null</c>.
        /// </exception>        
        Task HandleAsync<TMessage>(TMessage message, IMessageValidator<TMessage> validator) where TMessage : class, IMessage<TMessage>;

        /// <summary>
        /// Processes the specified message by invoking all registered message handlers asynchronously.
        /// </summary>
        /// <typeparam name="TMessage">Type of the message.</typeparam>
        /// <param name="message">Message to handle.</param>
        /// <param name="validator">Optional validator of the message.</param>
        /// <param name="token">
        /// Optional token that can be used to cancel the operation.
        /// </param>        
        /// <returns>The <see cref="Task" /> that is handling the <paramref name="message"/>.</returns>   
        /// <exception cref="ArgumentNullException">
        /// <paramref name="message"/> is <c>null</c>.
        /// </exception>                
        Task HandleAsync<TMessage>(TMessage message, IMessageValidator<TMessage> validator, CancellationToken? token) where TMessage : class, IMessage<TMessage>;

        /// <summary>
        /// Processes the specified message by invoking the specified delegate asynchronously.
        /// </summary>
        /// <typeparam name="TMessage">Type of the message.</typeparam>
        /// <param name="message">Message to handle.</param>
        /// <param name="validator">Optional validator of the message.</param>
        /// <param name="handler">Delegate that will be used to handle the message.</param>  
        /// <returns>The <see cref="Task" /> that is handling the <paramref name="message"/>.</returns>       
        /// <exception cref="ArgumentNullException">
        /// <paramref name="message"/> is <c>null</c>.
        /// </exception>        
        Task HandleAsync<TMessage>(TMessage message, IMessageValidator<TMessage> validator, Action<TMessage> handler) where TMessage : class, IMessage<TMessage>;

        /// <summary>
        /// Processes the specified message by invoking the specified delegate asynchronously.
        /// </summary>
        /// <typeparam name="TMessage">Type of the message.</typeparam>
        /// <param name="message">Message to handle.</param>
        /// <param name="validator">Optional validator of the message.</param>
        /// <param name="handler">Delegate that will be used to handle the message.</param>
        /// <param name="token">
        /// Optional token that can be used to cancel the operation.
        /// </param>    
        /// <returns>The <see cref="Task" /> that is handling the <paramref name="message"/>.</returns>             
        /// <exception cref="ArgumentNullException">
        /// <paramref name="message"/> is <c>null</c>.
        /// </exception>                
        Task HandleAsync<TMessage>(TMessage message, IMessageValidator<TMessage> validator, Action<TMessage> handler, CancellationToken? token) where TMessage : class, IMessage<TMessage>;

        /// <summary>
        /// Processes the specified message by invoking the specified handler asynchronously.
        /// </summary>
        /// <typeparam name="TMessage">Type of the message.</typeparam>
        /// <param name="message">Message to handle.</param>
        /// <param name="validator">Optional validator of the message.</param>
        /// <param name="handler">Handler that will be used to handle the message.</param>   
        /// <returns>The <see cref="Task" /> that is handling the <paramref name="message"/>.</returns>      
        /// <exception cref="ArgumentNullException">
        /// <paramref name="message"/> is <c>null</c>.
        /// </exception>        
        Task HandleAsync<TMessage>(TMessage message, IMessageValidator<TMessage> validator, IMessageHandler<TMessage> handler) where TMessage : class, IMessage<TMessage>;

        /// <summary>
        /// Processes the specified message by invoking the specified handler asynchronously.
        /// </summary>
        /// <typeparam name="TMessage">Type of the message.</typeparam>
        /// <param name="message">Message to handle.</param>
        /// <param name="validator">Optional validator of the message.</param>
        /// <param name="handler">Handler that will be used to handle the message.</param>
        /// <param name="token">
        /// Optional token that can be used to cancel the operation.
        /// </param>  
        /// <returns>The <see cref="Task" /> that is handling the <paramref name="message"/>.</returns>               
        /// <exception cref="ArgumentNullException">
        /// <paramref name="message"/> is <c>null</c>.
        /// </exception>                 
        Task HandleAsync<TMessage>(TMessage message, IMessageValidator<TMessage> validator, IMessageHandler<TMessage> handler, CancellationToken? token) where TMessage : class, IMessage<TMessage>;

        /// <summary>
        /// Processes the specified message by invoking all registered message handlers.
        /// </summary>
        /// <typeparam name="TMessage">Type of the message.</typeparam>
        /// <param name="message">Message to handle.</param>                
        /// <exception cref="ArgumentNullException">
        /// <paramref name="message"/> is <c>null</c>.
        /// </exception>
        /// <exception cref="FunctionalException">
        /// The <paramref name="message"/> or the sender of the <paramref name="message"/> did not meet
        /// the preconditions that are in effect for this message to process.
        /// </exception>
        void Handle<TMessage>(TMessage message) where TMessage : class, IMessage<TMessage>;

        /// <summary>
        /// Processes the specified message by invoking all registered message handlers.
        /// </summary>
        /// <typeparam name="TMessage">Type of the message.</typeparam>
        /// <param name="message">Message to handle.</param>
        /// <param name="validator">Optional validator of the message.</param>        
        /// <exception cref="ArgumentNullException">
        /// <paramref name="message"/> is <c>null</c>.
        /// </exception>
        /// <exception cref="FunctionalException">
        /// The <paramref name="message"/> or the sender of the <paramref name="message"/> did not meet
        /// the preconditions that are in effect for this message to process.
        /// </exception>
        void Handle<TMessage>(TMessage message, IMessageValidator<TMessage> validator) where TMessage : class, IMessage<TMessage>;

        /// <summary>
        /// Processes the specified message by invoking all registered message handlers.
        /// </summary>
        /// <typeparam name="TMessage">Type of the message.</typeparam>
        /// <param name="message">Message to handle.</param>
        /// <param name="validator">Optional validator of the message.</param>
        /// <param name="token">
        /// Optional token that can be used to cancel the operation.
        /// </param>          
        /// <exception cref="ArgumentNullException">
        /// <paramref name="message"/> is <c>null</c>.
        /// </exception>
        /// <exception cref="FunctionalException">
        /// The <paramref name="message"/> or the sender of the <paramref name="message"/> did not meet
        /// the preconditions that are in effect for this message to process.
        /// </exception>
        /// <exception cref="OperationCanceledException">
        /// <paramref name="token"/> was specified and used to cancel the execution.
        /// </exception>         
        void Handle<TMessage>(TMessage message, IMessageValidator<TMessage> validator, CancellationToken? token) where TMessage : class, IMessage<TMessage>;

        /// <summary>
        /// Processes the specified message by invoking the specified delegate.
        /// </summary>
        /// <typeparam name="TMessage">Type of the message.</typeparam>
        /// <param name="message">Message to handle.</param>
        /// <param name="validator">Optional validator of the message.</param>
        /// <param name="handler">Delegate that will be used to handle the message.</param>        
        /// <exception cref="ArgumentNullException">
        /// <paramref name="message"/> is <c>null</c>.
        /// </exception>
        /// <exception cref="FunctionalException">
        /// The <paramref name="message"/> or the sender of the <paramref name="message"/> did not meet
        /// the preconditions that are in effect for this message to process.
        /// </exception>
        void Handle<TMessage>(TMessage message, IMessageValidator<TMessage> validator, Action<TMessage> handler) where TMessage : class, IMessage<TMessage>;

        /// <summary>
        /// Processes the specified message by invoking the specified delegate.
        /// </summary>
        /// <typeparam name="TMessage">Type of the message.</typeparam>
        /// <param name="message">Message to handle.</param>
        /// <param name="validator">Optional validator of the message.</param>
        /// <param name="handler">Delegate that will be used to handle the message.</param>
        /// <param name="token">
        /// Optional token that can be used to cancel the operation.
        /// </param>                
        /// <exception cref="ArgumentNullException">
        /// <paramref name="message"/> is <c>null</c>.
        /// </exception>
        /// <exception cref="FunctionalException">
        /// The <paramref name="message"/> or the sender of the <paramref name="message"/> did not meet
        /// the preconditions that are in effect for this message to process.
        /// </exception>
        /// <exception cref="OperationCanceledException">
        /// <paramref name="token"/> was specified and used to cancel the execution.
        /// </exception>         
        void Handle<TMessage>(TMessage message, IMessageValidator<TMessage> validator, Action<TMessage> handler, CancellationToken? token) where TMessage : class, IMessage<TMessage>;

        /// <summary>
        /// Processes the specified message by invoking the specified handler.
        /// </summary>
        /// <typeparam name="TMessage">Type of the message.</typeparam>
        /// <param name="message">Message to handle.</param>
        /// <param name="validator">Optional validator of the message.</param>
        /// <param name="handler">Handler that will be used to handle the message.</param>        
        /// <exception cref="ArgumentNullException">
        /// <paramref name="message"/> is <c>null</c>.
        /// </exception>
        /// <exception cref="FunctionalException">
        /// The <paramref name="message"/> or the sender of the <paramref name="message"/> did not meet
        /// the preconditions that are in effect for this message to process.
        /// </exception>
        void Handle<TMessage>(TMessage message, IMessageValidator<TMessage> validator, IMessageHandler<TMessage> handler) where TMessage : class, IMessage<TMessage>;

        /// <summary>
        /// Processes the specified message by invoking the specified handler.
        /// </summary>
        /// <typeparam name="TMessage">Type of the message.</typeparam>
        /// <param name="message">Message to handle.</param>
        /// <param name="validator">Optional validator of the message.</param>
        /// <param name="handler">Handler that will be used to handle the message.</param>
        /// <param name="token">
        /// Optional token that can be used to cancel the operation.
        /// </param>                
        /// <exception cref="ArgumentNullException">
        /// <paramref name="message"/> is <c>null</c>.
        /// </exception>
        /// <exception cref="FunctionalException">
        /// The <paramref name="message"/> or the sender of the <paramref name="message"/> did not meet
        /// the preconditions that are in effect for this message to process.
        /// </exception>
        /// <exception cref="OperationCanceledException">
        /// <paramref name="token"/> was specified and used to cancel the execution.
        /// </exception>         
        void Handle<TMessage>(TMessage message, IMessageValidator<TMessage> validator, IMessageHandler<TMessage> handler, CancellationToken? token) where TMessage : class, IMessage<TMessage>;        

        #endregion
    }
}
