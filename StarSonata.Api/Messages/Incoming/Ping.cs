namespace StarSonata.Api.Messages.Incoming
{
    using System;
    using System.Runtime.InteropServices;

    public class Ping : IIncomingMessage
    {
        public int Sec;

        public int USec;

        public Ping(ReadOnlySpan<byte> data)
        {
            this.Sec = MemoryMarshal.Read<int>(data);
            this.USec = data[4];
        }
    }
}
