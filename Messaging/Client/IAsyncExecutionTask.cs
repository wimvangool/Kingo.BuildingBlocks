﻿using System.ComponentModel.Messaging.Server;

namespace System.ComponentModel.Messaging.Client
{
    /// <summary>
    /// Represents a task that encapsulates the asynchronous execution of a request.
    /// </summary>
    public interface IAsyncExecutionTask : IIsBusyIndicator
    {
        /// <summary>
        /// Returns the identifier of this task.
        /// </summary>
        Guid RequestId
        {
            get;
        }

        /// <summary>
        /// Occurs when <see cref="Status" /> changes.
        /// </summary>
        event EventHandler StatusChanged;

        /// <summary>
        /// Returns the status of this task.
        /// </summary>
        AsyncExecutionTaskStatus Status
        {
            get;
        }

        /// <summary>
        /// Occurs when <see cref="Progress" /> changes.
        /// </summary>
        event EventHandler ProgressChanged;

        /// <summary>
        /// Returns the progress has made so far.
        /// </summary>
        Progress Progress
        {
            get;
        }

        /// <summary>
        /// Executes this task.
        /// </summary>
        /// <exception cref="InvalidOperationException">
        /// The task has already been started.
        /// </exception>
        void Execute();

        /// <summary>
        /// Cancels this task. This method does nothing if the task has already ended.
        /// </summary>        
        void Cancel();
    }
}