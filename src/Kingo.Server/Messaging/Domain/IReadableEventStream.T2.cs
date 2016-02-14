﻿using System;

namespace Kingo.Messaging.Domain
{
    /// <summary>
    /// Represents a buffered stream of events that can be flushed to another stream.
    /// </summary>
    /// <typeparam name="TKey">Type of the aggregate's key.</typeparam>
    /// <typeparam name="TVersion">Type of the aggregate's version.</typeparam>
    public interface IReadableEventStream<out TKey, out TVersion>        
        where TVersion : struct, IEquatable<TVersion>, IComparable<TVersion>
    {
        /// <summary>
        /// Writes the contents of this stream to the specified <paramref name="stream"/>.
        /// </summary>
        /// <param name="stream">The stream to flush to.</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="stream"/> is <c>null</c>.
        /// </exception>
        void WriteTo(IWritableEventStream<TKey, TVersion> stream);
    }
}