﻿using System;

namespace Kingo.Messaging.Domain
{
    internal sealed class DomainEventStream<TKey, TVersion> : IWritableEventStream<TKey, TVersion>        
        where TVersion : struct, IEquatable<TVersion>, IComparable<TVersion>  
    {
        private readonly UnitOfWorkContext _context;

        internal DomainEventStream(UnitOfWorkContext context)
        {
            _context = context;
        }

        public void Write<TEvent>(TEvent @event) where TEvent : class, IHasKeyAndVersion<TKey, TVersion>, IMessage
        {
            if (_context != null)
            {
                _context.Publish(@event);
            }
        }
    }
}