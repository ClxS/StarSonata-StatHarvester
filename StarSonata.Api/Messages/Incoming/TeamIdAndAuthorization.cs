namespace StarSonata.Api.Messages.Incoming
{
    using System;

    class TeamIdAndAuthorization : IIncomingMessage
    {
        [Flags]
        public enum Result
        {
            Success = 0,
            TeamDoesNotExist = 1,
            UserDoesNotExist = 1 << 1,
            UserNotOnTeam = 1 << 2,
            UserNotRanked = 1 << 3,
            Exception = 1 << 4
        };

        public int RequestId { get; }

        public Result RequestResult { get; }

        public int TeamId { get; }

        public TeamIdAndAuthorization(ReadOnlySpan<byte> data)
        {
            var offset = 0;
            this.RequestId = ByteUtility.GetInt(data, ref offset);
            this.RequestResult = (Result)ByteUtility.GetInt(data, ref offset);
            this.TeamId = ByteUtility.GetInt(data, ref offset);
        }
    }
}
