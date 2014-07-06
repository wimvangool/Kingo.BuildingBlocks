﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YellowFlare.MessageProcessing.Requests
{
    internal sealed class MessageStateTracker
    {
        private readonly IMessage _message;
        private Guid _executionId;
        private bool _messageHadChanges;

        public MessageStateTracker(IMessage message)
        {
            _message = message;
        }

        public void NotifyExecutionStarted(Guid executionId)
        {
            _executionId = executionId;
            _messageHadChanges = _message.HasChanges;
        }

        public void NotifyExecutionEndedPrematurely(Guid executionId)
        {
            if (_executionId.Equals(executionId))
            {
                _message.HasChanges = _message.HasChanges || _messageHadChanges;
                _executionId = Guid.Empty;
            }
        }
    }
}
