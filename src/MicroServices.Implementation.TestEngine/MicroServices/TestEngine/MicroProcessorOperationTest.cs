﻿using System.Threading.Tasks;

namespace Kingo.MicroServices.TestEngine
{
    /// <summary>
    /// Serves as a base-class for all tests.
    /// </summary>    
    public abstract class MicroProcessorOperationTest : IMicroProcessorOperationTest
    {
        Task IMicroProcessorOperationTest.GivenAsync(IMicroProcessorOperationRunner runner, MicroProcessorOperationTestContext context) =>
            GivenAsync(runner, context);

        /// <summary>
        /// Prepares this test for execution.
        /// </summary>
        /// <param name="runner">
        /// Processor that can be used to run existing tests or handle messages as a means to setup a desired program state.
        /// </param>
        /// <param name="context">The context in which the test is running.</param>        
        protected virtual Task GivenAsync(IMicroProcessorOperationRunner runner, MicroProcessorOperationTestContext context) =>
            Task.CompletedTask;
    }
}
