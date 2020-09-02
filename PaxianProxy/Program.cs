namespace StatHarvester
{
    using System;
    using System.Diagnostics;
    using System.IO;
    using System.Linq;
    using System.Net;
    using System.Reactive.Linq;
    using System.Runtime.InteropServices;
    using System.Security;
    using System.Threading;
    using System.Threading.Channels;
    using System.Threading.Tasks;
    using Autofac;
    using AutofacSerilogIntegration;
    using DAL.Repositories;
    using Extensions.Autofac;
    using Serilog;
    using StarSonata.Api;
    using StarSonata.Api.Messages.Incoming;
    using StarSonata.Api.Messages.Outgoing;
    using StarSonata.Api.Objects;
    using Utility;
    using Utility.SerilogEnrichers;

    internal class Program
    {
        private static IContainer BuildServiceContainer()
        {
            var builder = new ContainerBuilder();
            builder.AddStarSonata();
            builder.RegisterLogger();

            return builder.Build();
        }

        private static SecureString GetTemporaryPassword()
        {
            var pw = new SecureString();
            pw.AppendChar('Y');
            pw.AppendChar('o');
            pw.AppendChar('u');
            pw.AppendChar('r');
            pw.AppendChar('P');
            pw.AppendChar('W');
            pw.AppendChar('H');
            pw.AppendChar('e');
            pw.AppendChar('r');
            pw.AppendChar('e');

            return pw;
        }

        private static async Task Main()
        {
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Verbose()
                .Enrich.FromLogContext()
                .Enrich.With<CallerEnricher>()
                .WriteTo.File(
                    "log.txt",
                    rollingInterval: RollingInterval.Day,
                    outputTemplate:
                    "[{Timestamp:HH:mm:ss} {Level:u3}]{GuildEnrichmentExt}{TeamEnrichment}{UserEnrichment} {Message} ({Caller}:{Line}){NewLine}{Exception}")
                .WriteTo.Console(
                    outputTemplate:
                    "[{Timestamp:HH:mm:ss} {Level:u3}]{GuildEnrichment}{TeamEnrichment}{UserEnrichment} {Message} ({Caller}:{Line}){NewLine}{Exception}")
                .CreateLogger();

            var container = BuildServiceContainer();
            var ssClient = container.Resolve<StarSonataClient>();
            ssClient.LoginRequired += TryToLogin;
            ssClient.Running += SsClient_Running;

            await RunStarSonataClientAsync(ssClient);
        }

        private static void SsClient_Running(object sender, EventArgs e)
        {
            var ssClient = (StarSonataClient) sender;
            var tradeMessages = Channel.CreateUnbounded<string>();
            ssClient.WhenMessageReceived.OfType<TextMessage>().Subscribe(
                msg =>
                {
                    if (msg.Message.Channel != MessageChannel.Event ||
                        !msg.Message.Message.Contains("Average selling price (to shop)"))
                    {
                        return;
                    }

                    Debug.WriteLine($"\n\n!!!\n{msg.Message.Message}\n!!!");
                    tradeMessages.Writer.WriteAsync(msg.Message.Message);
                });

            _ = ProcessMessages(tradeMessages.Reader);
            _ = CheckItems(ssClient);
        }

        private static Task CheckItems(StarSonataClient ssClient)
        {
            return Task.Run(async () =>
            {
                var lines = File.ReadAllLines("items.txt").OrderBy(a => Guid.NewGuid()).ToArray();
                while (true)
                {
                    foreach (var item in lines)
                    {
                        while (!ssClient.IsLoggedIn)
                        {
                            Thread.Yield();
                        }

                        try
                        {
                            var itemName = item.Split(',').First();
                            await ssClient.SendMessageAsync(new ChatMessage(MessageChannel.Event, $"/mc {itemName}"));
                            await Task.Delay(TimeSpan.FromSeconds(5));
                        }
                        catch (Exception e)
                        {
                            Log.Error(e, "Error sending MC message");
                        }
                    }
                }
            });
        }

        private static async Task ProcessMessages(ChannelReader<string> tradeMessagesReader)
        {
            try
            {
                await foreach (var message in tradeMessagesReader.ReadAllAsync())
                {
                    var item = ParseHelper.ParseItem(message);
                    if (item == null)
                    {
                        continue;
                    }

                    await ItemRepository.AddOrUpdateItemAsync(item);
                }
            }
            catch (Exception e)
            {
                Log.Error(e, "Error in entry loop");
            }
        }

        private static async Task RunStarSonataClientAsync(StarSonataClient client)
        {
            var hostAddresses = await Dns.GetHostAddressesAsync("liberty.starsonata.com");
            await client.Run(hostAddresses[0], 3030);
        }

        private static async void TryToLogin(object sender, EventArgs e)
        {
            await Task.Delay(TimeSpan.FromSeconds(5));

            Log.Information("Sending Login Request (User: {User})", "Pax");
            await ((StarSonataClient) sender).SendAsync(new ChatClientLogin(new User
            {
                Username = "Pax",
                Password = GetPassword(GetTemporaryPassword())
            }));
        }

        private static string GetPassword(SecureString value)
        {
            var valuePtr = IntPtr.Zero;
            try
            {
                valuePtr = Marshal.SecureStringToGlobalAllocUnicode(value);
                return Marshal.PtrToStringUni(valuePtr);
            }
            finally
            {
                Marshal.ZeroFreeGlobalAllocUnicode(valuePtr);
            }
        }
    }
}