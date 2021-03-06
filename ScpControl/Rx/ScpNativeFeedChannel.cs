﻿using System;
using System.Linq;
using System.Reactive.Linq;
using System.Threading.Tasks;
using ReactiveSockets;

namespace ScpControl.Rx
{
    public class ScpNativeFeedChannel : IChannel<byte[]>
    {
        private readonly IReactiveSocket _socket;

        /// <summary>
        /// Initializes the channel with the given socket, using 
        /// the given encoding for messages.
        /// </summary>
        public ScpNativeFeedChannel(IReactiveSocket socket)
        {
            this._socket = socket;

            Receiver = from packet in socket.Receiver.Buffer(96)
                       select packet.ToArray();
        }

        public IObservable<byte[]> Receiver { get; private set; }

        public Task SendAsync(byte[] message)
        {
            try
            {
                return _socket.SendAsync(message);
            }
            catch (InvalidOperationException)
            {
                return Task.FromResult<object>(null);
            }
        }
    }
}