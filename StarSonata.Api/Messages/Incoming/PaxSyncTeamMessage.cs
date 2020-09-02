namespace StarSonata.Api.Messages.Incoming
{
    using System;

    public class PaxSyncTeamMessage : TextMessage
    {
        public PaxSyncTeamMessage(ReadOnlySpan<byte> data)
            : base(data)
        {
            var offset = 0;
            this.TeamId = ByteUtility.GetInt(data.Slice(data.Length - sizeof(int)), ref offset);
        }

        public int TeamId { get; set; }
    }
}
