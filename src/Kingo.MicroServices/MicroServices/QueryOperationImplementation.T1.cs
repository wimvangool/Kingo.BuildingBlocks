﻿using System;
using System.Threading;
using System.Threading.Tasks;

namespace Kingo.MicroServices
{
    internal sealed class QueryOperationImplementation<TResponse> : QueryOperation<TResponse>
    {
        #region [====== MethodOperation ======]

        private sealed class MethodOperation : ExecuteAsyncMethodOperation<TResponse>
        {
            private readonly QueryOperationImplementation<TResponse> _operation;
            private readonly ExecuteAsyncMethod<TResponse> _method;
            private readonly QueryOperationContext _context;

            public MethodOperation(QueryOperationImplementation<TResponse> operation, QueryOperationContext context)
            {
                _operation = operation;
                _method = new ExecuteAsyncMethod<TResponse>(operation._query);
                _context = context.PushOperation(this);
            }

            public override IMessage Message =>
                _operation.Message;

            public override CancellationToken Token =>
                _operation.Token;

            public override MicroProcessorOperationType Type =>
                _operation.Type;

            public override MicroProcessorOperationKinds Kind =>
                _operation.Kind;

            public override ExecuteAsyncMethod Method =>
                _method;

            public override QueryOperationContext Context =>
                _context;

            public override async Task<QueryOperationResult<TResponse>> ExecuteAsync() =>
                new QueryOperationResult<TResponse>(await _method.ExecuteAsync(_context));
        }

        #endregion

        private readonly IQuery<TResponse> _query;

        public QueryOperationImplementation(MicroProcessor processor, IQuery<TResponse> query, CancellationToken? token) :
            base(processor, token)
        {
            _query = query ?? throw new ArgumentNullException(nameof(query));
        }

        public override IMessage Message =>
            null;

        protected override ExecuteAsyncMethodOperation<TResponse> CreateMethodOperation(QueryOperationContext context) =>
            new MethodOperation(this, context);
    }
}