﻿using System;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Kingo.Messaging
{
    [TestClass]
    public sealed class WriteScenario_Fails_IfAssertEventIndexIsNegative : WriteScenarioTest<object>
    {
        protected override object When() =>
            new object();

        [TestMethod]
        [ExpectedException(typeof(IndexOutOfRangeException))]
        public override async Task ThenAsync()
        {
            try
            {
                await Result.IsEventStreamAsync(0, stream =>
                {
                    AssertEvent<object>(stream, -1, @event => { });
                });
            }
            catch (IndexOutOfRangeException exception)
            {
                Assert.IsTrue(exception.Message.StartsWith("There is no element at index '-1' (Count = 0)."));
                throw;
            }
            finally
            {
                await base.ThenAsync();
            }
        }        
    }
}
