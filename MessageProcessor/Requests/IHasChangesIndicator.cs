﻿using System;
using System.ComponentModel;

namespace YellowFlare.MessageProcessing.Requests
{
    /// <summary>
    /// Represents a component that can indicate whether or not it has any changes.
    /// </summary>
    public interface IHasChangesIndicator : INotifyPropertyChanged
    {
        /// <summary>
        /// Occurs when <see cref="HasChanges" /> changed.
        /// </summary>
        event EventHandler HasChangesChanged;

        /// <summary>
        /// Indicates whether or not the component has any changes.
        /// </summary>
        bool HasChanges
        {
            get;
        }
    }
}
