﻿using System;
using System.Collections.Generic;
using System.ComponentModel.Messaging.Resources;
using System.Linq;
using System.Text;

namespace System.ComponentModel.Messaging.Client.DataVirtualization
{
    /// <summary>
    /// Arguments of the <see cref="IVirtualCollectionPageLoader{T}.CountLoaded" /> event.
    /// </summary>
    public class CountLoadedEventArgs : EventArgs
    {
        /// <summary>
        /// The count that has been loaded.
        /// </summary>
        public readonly int Count;

        /// <summary>
        /// Initializes a new instance of the <see cref="CountLoadedEventArgs" /> class.
        /// </summary>
        /// <param name="count">The count that has been loaded.</param>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="count"/> is negative.
        /// </exception>
        public CountLoadedEventArgs(int count)
        {
            if (count < 0)
            {
                throw NewInvalidCountException(count);
            }
            Count = count;
        }

        private static Exception NewInvalidCountException(int count)
        {
            var messageFormat = ExceptionMessages.CountLoadedEventArgs_InvalidCount;
            var message = string.Format(messageFormat, count);
            return new ArgumentOutOfRangeException("count", message);
        }
    }
}