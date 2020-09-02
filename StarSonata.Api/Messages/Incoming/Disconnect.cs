using System;

namespace StarSonata.Api.Messages.Incoming
{
    public class Disconnect : IIncomingMessage
    {
        public Disconnect(ReadOnlySpan<byte> data)
        {
        }
    }
}
