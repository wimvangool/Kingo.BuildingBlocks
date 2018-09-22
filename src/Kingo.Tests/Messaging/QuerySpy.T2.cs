﻿using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using static Kingo.Threading.AsyncMethod;

namespace Kingo.Messaging
{
    internal sealed class QuerySpy<TMessageIn, TMessageOut> : IQuery<TMessageIn, TMessageOut> where TMessageIn : class
    {
        private readonly List<TMessageIn> _messages;

        public QuerySpy()
        {
            _messages = new List<TMessageIn>();
        } 

        public Task<TMessageOut> ExecuteAsync(TMessageIn message, IMicroProcessorContext context)
        {
            return Run(() =>
            {
                _messages.Add(message);

                return default(TMessageOut);
            });
        }

        public void AssertExecuteCountIs(int count) =>
            Assert.AreEqual(count, _messages.Count);

        public void AssertMessageReceived(int index, TMessageIn message) =>
            Assert.AreSame(message, _messages[index]);
    }
}
