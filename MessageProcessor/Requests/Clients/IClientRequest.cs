﻿using System;

namespace YellowFlare.MessageProcessing.Requests.Clients
{
    /// <summary>
    /// Represents a request made from a client that lies on top of a regular <see cref="IRequest">Request</see>.
    /// </summary>
    public interface IClientRequest : System.Windows.Input.ICommand, IIsBusyIndicator, IIsValidIndicator
    {
        /// <summary>
        /// Indicates that <see cref="IsExecuting" /> has changed.
        /// </summary>
        event EventHandler IsExecutingChanged;

        /// <summary>
        /// Indicates whether or not one or more executions for this <see cref="IRequest" /> are running.
        /// </summary>
	    bool IsExecuting
	    {
	        get;
	    }
    }
}
