﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Kingo.MicroServices
{
    /// <summary>
    /// When implemented by a class, represents a builder of <see cref="IMessage" /> instances.
    /// </summary>
    public interface IMessageBuilder : IMessageFactory
    {
        /// <summary>
        /// Gets or sets the message identifier. If not specified, the builder will generate a new message-id.
        /// </summary>
        string MessageId
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the identifier of the message that the message is correlated with.
        /// </summary>
        string CorrelationId
        {
            get;
            set;
        }
    }
}
