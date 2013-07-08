﻿
namespace YellowFlare.MessageProcessing.SampleHandlers.ForTryRegisterInTests
{
    [InstanceLifetime((InstanceLifetime) 5)]
    internal sealed class MessageHandlerWithInvalidLifetimeAttribute : IMessageHandler<Command>
    {
        public void Handle(Command message) {}            
    }
}