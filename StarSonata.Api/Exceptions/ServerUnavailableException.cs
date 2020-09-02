namespace StarSonata.Api.Exceptions
{
    using System;

    public class ServerUnavailableException : Exception
    {
        public ServerUnavailableException()
        {
        }

        public ServerUnavailableException(string message) : base(message)
        {
        }

        public ServerUnavailableException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
