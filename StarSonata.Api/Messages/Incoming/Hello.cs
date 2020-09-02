namespace StarSonata.Api.Messages.Incoming
{
    using System;

    public class Hello : IIncomingMessage
    {
        public Hello(ReadOnlySpan<byte> _)
        {
        }
    }
}