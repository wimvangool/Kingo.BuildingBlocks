﻿using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Transactions;
using Kingo.Reflection;

namespace Kingo.MicroServices.Controllers
{
    /// <summary>
    /// Serves as a base-class for all <see cref="IMicroServiceBus" /> implementations. 
    /// </summary>
    public abstract class MicroServiceBus : AsyncDisposable, IMicroServiceBus
    {
        #region [====== SenderHost ======]

        private sealed class SenderHost : MicroServiceBusClientHost
        {
            private readonly MicroServiceBus _microServiceBus;
            private readonly MessageDirection _supportedMessageDirection;

            public SenderHost(MicroServiceBus microServiceBus, MessageDirection supportedMessageDirection)
            {
                _microServiceBus = microServiceBus;
                _supportedMessageDirection = supportedMessageDirection;
            }

            protected override string Name =>
                nameof(_microServiceBus.Sender).ToLowerInvariant();

            protected override MessageDirection SupportedMessageDirection =>
                _supportedMessageDirection;

            protected override Task<MicroServiceBusClient> CreateClientAsync(CancellationToken token) =>
                _microServiceBus.CreateSenderAsync(token);
        }

        #endregion

        #region [====== ReceiverHost ======]

        private sealed class ReceiverHost : MicroServiceBusClientHost
        {
            private readonly MicroServiceBus _microServiceBus;
            private readonly MessageDirection _supportedMessageDirection;

            public ReceiverHost(MicroServiceBus microServiceBus, MessageDirection supportedMessageDirection)
            {
                _microServiceBus = microServiceBus;
                _supportedMessageDirection = supportedMessageDirection;
            }

            protected override string Name =>
                nameof(_microServiceBus.Receiver);

            protected override MessageDirection SupportedMessageDirection =>
                _supportedMessageDirection;

            protected override Task<MicroServiceBusClient> CreateClientAsync(CancellationToken token) =>
                _microServiceBus.CreateReceiverAsync(token);
        }

        #endregion

        private readonly SenderHost _sender;
        private readonly ReceiverHost _receiver;

        /// <summary>
        /// Initializes a new instance of the <see cref="MicroServiceBus" /> class.
        /// </summary>
        internal MicroServiceBus(MessageDirection senderMessageDirection, MessageDirection receiverMessageDirection)
        {
            _sender = new SenderHost(this, senderMessageDirection);
            _receiver = new ReceiverHost(this, receiverMessageDirection);
        }

        /// <inheritdoc />
        public override string ToString() =>
            $"{GetType().FriendlyName()} ({nameof(Sender)} = {_sender}, {nameof(Receiver)} = {_receiver})";

        /// <inheritdoc />
        protected override async ValueTask DisposeAsync(DisposeContext context)
        {
            if (context != DisposeContext.Finalizer)
            {
                await _sender.DisposeAsync();
                await _receiver.DisposeAsync();
            }
            await base.DisposeAsync(context);
        }

        #region [====== Sender ======]

        /// <summary>
        /// Returns the sender of this bus.
        /// </summary>
        protected IMicroServiceBus Sender =>
            _sender;

        /// <summary>
        /// Starts the component of this bus that is responsible for sending messages.
        /// After this method is called, <see cref="SendAsync"/> may be used to send
        /// new messages on this bus.
        /// </summary>
        /// <param name="token">Token that can be used to cancel the operation.</param>
        /// <exception cref="InvalidOperationException">
        /// The message sender has already been started.
        /// </exception>
        public Task StartSendingMessagesAsync(CancellationToken token) =>
            _sender.StartAsync(token);

        /// <summary>
        /// Stops the component of this bus that is responsible for sending messages.
        /// After this method is called, it is no longer possible to use <see cref="SendAsync"/>
        /// to send new messages.
        /// </summary>
        /// <exception cref="InvalidOperationException">
        /// The message sender has already been stopped.
        /// </exception>
        public Task StopSendingMessagesAsync() =>
            _sender.StopAsync();

        /// <inheritdoc />
        public Task SendAsync(IEnumerable<IMessage> messages) =>
            Sender.SendAsync(messages);

        /// <summary>
        /// Creates and returns a <see cref="MicroServiceBusClient"/> that is ready to send messages.
        /// </summary>
        /// <param name="token">Token that can be used to cancel the operation.</param>
        /// <returns>A new <see cref="MicroServiceBusClient"/> that is ready to send messages.</returns>
        protected abstract Task<MicroServiceBusClient> CreateSenderAsync(CancellationToken token);

        #endregion

        #region [====== Receiver ======]

        /// <summary>
        /// Gets the receiver of this bus.
        /// </summary>
        protected IMicroServiceBus Receiver =>
            _receiver;

        /// <summary>
        /// Starts the component of this bus that is responsible for receiving messages.
        /// After this method is called, the bus will start feeding received messages into the
        /// system (e.g. by handing them over to a <see cref="IMicroProcessor"/> or another
        /// <see cref="IMicroServiceBus" />.
        /// </summary>
        /// <param name="token">Token that can be used to cancel the operation.</param>
        /// <exception cref="InvalidOperationException">
        /// The message receiver has already been started.
        /// </exception>
        public Task StartReceivingMessagesAsync(CancellationToken token) =>
            _receiver.StartAsync(token);

        /// <summary>
        /// Stops the component of this bus that is responsible for sending messages.
        /// After this method is called, it is no longer possible to use <see cref="SendAsync"/>
        /// to send new messages.
        /// </summary>
        /// <exception cref="InvalidOperationException">
        /// The message receiver has already been stopped.
        /// </exception>
        public Task StopReceivingMessagesAsync() =>
            _receiver.StopAsync();

        /// <summary>
        /// Creates and returns a <see cref="MicroServiceBusClient"/> that is actively receiving messages.
        /// </summary>
        /// <param name="token">Token that can be used to cancel the operation.</param>
        /// <returns>A new <see cref="MicroServiceBusClient"/> that is actively receiving messages.</returns>
        protected abstract Task<MicroServiceBusClient> CreateReceiverAsync(CancellationToken token);

        #endregion
    }
}
