namespace StarSonata.Api.Communication
{
    using System;
    using Messages;
    using Messages.Incoming;
    using Serilog;

    class PacketFactory
    {
        private const byte ScPing = 1;
        private const byte ScHello = 2;
        private const byte ScTextMessage = 16;
        private const byte ScLoginFail = 58;
        private const byte ScCharacterList = 64;
        private const byte ScDisconnect = 67;
        private const byte ScPaxTeamMessage = 179;
        private const byte SCPaxTeamIdAndAuthorizationResponse = 180;

        public static IIncomingMessage Create(in byte type, ReadOnlySpan<byte> data)
        {
            switch (type)
            {
                case ScHello: return new Hello(data);
                case ScPing: return new Ping(data);
                case ScTextMessage: return new TextMessage(data);
                case ScLoginFail: return new LoginFail(data);
                case ScCharacterList: return new CharacterList(data);
                case ScDisconnect: return new Disconnect(data);
                case ScPaxTeamMessage: return new PaxSyncTeamMessage(data);
                case SCPaxTeamIdAndAuthorizationResponse: return new TeamIdAndAuthorization(data);
                default:
                    Log.Information("Unhandled Packet Type: {Id}", type);
                    return null;
            }
        }
    }
}
