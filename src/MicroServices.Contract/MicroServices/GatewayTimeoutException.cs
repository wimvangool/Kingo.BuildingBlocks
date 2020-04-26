﻿using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Security.Permissions;
using System.Text;

namespace Kingo.MicroServices
{
    /// <summary>
    /// This exception is thrown by a service when an internal timeout occurred and the operation was cancelled as a result.
    /// </summary>
    [Serializable]
    public class GatewayTimeoutException : InternalServerErrorException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GatewayTimeoutException" /> class.
        /// </summary>
        /// <param name="operationStackTrace">The stack trace of the processor at the time the exception was thrown.</param> 
        /// <param name="message">Message of the exception.</param>
        /// <param name="innerException">Cause of this exception.</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="operationStackTrace"/> is <c>null</c>.
        /// </exception>
        public GatewayTimeoutException(MicroProcessorOperationStackTrace operationStackTrace, string message = null, Exception innerException = null) :
            base(operationStackTrace, message, innerException) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="GatewayTimeoutException" /> class.
        /// </summary>
        /// <param name="info">The serialization info.</param>
        /// <param name="context">The streaming context.</param>
        [SecurityPermission(SecurityAction.Demand, SerializationFormatter = true)]
        protected GatewayTimeoutException(SerializationInfo info, StreamingContext context) :
            base(info, context) { }

        /// <summary>
        /// Returns <c>504</c>, indicating an upstream timeout occurred while processing the request.
        /// </summary>
        public override int ErrorCode =>
            504;
    }
}