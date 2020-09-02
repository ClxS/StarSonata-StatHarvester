namespace StarSonata.Api.Messages.Incoming
{
    using System;
    using Objects;

    public class TextMessage : IIncomingMessage
    {
        public const byte MsgError = 1;

        public const byte MsgGlobalLoginMsg = 13;

        public const byte MsgUserChatclientonly = 15;

        public const byte MsgUserGalaxy = 4;

        public const byte MsgUserGlobal = 3;

        public const byte MsgUserGroup = 11;

        public const byte MsgUserHelp = 14;

        public const byte MsgUserModerator = 12;

        public const byte MsgUserTeam = 5;

        public const byte MsgUserTrade = 10;

        public const byte MsgUserUser = 7;

        public TextMessage(ReadOnlySpan<byte> data)
        {
            var offset = 0;
            var type = ByteUtility.GetByte(data, ref offset);
            var message = ByteUtility.GetString(data, ref offset);
            var username = string.Empty;
            var channelId = ByteUtility.GetString(data, ref offset);
            if (offset < data.Length)
            {
                username = ByteUtility.GetString(data, ref offset);
            }

            MessageChannel channel = MessageChannel.Event;
            switch (type)
            {
                case MsgUserTeam:
                    break;
                case MsgUserGalaxy:
                    channel = MessageChannel.Galaxy;
                    break;
                case MsgUserGlobal:
                    channel = MessageChannel.All;
                    break;
                case MsgUserTrade:
                    channel = MessageChannel.Trade;
                    break;
                case MsgUserGroup:
                    channel = MessageChannel.Squad;
                    break;
                case MsgUserModerator:
                    channel = MessageChannel.Moderator;
                    break;
                case MsgUserHelp:
                    channel = MessageChannel.Help;
                    break;
                case MsgUserChatclientonly:
                    channel = MessageChannel.Chat;
                    break;
                case MsgGlobalLoginMsg:
                    channel = MessageChannel.Event;
                    break;
                case MsgError:
                    channel = MessageChannel.Event;
                    break;
                case MsgUserUser:
                    channel = MessageChannel.Private;
                    break;
                default:
                    channel = MessageChannel.Event;
                    break;
            }

            this.Message = new ChatMessage(channel, message, username);
        }

        public ChatMessage Message { get; set; }
    }
}
