namespace StarSonata.Api.Messages.Incoming
{
    using System;

    public class LoginFail : IIncomingMessage
    {
        public LoginFail(ReadOnlySpan<byte> data)
        {
            var offset = 0;
            this.FailureReason = ByteUtility.GetString(data, ref offset);
        }

        public string FailureReason { get; set; }
    }
}
