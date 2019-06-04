﻿using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using Kingo.Reflection;

namespace Kingo.MicroServices
{        
    internal sealed class MessageHandlerClass : IMessageHandlerFactory
    {
        #region [====== Registration ======]

        private abstract class RegistrationWrapper
        {
            public abstract object MessageHandler
            {
                get;
            }

            public override string ToString() =>
                MessageHandler.ToString();
        }

        private sealed class RegistrationWrapper<TMessageHandler> : RegistrationWrapper where TMessageHandler : class
        {
            private readonly TMessageHandler _messageHandler;

            public RegistrationWrapper(TMessageHandler messageHandler)
            {
                _messageHandler = messageHandler ?? throw new ArgumentNullException(nameof(messageHandler));
            }

            public override object MessageHandler =>
                _messageHandler;
        }

        #endregion

        private MessageHandlerClass(Type type, IEnumerable<Type> interfaces, IMessageHandlerConfiguration configuration)
        {           
            Type = type;
            RegistrationType = typeof(RegistrationWrapper<>).MakeGenericType(type);
            Interfaces = interfaces.Select(interfaceType => new MessageHandlerInterface(interfaceType)).ToArray();
            Configuration = configuration;            
        }        

        public Type Type
        {
            get;
        }

        public Type RegistrationType
        {
            get;
        }

        public IReadOnlyList<MessageHandlerInterface> Interfaces
        {
            get;
        }

        public IMessageHandlerConfiguration Configuration
        {
            get;
        }
        
        public override string ToString() =>
            $"{Type.FriendlyName()} ({Interfaces.Count} interface(s) implemented) - Configuration = {Configuration}";

        public IEnumerable<MessageHandler> ResolveMessageHandlers<TMessage>(TMessage message, MessageHandlerContext context)
        {
            if (ReferenceEquals(message, null))
            {
                throw new ArgumentNullException(nameof(message));
            }
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }
            if (context.Operation.IsSupported(Configuration.SupportedOperationTypes))
            {
                // This LINQ construct first selects all message handler interface definitions that are compatible with
                // the specified message. Then it will dynamically create the correct message handler type for each match
                // and return it.
                //
                // Example:
                //   When the class implements both IMessageHandler<object> and IMessageHandler<SomeMessage>, then if
                //   message is of type SomeMessage, two message-handler instances are returned, one for each
                //   implementation.
                return
                    from messageHandlerInterface in Interfaces                    
                    where messageHandlerInterface.MessageType.IsInstanceOfType(message)
                    select CreateMessageHandler(message, context, messageHandlerInterface);
            }
            return Enumerable.Empty<MessageHandler>();
        }

        private static readonly ConcurrentDictionary<MessageHandlerInterface, Func<object, object, MessageHandlerContext, Type, Type, MessageHandler>> _MessageHandlerFactories =
            new ConcurrentDictionary<MessageHandlerInterface, Func<object, object, MessageHandlerContext, Type, Type, MessageHandler>>();

        private MessageHandler CreateMessageHandler<TMessage>(TMessage message, MessageHandlerContext context, MessageHandlerInterface messageHandlerInterface) =>
            _MessageHandlerFactories.GetOrAdd(messageHandlerInterface, CreateMessageHandlerFactory).Invoke(ResolveMessageHandler(context), message, context, Type, messageHandlerInterface.InterfaceType);                                             
        
        private static Func<object, object, MessageHandlerContext, Type, Type, MessageHandler> CreateMessageHandlerFactory(MessageHandlerInterface messageHandlerInterface)
        {
            // In order to support message handlers with contravariant IMessageHandler<TMessage>-implementations, the resolved instance is wrapped
            // inside an MessageHandlerDecorator<TActual> where TActual is the type of the generic type parameter of the specified interface. This ensures that
            // the correct interface method of the handler is called at runtime.
            //
            // For example, suppose a message handler is declared as follows...
            //
            // public sealed class ObjectHandler : IMessageHandler<object>
            // {
            //     public Task HandleAsync(object message, MessageHandlerContext context) 
            //     {
            //         ...
            //     }    
            // }
            //
            // ...and this method is called with a TMessage of type 'string', then the code will do something along the following lines:
            //            
            // - return new MessageHandlerDecorator<object>(Resolve<ObjectHandler>(), message, context, typeof(ObjectHandler), typeof(IMessageHandler<object>));            
            var handlerParameter = Expression.Parameter(typeof(object), "handler");
            var handlerOfExpectedType = Expression.Convert(handlerParameter, messageHandlerInterface.InterfaceType);
            var messageParameter = Expression.Parameter(typeof(object), "message");
            var messageOfExpectedType = Expression.Convert(messageParameter, messageHandlerInterface.MessageType);
            var contextParameter = Expression.Parameter(typeof(MessageHandlerContext), "context");
            var typeParameter = Expression.Parameter(typeof(Type), "type");
            var interfaceTypeParameter = Expression.Parameter(typeof(Type), "interfaceType");

            var decoratorTypeDefinition = typeof(MessageHandlerDecorator<>);
            var decoratorType = decoratorTypeDefinition.MakeGenericType(messageHandlerInterface.MessageType);
            var decoratorConstructorParameters = new [] { messageHandlerInterface.InterfaceType, messageHandlerInterface.MessageType, typeof(MessageHandlerContext), typeof(Type), typeof(Type) };
            var decoratorConstructor = decoratorType.GetConstructor(BindingFlags.NonPublic | BindingFlags.Instance, null, decoratorConstructorParameters, null);
            var newDecoratorExpression = Expression.New(decoratorConstructor, handlerOfExpectedType, messageOfExpectedType, contextParameter, typeParameter, interfaceTypeParameter);
            var newMessageHandlerExpression = Expression.Lambda<Func<object, object, MessageHandlerContext, Type, Type, MessageHandler>>(
                Expression.Convert(newDecoratorExpression, typeof(MessageHandler)),
                handlerParameter,
                messageParameter,
                contextParameter,
                typeParameter,
                interfaceTypeParameter);

            return newMessageHandlerExpression.Compile();
        }

        private object ResolveMessageHandler(MicroProcessorContext context)
        {
            var wrapper = context.ServiceProvider.GetService(RegistrationType) as RegistrationWrapper;
            if (wrapper == null)
            {
                throw NewCouldNotResolveMessageHandlerException(context.ServiceProvider.GetType(), Type);
            }
            return wrapper.MessageHandler;
        }

        private static Exception NewCouldNotResolveMessageHandlerException(Type serviceProviderType, Type messageHandlerType)
        {
            var messageFormat = ExceptionMessages.MessageHandlerClass_CouldNotResolveMessageHandler;
            var message = string.Format(messageFormat, serviceProviderType.FriendlyName(), messageHandlerType.FriendlyName());
            return new InvalidOperationException(message);
        }

        #region [====== FromTypes ======]

        private static readonly ConcurrentDictionary<Type, Type[]> _MessageHandlerInterfaceTypes = new ConcurrentDictionary<Type, Type[]>();
        private static readonly Type _MessageHandlerTypeDefinition = typeof(IMessageHandler<>);

        public static IEnumerable<MessageHandlerClass> FromTypes(IEnumerable<Type> types)
        {      
            if (types == null)
            {
                throw new ArgumentNullException(nameof(types));
            }
            foreach (var type in types.Distinct())
            {
                if (IsMessageHandlerClass(type, out var messageHandlerClass))
                {
                    yield return messageHandlerClass;
                }
            }            
        }

        internal static bool IsMessageHandlerClass(Type type, out MessageHandlerClass handler)
        {
            if (type.IsAbstract || !type.IsClass || type.ContainsGenericParameters)
            {
                handler = null;
                return false;
            }
            var interfaceTypes = GetMessageHandlerInterfaceTypesImplementedBy(type);
            if (interfaceTypes.Length == 0)
            {
                handler = null;
                return false;
            }
            handler = new MessageHandlerClass(type, interfaceTypes, DetermineMessageHandlerConfigurationOf(type));
            return true;
        }

        private static IMessageHandlerConfiguration DetermineMessageHandlerConfigurationOf(Type type)
        {
            if (TryGetMessageHandlerAttribute(type, out var configuration))
            {
                return configuration;
            }
            return MessageHandlerConfiguration.Default;
        }

        internal static bool TryGetMessageHandlerAttribute(Type classType, out IMessageHandlerConfiguration attribute)
        {
            var attributes = classType.GetCustomAttributes(typeof(MessageHandlerAttribute), true);
            if (attributes.Length == 0)
            {
                attribute = null;
                return false;
            }
            attribute = attributes.Cast<MessageHandlerAttribute>().Single();
            return true;
        }

        private static Type[] GetMessageHandlerInterfaceTypesImplementedBy(Type classType) =>
            _MessageHandlerInterfaceTypes.GetOrAdd(classType, type => type.GetInterfaces(_MessageHandlerTypeDefinition).ToArray());        

        #endregion                    
    }
}
