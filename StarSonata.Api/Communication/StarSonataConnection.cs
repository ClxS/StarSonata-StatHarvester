namespace StarSonata.Api.Communication
{
    using System;
    using System.IO.Pipelines;
    using System.Net.Sockets;
    using System.Runtime.InteropServices;
    using System.Threading.Channels;
    using System.Threading.Tasks;
    using Messages;

    internal class StarSonataConnection
    {
        private readonly Channel<IIncomingMessage> messages;
        private readonly Socket socket;

        public StarSonataConnection(Socket socket)
        {
            this.socket = socket;
            this.messages = Channel.CreateUnbounded<IIncomingMessage>();
        }

        public ChannelReader<IIncomingMessage> Messages => this.messages.Reader;

        public Task ProcessAsync()
        {
            var pipe = new Pipe(PipeOptions.Default);
            var socketTask = this.ConsumePacketsAsync(pipe.Writer);
            var packageTask = PackagePacketsAsync(pipe.Reader, this.messages.Writer);
            return Task.WhenAll(socketTask, packageTask);
        }

        public async Task SendMessageAsync(IOutgoingMessage message)
        {
            Memory<byte> outgoing = message.GetOutData();
            await this.socket.SendAsync(outgoing, SocketFlags.None);
        }

        private static async Task PackagePacketsAsync(PipeReader pipe, ChannelWriter<IIncomingMessage> channel)
        {
            while (true)
            {
                var result = await pipe.ReadAsync();
                if (result.IsCanceled || result.IsCompleted)
                {
                    break;
                }

                var type = MemoryMarshal.Read<byte>(result.Buffer.FirstSpan);
                var packet = PacketFactory.Create(type, result.Buffer.FirstSpan.Slice(1));
                if (packet != null)
                {
                    await channel.WriteAsync(packet);
                }

                pipe.AdvanceTo(result.Buffer.GetPosition(result.Buffer.Length));
            }

            channel.Complete();
        }

        private async Task<bool> ReadAsync(Memory<byte> buffer, int expectedSize)
        {
            var bytesRead = 0;
            while (bytesRead < expectedSize)
            {
                var readTask = this.socket.ReceiveAsync(
                    buffer.Slice(bytesRead, expectedSize - bytesRead),
                    SocketFlags.None);
                var taskResult = await Task.WhenAny(readTask.AsTask()).ConfigureAwait(false);
                if (taskResult.Result == 0)
                {
                    return false;
                }

                bytesRead += taskResult.Result;
            }

            return true;
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "CA1031:Do not catch general exception types",
            Justification = "We handle it gracefully elsewhere>")]
        private async Task ConsumePacketsAsync(PipeWriter pipe)
        {
            Memory<byte> sizeMemory = new byte[sizeof(short)];
            while (true)
            {
                try
                {
                    if (!await this.ReadAsync(sizeMemory, sizeof(short)).ConfigureAwait(false))
                    {
                        break;
                    }

                    var expectedSize = MemoryMarshal.Read<short>(sizeMemory.Span) + 1;
                    if (!await this.ReadAsync(pipe.GetMemory(expectedSize), expectedSize).ConfigureAwait(false))
                    {
                        break;
                    }

                    pipe.Advance(expectedSize);
                }
                catch
                {
                    break;
                }

                await pipe.FlushAsync();
            }

            pipe.Complete();
        }
    }
}
