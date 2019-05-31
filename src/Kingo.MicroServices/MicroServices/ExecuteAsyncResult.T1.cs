﻿namespace Kingo.MicroServices
{    
    /// <summary>
    /// Represents the result of executing a query by a <see cref="IMicroProcessor" />.
    /// </summary>
    /// <typeparam name="TResponse"></typeparam>
    public sealed class ExecuteAsyncResult<TResponse> : IMicroProcessorOperationResult<TResponse>
    {        
        internal ExecuteAsyncResult(TResponse response)
        {
            Response = response;
        }

        TResponse IMicroProcessorOperationResult<TResponse>.Value =>
            Response;
        
        /// <summary>
        /// The response that was returned by the query.
        /// </summary>
        public TResponse Response
        {
            get;
        }

        /// <inheritdoc />
        public override string ToString() =>
            Response == null ? "null" : Response.ToString();
    }
}
