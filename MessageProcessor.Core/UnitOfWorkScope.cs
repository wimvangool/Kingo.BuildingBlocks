﻿using System;
using YellowFlare.MessageProcessing.Resources;

namespace YellowFlare.MessageProcessing
{    
    /// <summary>
    /// Represents a scope that controls the lifetime of a <see cref="UnitOfWorkContext" />.
    /// </summary>        
    public sealed class UnitOfWorkScope : IDisposable
    {
        private readonly BufferedEventBus _bufferedEventBus;
        private readonly bool _isContextOwner;
        private bool _hasCompleted;
        private bool _isDisposed;                            
        
        /// <summary>
        /// Initializes a new instance of the <see cref="UnitOfWorkScope" /> class.
        /// </summary>
        /// <param name="domainEventBus">Bus on which all domain-events will be published.</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="domainEventBus"/> is <c>null</c>.
        /// </exception>
        public UnitOfWorkScope(IDomainEventBus domainEventBus)
        {
            if (domainEventBus == null)
            {
                throw new ArgumentNullException("domainEventBus");
            }
            UnitOfWorkContext context;

            _isContextOwner = StartScope(out context);
            _bufferedEventBus = context.PushBus(domainEventBus);
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>        
        /// <remarks>
        /// The Dispose()-method marks the end of the current scope's lifetime and will set
        /// <see cref="UnitOfWorkContext.Current" /> back to <c>null</c> after disposing it.
        /// </remarks>
        public void Dispose()
        {
            if (_isDisposed)
            {
                return;
            }            
            EndScope(this);

            _isDisposed = true;                       
        }        

        /// <summary>
        /// Completes the scope by flushing all registered wrappers.
        /// </summary>
        /// <exception cref="ObjectDisposedException">
        /// The scope has already been disposed.
        /// </exception>
        /// <exception cref="InvalidOperationException">
        /// The scope has already been completed.
        /// </exception>
        public void Complete()
        {
            if (_isDisposed)
            {
                throw NewScopeAlreadyDisposedException();
            }
            if (_hasCompleted)
            {
                throw NewScopeAlreadyCompletedException();
            }
            var context = UnitOfWorkContext.Current;
            if (context == null)
            {
                throw NewIncorrectNestingOrWrongThreadException();
            }
            if (_isContextOwner)
            {
                context.Flush();
            }
            _hasCompleted = true;                                       
        }        

        private static bool StartScope(out UnitOfWorkContext context)
        {
            context = UnitOfWorkContext.Current;

            if (context == null)
            {
                context = UnitOfWorkContext.Current = new UnitOfWorkContext();
                return true;
            }
            return false;
        }

        private static void EndScope(UnitOfWorkScope scope)
        {
            var context = UnitOfWorkContext.Current;
            if (context == null)
            {
                throw NewIncorrectNestingOrWrongThreadException();
            }
            var bufferedEventBus = context.PopBus();
            if (bufferedEventBus != scope._bufferedEventBus)
            {
                throw NewIncorrectNestingOrWrongThreadException();
            }
            if (scope._isContextOwner)
            {
                context.Dispose();

                UnitOfWorkContext.Current = null;
            }
        }

        private static Exception NewScopeAlreadyDisposedException()
        {
            return new ObjectDisposedException(typeof(UnitOfWorkScope).Name);
        }                               

        private static Exception NewScopeAlreadyCompletedException()
        {
            return new InvalidOperationException(ExceptionMessages.UnitOfWorkScope_ScopeAlreadyCompleted);
        }

        private static Exception NewIncorrectNestingOrWrongThreadException()
        {
            return new InvalidOperationException(ExceptionMessages.UnitOfWorkScope_IncorrectNestingOrWrongThread);
        }
    }
}

