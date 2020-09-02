namespace StarSonata.Api.Communication
{
    using System;
    using System.Net;
    using System.Net.Sockets;
    using System.Reactive.Disposables;
    using System.Reactive.Linq;
    using System.Reactive.Subjects;
    using System.Threading;
    using System.Threading.Channels;
    using System.Threading.Tasks;
    using Exceptions;
    using Extensions;
    using Messages;
    using Serilog;

    internal class StarSonataMonitor : IDisposable
    {
        private readonly ReaderWriterLockSlim locker;
        private readonly int port;
        private readonly IPAddress serverAddress;
        private readonly Subject<IIncomingMessage> messageSubject;

        private StarSonataConnection activeConnection;

        public StarSonataMonitor(IPAddress serverAddress, int port)
        {
            this.serverAddress = serverAddress;
            this.port = port;
            this.locker = new ReaderWriterLockSlim();
            this.messageSubject = new Subject<IIncomingMessage>();
        }

        public event EventHandler ConnectionEstablished;

        public event EventHandler ConnectionLost;

        public IObservable<IIncomingMessage> Messages => this.messageSubject.AsObservable();

        public void Dispose()
        {
            this.locker?.Dispose();
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "CA1031:Do not catch general exception types",
            Justification = "<Pending>")]
        public async Task RunMonitorAsync()
        {
            while (true)
            {
                try
                {
                    Log.Information("Trying to connect to {Server}:{Port}", this.serverAddress, this.port);
                    using var client = new TcpClient();
                    await client.ConnectAsync(this.serverAddress, this.port).ConfigureAwait(false);

                    Log.Information("Connected to {Server}:{Port}", this.serverAddress, this.port);
                    using (this.locker.WriteLock())
                    {
                        this.activeConnection = new StarSonataConnection(client.Client);
                    }

                    {
                        using var cancel = new CancellationDisposable();
                        var pumpTask = Task.Run(async () =>
                        {
                            await foreach (var message in this.activeConnection.Messages.ReadAllAsync(cancel.Token))
                            {
                                Log.Verbose("Received Message: {Message}", message);
                                this.messageSubject.OnNext(message);
                            }
                        });

                        this.OnConnectionEstablished();
                        await this.activeConnection.ProcessAsync().ConfigureAwait(false);
                        await pumpTask.ConfigureAwait(false);
                    }

                    this.OnConnectionLost();

                    using (this.locker.WriteLock())
                    {
                        this.activeConnection = null;
                    }

                    await Task.Delay(TimeSpan.FromSeconds(30)).ConfigureAwait(false);
                }
                catch(Exception e)
                {
                    Log.Error(e, "Exception on connection {Server}:{Port}", this.serverAddress, this.port);
                    await Task.Delay(TimeSpan.FromSeconds(30)).ConfigureAwait(false);
                }
            }
        }

        public async Task SendMessageAsync(IOutgoingMessage message)
        {
            Log.Verbose("Sending Message {Message}", message);
            try
            {
                using (this.locker.ReadLock())
                {
                    if (this.activeConnection == null)
                    {
                        throw new ServerUnavailableException();
                    }

                    await this.activeConnection.SendMessageAsync(message).ConfigureAwait(false);
                }
            }
            catch (Exception e)
            {
                Log.Error(e, "Failed to send message");
                if (e is ServerUnavailableException)
                {
                    throw;
                }

                throw new ServerUnavailableException();
            }
        }

        protected virtual void OnConnectionEstablished()
        {
            this.ConnectionEstablished?.Invoke(this, EventArgs.Empty);
        }

        protected virtual void OnConnectionLost()
        {
            this.ConnectionLost?.Invoke(this, EventArgs.Empty);
        }
    }
}
