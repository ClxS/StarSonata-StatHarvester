namespace StarSonata.Api
{
    using System;
    using System.Linq;
    using System.Net;
    using System.Reactive.Linq;
    using System.Runtime.InteropServices;
    using System.Security;
    using System.Threading.Tasks;
    using Communication;
    using Messages;
    using Messages.Incoming;
    using Messages.Outgoing;
    using Objects;
    using Serilog;

    public class StarSonataClient
    {
        private StarSonataMonitor monitor;

        public event EventHandler Running;

        public event EventHandler LoginRequired;

        public IObservable<IIncomingMessage> WhenMessageReceived { get; private set; }

        public bool IsLoggedIn { get; set; }

        public async Task Run(IPAddress serverIp, int port)
        {
            if (this.monitor != null)
            {
                throw new Exception("Initialise has already been called.");
            }

            this.monitor = new StarSonataMonitor(serverIp, port);
            this.monitor.ConnectionEstablished += this.Monitor_ConnectionEstablished;
            this.monitor.ConnectionLost += Monitor_ConnectionLost;

            this.WhenMessageReceived = this.monitor.Messages;
            this.AddDefaultSubscriptions();
            this.OnRunning();

            await this.monitor.RunMonitorAsync().ConfigureAwait(false);
        }

        private void Monitor_ConnectionLost(object sender, EventArgs e)
        {
            this.IsLoggedIn = false;
        }

        public async Task SendMessageAsync(ChatMessage message)
        {
            await this.SendAsync(new TextMessageOut(message)).ConfigureAwait(false);
        }

        public Task SendAsync(IOutgoingMessage message)
        {
            return this.monitor.SendMessageAsync(message);
        }

        protected virtual void OnRunning()
        {
            this.Running?.Invoke(this, EventArgs.Empty);
        }

        protected virtual void OnLoginRequired()
        {
            this.LoginRequired?.Invoke(this, EventArgs.Empty);
        }

        private void AddDefaultSubscriptions()
        {
            Log.Information("Subscribing to Received Observable");

            // Received a ping, send a pong
            this.WhenMessageReceived.Where(msg => msg is Ping).Subscribe(
                async msg =>
                {
                    var ping = (Ping)msg;
                    try
                    {
                        await this.monitor.SendMessageAsync(new Pong(ping.Sec, ping.USec)).ConfigureAwait(false);
                    }
                    catch (Exception ex)
                    {
                        // TODO[CJ] Log
                        Log.Error(ex, "Error sending ping");
                    }
                });

            this.WhenMessageReceived.Where(msg => msg is Disconnect).Subscribe(
                msg => { this.OnLoginRequired(); });

            this.WhenMessageReceived.Where(msg => msg is LoginFail).Subscribe(
                msg => { });

            this.WhenMessageReceived.Where(msg => msg is Hello).Subscribe(
                msg => { this.IsLoggedIn = true; });

            // Login as the first available character
            this.WhenMessageReceived.Where(msg => msg is CharacterList).Subscribe(
                async msg =>
                {
                    Log.Verbose("Received Character List");
                    try
                    {
                        var characterList = (CharacterList)msg;
                        Log.Information("Logging in as " + characterList.Characters.First().Name);
                        await this.monitor.SendMessageAsync(new SelectCharacter(characterList.Characters.First()))
                                  .ConfigureAwait(false);
                    }
                    catch (Exception e)
                    {
                        Log.Error(e, "Failed to login");
                    }
                });

            // Log Text Messages
            this.WhenMessageReceived.OfType<TextMessage>().Subscribe(
                msg =>
                {
                    if (msg.Message.Channel == MessageChannel.Event)
                    {
                        return;
                    }

                    var textMessage = msg;
                    Log.Information("{CHANNEL} {USERNAME} {MESSAGE}", textMessage.Message.Channel.ToString(),
                        textMessage.Message.Username, textMessage.Message.Message);
                });
        }

        private void Monitor_ConnectionEstablished(object sender, EventArgs e)
        {
            this.OnLoginRequired();
        }
    }
}
