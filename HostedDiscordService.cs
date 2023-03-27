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

            string token = "MTA4Njk4MTY4NzQyNDA1NzQwNQ.GocTM8.cW0CPzBAkroyojJj3ZtsY2zEKQ2-98rxB-0l-A";
            await _client.LoginAsync(TokenType.Bot, token);
            await _client.StartAsync();


            _client.MessageReceived += MessageReceived;
            _client.Disconnected += OnDisconnected;

            await Task.Delay(-1);
        }

        private async Task OnDisconnected(Exception arg)
        {
            string token = "MTA4Njk4MTY4NzQyNDA1NzQwNQ.GocTM8.cW0CPzBAkroyojJj3ZtsY2zEKQ2-98rxB-0l-A";
            await _client.LoginAsync(TokenType.Bot, token);
            await this._client.StartAsync();
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
                var hostzoneService = scope.ServiceProvider.GetRequiredService<HostzoneService>();
                var content = commandMessage.Split(' ');
                var command = commandMessage.Split(' ')[0];
                List<IGrouping<long, PlayerStats>> result = null;

                switch (command)
                {
                    case "help" :
                        var embedHelper = new EmbedBuilder();
                        embedHelper.WithTitle($"Todos os comandos possíveis:");
                        embedHelper.Fields.Add(new EmbedFieldBuilder
                        {
                            IsInline = false,
                            Name = "stats",
                            Value = "Obtém um status geral de kills, Deaths, clutchs, Assists FirstKills CT, FirstKills TR."
                        });
                        embedHelper.Fields.Add(new EmbedFieldBuilder
                        {
                            IsInline = false,
                            Name = "link_user NOMEUSUARIOSTEAM",
                            Value = "Você pode associar o seu usuário steam para executar o comando só como 'stats'"
                        });
                        embedHelper.Fields.Add(new EmbedFieldBuilder
                        {
                            IsInline = false,
                            Name = "unlink_user NOMEUSUARIOSTEAM",
                            Value = "Você pode desassociar o seu usuário steam"
                        });
                        embedHelper.Fields.Add(new EmbedFieldBuilder
                        {
                            IsInline = false,
                            Name = "last_match",
                            Value = "Obter o resultado final da última partida. Pode ser especificado a ordem exemplo : 'last_match 1 ou last_match 2'"
                        });
                        embedHelper.Fields.Add(new EmbedFieldBuilder
                        {
                            IsInline = false,
                            Name = "rank_clutchs",
                            Value = "Exibe um resultado geral de todos os clutchs."
                        });
                        embedHelper.Fields.Add(new EmbedFieldBuilder
                        {
                            IsInline = false,
                            Name = "rank_kills",
                            Value = "Exibe um resultado geral de todas as kills por jogadores"
                        });
                        embedHelper.Fields.Add(new EmbedFieldBuilder
                        {
                            IsInline = false,
                            Name = "rank_first_kills_ct",
                            Value = "Exibe o resultado geral de todas as primeiras kills dos jogares no modo CT"
                        });
                        embedHelper.Fields.Add(new EmbedFieldBuilder
                        {
                            IsInline = false,
                            Name = "rank_first_kills_tr",
                            Value = "Exibe o resultado geral de todas as primeiras kills dos jogares no modo TR"
                        });
                        embedHelper.Fields.Add(new EmbedFieldBuilder
                        {
                            IsInline = false,
                            Name = "rollback",
                            Value = "Solicita rollback para o round atual, é possível especificar o round também através do comando: rollback 14"
                        });
                        embedHelper.Fields.Add(new EmbedFieldBuilder
                        {
                            IsInline = false,
                            Name = "execute_rollback",
                            Value = "Executa o rollback solicitado, somente admins tem permissão para executar este comando"
                        });
                        embedHelper.Fields.Add(new EmbedFieldBuilder
                        {
                            IsInline = false,
                            Name = "clear_rollback",
                            Value = "Remove a ultima solicitação de rollback"
                        });
                        embedHelper.Fields.Add(new EmbedFieldBuilder
                        {
                            IsInline = false,
                            Name = "changemap",
                            Value = "Ex: 'changemap mirage'. Muda o mapa da partida, somente admin tem permissão para executa este comando"
                        });
                        embedHelper.Fields.Add(new EmbedFieldBuilder
                        {
                            IsInline = false,
                            Name = "add_player_to_match {steamID} {team}",
                            Value = "Adiciona jogador a partida caso algum jogado caia no meio da partida. Ex: add_player_to_match 5135139358135 team1"
                        });
                        await message.Channel.SendMessageAsync("", embed: embedHelper.Build());
                        break;

                    case "stats":

                        if (!(content.Length > 1))
                        {
                            var discordUser = await context.Set<DiscordUser>().FirstOrDefaultAsync(x => x.DiscordId == message.Author.Id.ToString());
                            if (discordUser == null)
                            {
                                await message.Channel.SendMessageAsync($"Usuário {message.Author.Username} não está associado a nenhum player, otarião");
                                return;
                            }
                            result = await context.Set<PlayerStats>().Where(x => x.SteamId64 == long.Parse(discordUser.SteamID)).GroupBy(x => x.SteamId64).ToListAsync();

                        }
                        else
                        {
                            long.TryParse(content[1], out var steamId);
                            if(steamId == 0)
                            {
                                var playerByName = await context.Set<PlayerStats>().FirstOrDefaultAsync(x => x.Name == content[1]);
                                if(playerByName != null)
                                {
                                    result = await context.Set<PlayerStats>().Where(x =>x.SteamId64 == playerByName.SteamId64).GroupBy(x => x.SteamId64).ToListAsync();
                                }
                                else
                                {
                                    await message.Channel.SendMessageAsync($"Usuário {content[1]} não encontrado!");
                                    return;
                                }
                            }
                            else
                            {
                                result = await context.Set<PlayerStats>().Where(x => x.Name == content[1] || x.SteamId64 == steamId).GroupBy(x => x.SteamId64).ToListAsync();
                            }
                        }
             
                        if (result.Any())
                        {
                            var embed = new EmbedBuilder();
                            embed.WithTitle($"Total geral player: {result.First().OrderByDescending(x => x.MatchId).FirstOrDefault().Name}");
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
                            embed.Fields.Add(new EmbedFieldBuilder
                            {
                                IsInline = true,
                                Name = "FirstKills CT",
                                Value = result.First().Sum(x => x.FirstkillCt)
                            });
                            embed.Fields.Add(new EmbedFieldBuilder
                            {
                                IsInline = true,
                                Name = "FirstKills TR",
                                Value = result.First().Sum(x => x.FirstDeathT)
                            });

                            await message.Channel.SendMessageAsync("", embed: embed.Build());
                        }
                        else
                        {
                            await message.Channel.SendMessageAsync("Nenhum player encontrado com esse nome/steamID, você deve jogar ao menos uma partida para ter stats, desgraça do caralho");

                        }
                        break;
                    case "last_match":
                        if ((content.Length > 1))
                        {
                            if(int.TryParse(content[1], out var  order))
                            {
                                await discordEmitter.SendMessage2(message, order);
                                return;

                            }
                            await message.Channel.SendMessageAsync("Ratomanocu");
                            return;
                        }
                            await discordEmitter.SendMessage2(message);
                        break;
                    case "rank_clutchs":
                        var rankClutchs = await context.Set<PlayerStats>().OrderByDescending(x => x.MatchId).GroupBy(x => x.SteamId64)
                            .Select(x => new KeyValuePair<string, long>(x.OrderByDescending(x => x.MatchId).First().Name, x.Sum(c => c.V1) + x.Sum(c => c.V2) + x.Sum(c => c.V3) + x.Sum(c => c.V4) + x.Sum(c => c.V5))).ToListAsync();
                            var embedClutchs = new EmbedBuilder();
                        embedClutchs.WithTitle($"Rank geral de clutchs:");
                        var auxClutch = 1;

                        foreach (var player in rankClutchs.OrderByDescending(x => x.Value))
                        {
                            embedClutchs.Fields.Add(new EmbedFieldBuilder
                            {
                                IsInline = false,
                                Name = $"#{auxClutch} - {player.Key}",
                                Value = $"Clutchs: {player.Value}"
                            });
                            auxClutch++;
                        }
                        await message.Channel.SendMessageAsync("", embed: embedClutchs.Build());

                        break;
                    case "rank_kills":
                        var players = await context.Set<PlayerStats>().GroupBy(x => x.SteamId64)
                            .Select(x => new KeyValuePair<string, long>(x.OrderByDescending(x => x.MatchId).First().Name, x.Sum(x => x.Kills))).ToListAsync();
                        var embedKills = new EmbedBuilder();
                        embedKills.WithTitle($"Rank geral de Kills:");

                        var aux = 1;
                        foreach (var player in players.OrderByDescending(x => x.Value))
                        {
                            embedKills.Fields.Add(new EmbedFieldBuilder
                            {
                                IsInline = false,
                                Value = $"Kills: {player.Value}",
                                Name = $"#{aux} - {player.Key}",
                                
                            });
                            aux++;
                        }
                        await message.Channel.SendMessageAsync("", embed: embedKills.Build());

                        break;
                    case "rank_first_kills_ct":
                        var firstKillsCT = await context.Set<PlayerStats>().OrderByDescending(x => x.MatchId).GroupBy(x => x.SteamId64)
                            .Select(x => new KeyValuePair<string, long>(x.OrderByDescending(x => x.MatchId).First().Name, x.Sum(x => x.FirstkillCt))).ToListAsync();
                        var embedfirstKillsCT = new EmbedBuilder();
                        embedfirstKillsCT.WithTitle($"Rank geral de First-Kills CT:");
                        var auxftkCT = 1;

                        foreach (var player in firstKillsCT.OrderByDescending(x => x.Value))
                        {
                            embedfirstKillsCT.Fields.Add(new EmbedFieldBuilder
                            {
                                IsInline = false,
                                Name = $"#{auxftkCT} - {player.Key}",
                                Value = $"FirtKills CT: {player.Value}"
                            });
                            auxftkCT++;
                        }
                        await message.Channel.SendMessageAsync("", embed: embedfirstKillsCT.Build());

                        break;
                    case "rank_first_kills_tr":
                        var firstKillsTR = await context.Set<PlayerStats>().OrderByDescending(x => x.MatchId).GroupBy(x => x.SteamId64)
                            .Select(x => new KeyValuePair<string, long>(x.OrderByDescending(x => x.MatchId).First().Name, x.Sum(x => x.FirstkillT))).ToListAsync();
                        var embedfirstKillsTR = new EmbedBuilder();
                        embedfirstKillsTR.WithTitle($"Rank geral de First-Kills TR:");
                        var auxftk = 1;

                        foreach (var player in firstKillsTR.OrderByDescending(x => x.Value))
                        {
                            embedfirstKillsTR.Fields.Add(new EmbedFieldBuilder
                            {
                                IsInline = false,
                                Name = $"#{auxftk} - {player.Key}",
                                Value = $"FirtKills TR: {player.Value}"
                            });
                            auxftk++;
                        }
                        await message.Channel.SendMessageAsync("", embed: embedfirstKillsTR.Build());

                        break;
                    case "link_user":
                        var steamName = commandMessage.Replace("link_user", "");
                        await discordEmitter.AssociateUser(message, steamName);
                        break;
                    case "rollback":
                        if(content.Length > 1)
                        {
                            var round = content[1];
                            await hostzoneService.RollbackOrder(message, int.Parse(round));
                        }
                        else
                        {
                            await hostzoneService.RollbackOrder(message);
                        }

                        break;
                    case "execute_rollback":
                        var cookie = await hostzoneService.Logar();
                        await hostzoneService.ExecuteRollback(cookie, message);
                        break;
                    case "clear_rollback":
                        await hostzoneService.ClearRollbackRequest(message);
                        break;
                    case "changemap":
                        if (content.Length > 1)
                        {
                            var cookieMap = await hostzoneService.Logar();
                            var map = content[1];
                            await hostzoneService.ChangeMap(cookieMap, message, map);
                        }
                        else
                        {
                            await message.Channel.SendMessageAsync("Nenhum mapa informado, informe um porra!");
                        }

                        break;
                    case "add_player_to_match":
                        if (content.Length > 2)
                        {
                            var cookieMap = await hostzoneService.Logar();
                            var steamId = content[1];
                            var team = content[2];
                            await hostzoneService.AddPlayerToMatch(cookieMap, message, steamId, team);
                        }
                        else
                        {
                            await message.Channel.SendMessageAsync("Informe o steamId e o time que o player irá entrar");
                        }
                        break;
                    case "add_admin":
                        if (content.Length > 1)
                        {
                            var discordId = content[1].Replace("@", "").Replace("<", "").Replace(">", "");
                            if(long.TryParse(discordId, out var discordIdLong))
                            {
                                var commandOwner = context.Set<AdminBot>().FirstOrDefault(x => x.Id == message.Author.Id.ToString());
                                if(commandOwner == null)
                                {
                                    await message.Channel.SendMessageAsync("Você não têm permissão para adicionar admins");
                                    return;
                                }

                                var alreadyExists = context.Set<AdminBot>().FirstOrDefault(x => x.Id == discordIdLong.ToString());
                                if(alreadyExists != null)
                                {
                                    await message.Channel.SendMessageAsync("Admin já existe em nossa base.");
                                    return;
                                }

                                await context.Set<AdminBot>().AddAsync(new AdminBot { Id = discordIdLong.ToString(), Name = message.CleanContent.Split(" ")[1].Replace("@", "").Split("#")[0]});
                                await context.SaveChangesAsync();
                                await message.Channel.SendMessageAsync("Admin adicionado com sucesso");
                            }
                            else
                            {
                                await message.Channel.SendMessageAsync("Usuário inválido");
                            }
                        }
                        else
                        {
                            await message.Channel.SendMessageAsync("Informe o admin com @NomeDiscord Ex: add_admin @Poita");
                        }
                        break;
                    case "unlink_user":
                        await discordEmitter.DessociateUser(message);
                        break;
                    default:
                        break;
                }
            }
        }

    }
}
