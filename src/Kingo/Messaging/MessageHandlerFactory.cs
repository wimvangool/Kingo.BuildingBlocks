﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Security;
using Kingo.Resources;

namespace Kingo.Messaging
{
    /// <summary>
    /// When implemented, represents a factory of all message-handlers that are used to handle the messages
    /// for the <see cref="MicroProcessor"/>.
    /// </summary>
    public abstract class MessageHandlerFactory : IReadOnlyCollection<MessageHandlerClass>
    {
        [DebuggerDisplay("Count = {_messageHandlers.Count}")]
        private readonly List<MessageHandlerClass> _messageHandlers;

        /// <summary>
        /// Initializes a new instance of the <see cref="MessageHandlerFactory" /> class.
        /// </summary>        
        protected MessageHandlerFactory()
        {
            _messageHandlers = new List<MessageHandlerClass>();
        }

        /// <inheritdoc />
        public int Count =>
            _messageHandlers.Count;

        /// <inheritdoc />
        public IEnumerator<MessageHandlerClass> GetEnumerator() =>
            _messageHandlers.GetEnumerator();

        /// <inheritdoc />
        IEnumerator IEnumerable.GetEnumerator() =>
            _messageHandlers.GetEnumerator();

        /// <inheritdoc />
        public override string ToString() =>
            $"{_messageHandlers.Count} MessageHandler(s) Registered";

        #region [====== Resolve ======]                            

        /// <inheritdoc />
        internal IEnumerable<MessageHandler> ResolveMessageHandlers<TMessage>(MessageSources source, TMessage message)
        {
            if (message == null)
            {
                throw new ArgumentNullException(nameof(message));
            }
            return from handlerClass in _messageHandlers
                   let handlers = handlerClass.CreateInstancesInEveryRoleFor(this, source, message)
                   from handler in handlers
                   select handler;
        }

        /// <summary>
        /// Create an instance of the requested type.
        /// </summary>
        /// <param name="type">Type to create.</param>
        /// <returns>An instance of the requested type.</returns>                       
        protected internal abstract object Resolve(Type type);

        #endregion

        #region [====== Type Registration ======]

        private const InstanceLifetime _DefaultLifetime = InstanceLifetime.PerUnitOfWork;

        /// <summary>
        /// Registers all message handlers that are found in the assemblies that match the specified search criteria.
        /// </summary>
        /// <param name="searchPattern">The pattern that is used to match specified files/assemblies.</param>        
        /// <param name="predicate">Optional predicate that is used to filter specific types from the assemblies.</param>
        /// <returns>The factory that contains all registered message handlers.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="searchPattern"/> is <c>null</c>.
        /// </exception>        
        /// <exception cref="IOException">
        /// An error occurred while reading files from the specified location(s).
        /// </exception>
        /// <exception cref="SecurityException">
        /// The caller does not have the required permission
        /// </exception>
        public MessageHandlerFactory Register(string searchPattern, Func<Type, bool> predicate) =>
            Register(searchPattern, null, predicate);

        /// <summary>
        /// Registers all message handlers that are found in the assemblies that match the specified search criteria.
        /// </summary>
        /// <param name="searchPattern">The pattern that is used to match specified files/assemblies.</param>
        /// <param name="path">A path pointing to a specific directory. If <c>null</c>, the <see cref="TypeSet.CurrentDirectory"/> is used.</param>
        /// <param name="predicate">Optional predicate that is used to filter specific types from the assemblies.</param>
        /// <returns>The factory that contains all registered message handlers..</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="searchPattern"/> is <c>null</c>.
        /// </exception>        
        /// <exception cref="IOException">
        /// An error occurred while reading files from the specified location(s).
        /// </exception>
        /// <exception cref="SecurityException">
        /// The caller does not have the required permission
        /// </exception>
        public MessageHandlerFactory Register(string searchPattern, string path = null, Func<Type, bool> predicate = null)
        {
            var types =
                from type in TypeSet.Empty.Add(searchPattern, path)
                where predicate == null || predicate.Invoke(type)
                select type;

            return Register(types);
        }

        /// <summary>
        /// Registers all types of the specified <paramref name="types"/> that implement
        /// <see cref="IMessageHandler{T}" /> as message handlers. The exact behavior and lifetime
        /// of these handlers will be determined by the values or their <see cref="MessageHandlerAttribute" />.
        /// </summary>
        /// <param name="types"></param>
        /// <returns></returns>
        public MessageHandlerFactory Register(IEnumerable<Type> types)
        {
            if (types == null)
            {
                throw new ArgumentNullException(nameof(types));
            }
            foreach (var messageHandler in MessageHandlerClass.RegisterMessageHandlers(this, types))
            {
                _messageHandlers.Add(messageHandler);
            }
            return this;
        }

        /// <summary>
        /// Registers the specified type <typeparamref name="T" /> with the specified <paramref name="lifetime" />.
        /// </summary>
        /// <param name="lifetime">
        /// Lifetime of the instance that will be resolved.
        /// </param>
        /// <typeparam name="T">The type to register.</typeparam>
        /// <returns>This instance.</returns>
        public MessageHandlerFactory Register<T>(InstanceLifetime lifetime = _DefaultLifetime) =>
            Register(typeof(T), lifetime);

        /// <summary>
        /// Registers the specified <paramref name="type" /> with the specified <paramref name="lifetime" />.
        /// </summary>
        /// <param name="type">The type to register.</param>
        /// <param name="lifetime">Lifetime of the instance that will be resolved.</param>
        /// <returns>This instance.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="type"/> is <c>null</c>.
        /// </exception>
        public MessageHandlerFactory Register(Type type, InstanceLifetime lifetime = _DefaultLifetime) =>
            Register(null, type, lifetime);

        /// <summary>
        /// Registers a the specified type <typeparamref name="TTo" /> as an implementation of type <typeparamref name="TFrom" />
        /// with the specified <paramref name="lifetime" />.
        /// </summary>
        /// <param name="lifetime">Lifetime of the instance that will be resolved.</param>
        /// <typeparam name="TFrom">A base type of <typeparamref name="TTo"/> or interface implemented by <typeparamref name="TTo"/>.</typeparam>
        /// <typeparam name="TTo">The type to register.</typeparam>
        /// <returns>This instance.</returns>
        public MessageHandlerFactory Register<TFrom, TTo>(InstanceLifetime lifetime = _DefaultLifetime) =>
            Register(typeof(TFrom), typeof(TTo), lifetime);

        /// <summary>
        /// Registers a the specified type <paramref name="to" /> as an implementation of type <paramref name="from" />
        /// with the specified <paramref name="lifetime" />.
        /// </summary>
        /// <param name="from">A base type of <paramref name="to"/> or interface implemented by <paramref name="to"/> (optional).</param>
        /// <param name="to">The type to register.</param> 
        /// <param name="lifetime">Lifetime of the instance that will be resolved.</param>       
        /// <returns>This instance.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="to"/> is <c>null</c>.
        /// </exception>
        public MessageHandlerFactory Register(Type from, Type to, InstanceLifetime lifetime = _DefaultLifetime)
        {
            switch (lifetime)
            {
                case InstanceLifetime.PerResolve:
                    return RegisterPerResolve(from, to);                    

                case InstanceLifetime.PerUnitOfWork:
                    return RegisterPerUnitOfWork(from, to);                    

                case InstanceLifetime.PerProcessor:
                    return RegisterPerProcessor(from, to);                    

                default:
                    throw NewInvalidLifetimeSpecifiedException(lifetime);
            }
        }

        private static Exception NewInvalidLifetimeSpecifiedException(InstanceLifetime lifeTime)
        {
            var messageFormat = ExceptionMessages.MessageHandlerFactory_InvalidInstanceLifetime;
            var message = string.Format(messageFormat, lifeTime);
            return new ArgumentOutOfRangeException(message);
        }

        #endregion

        #region [====== Type Registration (Per Specific Lifetime) ======]                       

        /// <summary>
        /// Registers the specified type <paramref name="to" />. A new instance of this type will be created each time it is
        /// resolved. If <paramref name="from"/> is specified, a new instance of <paramref name="to"/> will be created
        /// when an instance of <paramref name="from"/> is requested.
        /// </summary>
        /// <param name="from">A base type of <paramref name="to"/> or interface implemented by <paramref name="to"/> (optional).</param>
        /// <param name="to">The type to register.</param>        
        /// <returns>This instance.</returns>   
        /// /// <exception cref="ArgumentNullException">
        /// <paramref name="to"/> is <c>null</c>.
        /// </exception>      
        protected abstract MessageHandlerFactory RegisterPerResolve(Type from, Type to);

        /// <summary>
        /// Registers the specified type <paramref name="to" />. Only one instance of this type will be created inside the scope of
        /// a unit of work, represented by the current <see cref="MicroProcessorContext" />. If <paramref name="from"/> is specified,
        /// an instance of <paramref name="to"/> will be resolved when an instance of <paramref name="from"/> is requested.
        /// </summary>
        /// <param name="from">A base type of <paramref name="to"/> or interface implemented by <paramref name="to"/> (optional).</param>
        /// <param name="to">The type to register.</param>        
        /// <returns>This instance.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="to"/> is <c>null</c>.
        /// </exception>     
        protected abstract MessageHandlerFactory RegisterPerUnitOfWork(Type from, Type to);

        /// <summary>
        /// Registers the specified type <paramref name="to" />. Only one instance of this type will ever be created by this factory,
        /// reflecting the singleton pattern (in the light of the owning processor). If <paramref name="from"/> is specified, an
        /// instance of <paramref name="to"/> will be resolved when an instance of <paramref name="from"/> is requested.
        /// </summary>
        /// <param name="from">A base type of <paramref name="to"/> or interface implemented by <paramref name="to"/> (optional).</param>
        /// <param name="to">The type to register.</param>        
        /// <returns>This instance.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="to"/> is <c>null</c>.
        /// </exception> 
        protected abstract MessageHandlerFactory RegisterPerProcessor(Type from, Type to);

        #endregion

        #region [====== RegisterInstance ======]

        /// <summary>
        /// Registers a certain instance as a singleton.
        /// </summary>        
        /// <param name="instance">The instance to register.</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="instance"/> is <c>null</c>.
        /// </exception> 
        public MessageHandlerFactory RegisterInstance(object instance) =>
            RegisterInstance(null, instance);

        /// <summary>
        /// Registers a certain instance as a singleton. The specified <paramref name="instance"/>
        /// will be returned when an instance of <typeparamref name="TFrom"/> is requested.
        /// </summary>
        /// <typeparam name="TFrom">A base type of <typeparamref name="TFrom"/> or interface implemented by <paramref name="instance"/>.</typeparam>        
        /// <param name="instance">The instance to register.</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="instance"/> is <c>null</c>.
        /// </exception> 
        public MessageHandlerFactory RegisterInstance<TFrom>(TFrom instance) =>
            RegisterInstance(typeof(TFrom), instance);

        /// <summary>
        /// Registers a certain instance as a singleton. If <paramref name="from"/> is specified,
        /// <paramref name="to"/> will be returned when an instance of <paramref name="from"/> is requested.
        /// </summary>
        /// <param name="from">A base type of <paramref name="to"/> or interface implemented by <paramref name="to"/> (optional).</param>
        /// <param name="to">The instance to register.</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="to"/> is <c>null</c>.
        /// </exception> 
        public abstract MessageHandlerFactory RegisterInstance(Type from, object to);

        #endregion        
    }
}
