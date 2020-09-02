namespace StarSonata.Api.Objects
{
    using System;

    public class ChatMessage
    {
        public ChatMessage()
        {
        }

        public ChatMessage(MessageChannel channel, string message, string username = null)
        {
            if (message.Length > 0 && (message[0] == 65467 || message[0] == '»'))
            {
                this.Message = message.Substring(1);
                this.IsExternalChatMessage = true;
            }
            else
            {
                this.Message = message;
                this.IsExternalChatMessage = false;
            }

            this.Channel = channel;
            this.Username = username;

            if (this.Message.StartsWith("/me ", StringComparison.OrdinalIgnoreCase))
            {
                this.Message = this.Message.Substring(3);
                this.IsMeMessage = true;
            }
        }

        public MessageChannel Channel { get; set; }

        public bool IsExternalChatMessage { get; set; }

        public bool IsMeMessage { get; set; }

        public string Message { get; set; }

        public string Username { get; set; }
    }
}
