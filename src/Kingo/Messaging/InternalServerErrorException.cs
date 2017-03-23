﻿using System;
using System.Runtime.Serialization;
using System.Security.Permissions;
using Kingo.Resources;

namespace Kingo.Messaging
{
    /// <summary>
    /// This exception is thrown by a <see cref="IMicroProcessor" /> when a technical failure prevented the processor from
    /// handling a message or executing a query correctly. This type semantically maps to HTTP response code <c>500</c>.
    /// </summary>
    [Serializable]
    public class InternalServerErrorException : ExternalProcessorException
    {        
        /// <summary>
        /// Initializes a new instance of the <see cref="InternalServerErrorException" /> class.
        /// </summary>
        /// <param name="failedMessage">The message that could not be processed.</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="failedMessage"/> is <c>null</c>.
        /// </exception>
        public InternalServerErrorException(object failedMessage) :
            base(failedMessage) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="InternalServerErrorException" /> class.
        /// </summary>
        /// <param name="failedMessage">The message that could not be processed.</param>
        /// <param name="message">Message of the exception.</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="failedMessage"/> is <c>null</c>.
        /// </exception>
        public InternalServerErrorException(object failedMessage, string message)
            : base(failedMessage, message) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="InternalServerErrorException" /> class.
        /// </summary>
        /// <param name="failedMessage">The message that could not be processed.</param>
        /// <param name="message">Message of the exception.</param>
        /// <param name="innerException">Cause of this exception.</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="failedMessage"/> is <c>null</c>.
        /// </exception>
        internal InternalServerErrorException(object failedMessage, string message, Exception innerException)
            : base(failedMessage, message, innerException) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="InternalServerErrorException" /> class.
        /// </summary>
        /// <param name="info">The serialization info.</param>
        /// <param name="context">The streaming context.</param>
        [SecurityPermission(SecurityAction.Demand, SerializationFormatter = true)]
        protected InternalServerErrorException(SerializationInfo info, StreamingContext context)
            : base(info, context) { }
        
        internal static InternalServerErrorException FromInnerException(object failedMessage, Exception innerException)
        {
            var messageFormat = ExceptionMessages.InternalServerErrorException_FromException;
            var message = string.Format(messageFormat, failedMessage.GetType().FriendlyName());
            return new InternalServerErrorException(failedMessage, message, innerException);
        }
    }
}
