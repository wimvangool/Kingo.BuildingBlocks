﻿using System;

namespace YellowFlare.MessageProcessing.Requests
{
    /// <summary>
    /// EventArgs for the <see cref="IQuery{T}.ExecutionSucceeded" /> event.
    /// </summary>
    public class ExecutionSucceededEventArgs<TResult> : ExecutionSucceededEventArgs
    {
        /// <summary>
        /// The result of the associated <see cref="IQuery{T}" />.
        /// </summary>
        public readonly TResult Result;		
	
        /// <summary>
        /// Initializes a new instance of the <see cref="ExecutionSucceededEventArgs" /> class.
        /// </summary>
        /// <param name="executionId">Identifier of the execution of the <see cref="IRequest" />.</param>        
        /// <param name="result">The result of the associated <see cref="IQuery{T}" />.</param>
	    public ExecutionSucceededEventArgs(Guid executionId, TResult result)
            : this(executionId, null, result) { }	    
	
        /// <summary>
        /// Initializes a new instance of the <see cref="ExecutionSucceededEventArgs" /> class.
        /// </summary>
        /// <param name="executionId">Identifier of the execution of the <see cref="IRequest" />.</param>
        /// <param name="message">If specified, refers to the message that was sent for the request.</param>
        /// <param name="result">The result of the associated <see cref="IQuery{T}" />.</param>
	    public ExecutionSucceededEventArgs(Guid executionId, object message, TResult result)
            : base(executionId, message)
	    {
	        Result = result;
	    }
    }
}
