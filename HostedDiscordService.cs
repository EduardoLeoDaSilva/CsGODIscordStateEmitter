using CsGOStateEmitter.Entities;
using Discord;
using Discord.WebSocket;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace CsGOStateEmitter
{
    public class HostedDiscordService : IHostedService
    {
        private DiscordSocketClient _client;
        private readonly IServiceProvider serviceProvider;
        public HostedDiscordService(IServiceProvider serviceProvider)
        {
            this.serviceProvider = serviceProvider;
        }
        public async Task StartAsync(CancellationToken cancellationToken)
        {
            _client = new DiscordSocketClient(new DiscordSocketConfig
            {
                GatewayIntents = GatewayIntents.AllUnprivileged | GatewayIntents.MessageContent,

            });

            _client.Log += Log;

            string token = "MTA4Njk4MTY4NzQyNDA1NzQwNQ.GAaZXH.i8xxfFL5u03CGOhfPA74gIVUxqIkszwY2txY_c";
            await _client.LoginAsync(TokenType.Bot, token);
            await _client.StartAsync();


            _client.MessageReceived += MessageReceived;

            await Task.Delay(-1);
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            await _client.StopAsync();
        }

        private Task Log(LogMessage msg)
        {
            Console.WriteLine(msg.Message);
            return Task.CompletedTask;
        }

        private async Task MessageReceived(SocketMessage message)
        {
            // Verifica se a mensagem é de um usuário e não é um comando do bot
            if (message.Author.IsBot || message.Content.StartsWith("!")) return;

            try
            {
                await ReadAndExecuteCommand(message.Content, message);

            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                Console.WriteLine(e.StackTrace);
                Console.WriteLine(e.InnerException);
                throw;
            }

            // Responde à mensagem do usuário
        }


        private async Task ReadAndExecuteCommand(string commandMessage, SocketMessage message)
        {
            using (var scope = serviceProvider.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<ApplicationContext>();
                var discordEmitter = scope.ServiceProvider.GetRequiredService<DiscordEmitter>();
                var content = commandMessage.Split(' ');
                var command = commandMessage.Split(' ')[0];
                switch (command)
                {
                    case "stats":

                        long.TryParse(content[1], out var steamId);
                        var result = await context.Set<PlayerStats>().Where(x => x.Name == content[1] || x.SteamId64 == steamId).GroupBy(x => x.SteamId64).ToListAsync();
                        if (result.Any())
                        {
                            var embed = new EmbedBuilder();
                            embed.WithTitle($"Player: {result.First().FirstOrDefault().Name}");
                            embed.Fields.Add(new EmbedFieldBuilder
                            {
                                IsInline = true,
                                Name = "Kills",
                                Value = result.First().Sum(x => x.Kills)
                            });
                            embed.Fields.Add(new EmbedFieldBuilder
                            {
                                IsInline = true,
                                Name = "Deaths",
                                Value = result.First().Sum(x => x.Deaths)
                            });
                            embed.Fields.Add(new EmbedFieldBuilder
                            {
                                IsInline = true,
                                Name = "Clutchs",
                                Value = result.First().Sum(x => x.V1) + result.First().Sum(x => x.V2) + result.First().Sum(x => x.V3) + result.First().Sum(x => x.V4) + result.First().Sum(x => x.V5)
                            });
                            embed.Fields.Add(new EmbedFieldBuilder
                            {
                                IsInline = true,
                                Name = "Assists",
                                Value = result.First().Sum(x => x.Assists)
                            });

                            await message.Channel.SendMessageAsync("", embed: embed.Build());
                        }
                        else
                        {
                            await message.Channel.SendMessageAsync("Nenhum player encontrado com esse nome/steamID");

                        }
                        break;
                    case "last_match":
                        await discordEmitter.SendMessage2(message);
                        break;
                    case "rank_clutchs":
                        break;
                    case "rank_kills":
                        break;
                    case "rank_first_kills":
                        break;
                    default:
                        break;
                }
            }
        }

    }
}
