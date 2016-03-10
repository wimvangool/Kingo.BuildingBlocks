﻿using System;

namespace Kingo.Messaging.Domain
{
    /// <summary>
    /// Represents an event that is mapped to its contract.
    /// </summary>
    /// <typeparam name="TKey">Key-type of an aggregate.</typeparam>
    /// <typeparam name="TVersion">Version-type of an aggregate.</typeparam>
    public sealed class EventToSave<TKey, TVersion> : SnapshotOrEventToSave<TKey, TVersion>
        where TVersion : struct, IEquatable<TVersion>, IComparable<TVersion>
    {
        private readonly ITypeToContractMap _typeToContractMap;
        private readonly IHasKeyAndVersion<TKey, TVersion> _event;

        internal EventToSave(ITypeToContractMap typeToContractMap, IHasKeyAndVersion<TKey, TVersion> @event)
        {
            if (typeToContractMap == null)
            {
                throw new ArgumentNullException(nameof(typeToContractMap));
            }
            if (@event == null)
            {
                throw new ArgumentNullException(nameof(@event));
            }
            _typeToContractMap = typeToContractMap;
            _event = @event;
        }

        internal override ITypeToContractMap TypeToContractMap
        {
            get { return _typeToContractMap; }
        }

        internal override IHasKeyAndVersion<TKey, TVersion> VersionedObject
        {
            get { return _event; }
        }

        /// <summary>
        /// Returns the event that was published by the aggregate.
        /// </summary>
        public IHasKeyAndVersion<TKey, TVersion> Value
        {
            get { return _event; }
        }
    }
}