using CsGOStateEmitter.Entities;
using CsGOStateEmitter.Helper;
using CsGOStateEmitter.Models;
using Discord;
using Discord.WebSocket;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using PuppeteerSharp;
using RestSharp;

namespace CsGOStateEmitter
{
    public class DiscordEmitter
    {
        private readonly ApplicationContext _context;
        private readonly StateManagement stateManagement;
        public DiscordEmitter(ApplicationContext context, StateManagement stateManagement)
        {
            _context = context;
            this.stateManagement = stateManagement;
        }

        public async Task<(Result, List<PlayerStats>)> GetLastMatchAndPlayerStats()
        {
            if (stateManagement.LastMatch == null)
            {
                stateManagement.LastMatch = await _context.Result.FirstOrDefaultAsync(x => x.EndTime == null);
            }

            if (stateManagement.LastMatch != null)
            {
                var finishedMatch = await _context.Result.FirstOrDefaultAsync(x => x.MatchId == stateManagement.LastMatch.MatchId && x.EndTime != null);
                if (finishedMatch != null)
                {
                    var playerStats = await _context.Stats.Where(x => x.MatchId == stateManagement.LastMatch.MatchId).ToListAsync();
                    return (stateManagement.LastMatch, playerStats);

                    stateManagement.LastMatch = null;
                }
            }

            return (stateManagement.LastMatch, null);
        }

        public async Task<(Result, List<PlayerStats>)> GetLastMatchAndPlayerStats2(int order)
        {

            var lastMatchs = await _context.Result.OrderByDescending(x => x.MatchId).Take(10).ToListAsync();
            stateManagement.LastMatch = lastMatchs.ElementAt(order - 1);

            if (stateManagement.LastMatch != null)
            {
                var finishedMatch = await _context.Result.FirstOrDefaultAsync(x => x.MatchId == stateManagement.LastMatch.MatchId);
                if (finishedMatch != null)
                {
                    var playerStats = await _context.Stats.Where(x => x.MatchId == stateManagement.LastMatch.MatchId).ToListAsync();
                    return (stateManagement.LastMatch, playerStats);

                    stateManagement.LastMatch = null;
                }
            }

            return (stateManagement.LastMatch, null);
        }


        private async Task<(string, string, Result)> BuildHtml()
        {
            var result = await this.GetLastMatchAndPlayerStats();
            if (result.Item2 == null)
            {
                return ("", "", null);
            }
            string table1 = "";
            string table2 = "";
            var team1 = result.Item2.GroupBy(x => x.Team).OrderBy(x => x.Key).FirstOrDefault().OrderByDescending(x => x.Kills).ToList();
            var team2 = result.Item2.GroupBy(x => x.Team).OrderBy(x => x.Key).LastOrDefault().OrderByDescending(x => x.Kills).ToList();
            foreach (var player in team1)
            {
                table1 += $"<tr>";
                table1 += $"<td scope=\"row\">{player.Name}</td>";
                table1 += $"<td>{player.Kills}</td>";
                table1 += $"<td>{player.Assists}</td>";
                table1 += $"<td>{player.Deaths}</td>";
                table1 += $"<td>{player.Kast}</td>";
                table1 += $"</tr>";

            }


            foreach (var player in team2)
            {
                table2 += $"<tr>";

                table2 += $"<td scope=\"row\">{player.Name}</td>";
                table2 += $"<td>{player.Kills}</td>";
                table2 += $"<td>{player.Assists}</td>";
                table2 += $"<td>{player.Deaths}</td>";
                table2 += $"<td>{player.Kast}</td>";
                table2 += $"</tr>";

            }
            return (table1, table2, result.Item1);
        }

        private async Task<(string, string, Result)> BuildHtml2(int order)
        {
            var result = await this.GetLastMatchAndPlayerStats2(order);
            if (result.Item2 == null)
            {
                return ("", "", null);
            }
            string table1 = "";
            string table2 = "";
            var team1 = result.Item2.GroupBy(x => x.Team).OrderBy(x => x.Key).FirstOrDefault().OrderByDescending(x => x.Kills).ToList();
            var team2 = result.Item2.GroupBy(x => x.Team).Count() > 1 ? result.Item2.GroupBy(x => x.Team).OrderBy(x => x.Key).LastOrDefault().OrderByDescending(x => x.Kills).ToList() : new List<PlayerStats>();
            foreach (var player in team1)
            {
                table1 += $"<tr>";
                table1 += $"<td scope=\"row\">{player.Name}</td>";
                table1 += $"<td>{player.Kills}</td>";
                table1 += $"<td>{player.Assists}</td>";
                table1 += $"<td>{player.Deaths}</td>";
                table1 += $"<td>{player.V1 + player.V2 + player.V3 + player.V4 + player.V5}</td>";
                table1 += $"</tr>";

            }

            foreach (var player in team2)
            {
                table2 += $"<tr>";

                table2 += $"<td scope=\"row\">{player.Name}</td>";
                table2 += $"<td>{player.Kills}</td>";
                table2 += $"<td>{player.Assists}</td>";
                table2 += $"<td>{player.Deaths}</td>";
                table2 += $"<td>{player.V1 + player.V2 + player.V3 + player.V4 + player.V5}</td>";
                table2 += $"</tr>";

            }
            return (table1, table2, result.Item1);
        }
        public async Task SendMessage()
        {
            var tableConte = await this.BuildHtml();

            if (string.IsNullOrEmpty(tableConte.Item1))
            {
                return;
            }

            await new BrowserFetcher().DownloadAsync(BrowserFetcher.DefaultChromiumRevision);
            var browser = await Puppeteer.LaunchAsync(new LaunchOptions
            {
                Headless = true
            });

            // Abre uma nova página
            var page = await browser.NewPageAsync();

            // Define o HTML da tabela
            var valuesContent = new ScoreBoardModel()
            {
                MatchId = tableConte.Item3.MatchId,
                ScoreBoard = $"{tableConte.Item3?.Team1Score ?? 0 } x {tableConte.Item3?.Team2Score ?? 0}",
                Map = tableConte.Item3?.MapName,
                Time1 = "Time 1",
                TableRowsTime1 = tableConte.Item1,
                Time2 = "Time 2",
                TableRowsTime2 = tableConte.Item2
            };

            var htmlContent = HtmlMessageHelper.SetValuesContent(valuesContent, "HtmlMessage", "Scoreboard.html");

            // Define o conteúdo da página como o HTML completo
            await page.SetContentAsync(htmlContent);

            // Espera pelo elemento da tabela estilizada estar disponível na página
            await page.WaitForSelectorAsync("table");

            // Extrai o elemento da tabela estilizada
            var elementBody = await page.QuerySelectorAsync("body");
            var elementTable = await page.QuerySelectorAsync(".scoreboard");

            // Obtem a altura e largura da tabela
            var sizeTable = await elementTable.BoundingBoxAsync();
            var sizeBody = await elementBody.BoundingBoxAsync();
            var alturaTabela = (int)Math.Ceiling(sizeBody.Height);
            var larguraTabela = (int)Math.Ceiling(sizeTable.Width);

            // Ajusta o tamanho do viewport da página para o tamanho da tabela
            await page.SetViewportAsync(new ViewPortOptions
            {
                Width = larguraTabela,
                Height = alturaTabela
            });

            // Extrai o screenshot da tabela
            var screenshotTabela = await elementTable.ScreenshotDataAsync(new ScreenshotOptions
            {
                Type = ScreenshotType.Png
            });

            // Envia o screenshot da tabela para o Discord usando o webhook
            var client = new RestClient("https://discord.com/api/webhooks/1086380521619198023/K276RTW_YmYZPwyKwKp5d0vvmL4AODV3Z5A5j3y3c2UI6iA3RqInNI2lRr1rY0O0gd_e");
            var request = new RestRequest("", Method.Post);
            request.AddHeader("Content-Type", "multipart/form-data");
            request.AddParameter("payload_json", "{\"content\":\"Tabela estilizada do exemplo:\"}");
            request.AddFile("file", screenshotTabela, "tabela.png", "image/png");
            var response = await client.ExecuteAsync(request);
            Console.WriteLine(response.Content);

            // Fecha o Chromium Headless
            await browser.CloseAsync();
        }

        public async Task SendImageFilesInBase64(SocketMessage message, List<string> imagesBase64, string messageAditional = null)
        {
            try
            {
                List<FileAttachment> attachments = new List<FileAttachment>();
                for (int i = 0; i < imagesBase64.Count; i++)
                {
                    attachments.Add(new FileAttachment(new MemoryStream(Convert.FromBase64String(imagesBase64[i])), $"screemshot-anticheating-{i}.png"));
                }
                var result = await message.Channel.SendFilesAsync(attachments, messageAditional);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                Console.WriteLine(e.StackTrace);
                Console.WriteLine(e.InnerException);
                throw;
            }
        }

        public async Task SendMessage2(SocketMessage message, int order = 1)
        {
            var tableConte = await this.BuildHtml2(order);
            if (string.IsNullOrEmpty(tableConte.Item1))
            {
                return;
            }
            await new BrowserFetcher().DownloadAsync(BrowserFetcher.DefaultChromiumRevision);
            var browser = await Puppeteer.LaunchAsync(new LaunchOptions
            {
                Headless = true,
                Args = new string[] { "--no-sandbox" }
            });

            // Abre uma nova página
            var page = await browser.NewPageAsync();

            var valuesContent = new ScoreBoardModel()
            {
                ScoreBoard = $"{tableConte.Item3?.Team1Score ?? 0 } x {tableConte.Item3?.Team2Score ?? 0}",
                Map = tableConte.Item3?.MapName,
                Time1 = "Time 1",
                TableRowsTime1 = tableConte.Item1,
                Time2 = "Time 2",
                TableRowsTime2 = tableConte.Item2
            };

            var htmlContent = HtmlMessageHelper.SetValuesContent(valuesContent, "HtmlMessage", "Scoreboard.html");

            // Define o conteúdo da página como o HTML completo
            await page.SetContentAsync(htmlContent);

            // Espera pelo elemento da tabela estilizada estar disponível na página
            await page.WaitForSelectorAsync("table");

            // Extrai o elemento da tabela estilizada
            var elementBody = await page.QuerySelectorAsync("body");
            var elementTable = await page.QuerySelectorAsync(".scoreboard");

            // Obtem a altura e largura da tabela
            var sizeTable = await elementTable.BoundingBoxAsync();
            var sizeBody = await elementBody.BoundingBoxAsync();
            var alturaTabela = (int)Math.Ceiling(sizeTable.Height);
            var larguraTabela = (int)Math.Ceiling(sizeBody.Width);

            // Ajusta o tamanho do viewport da página para o tamanho da tabela
            await page.SetViewportAsync(new ViewPortOptions
            {
                Width = larguraTabela,
                Height = alturaTabela
            });

            // Extrai o screenshot da tabela
            var screenshotTabela = await elementTable.ScreenshotDataAsync(new ScreenshotOptions
            {
                Type = ScreenshotType.Png
            });

            Stream stream = new MemoryStream(screenshotTabela);

            try
            {
                var result = await message.Channel.SendFileAsync(stream, "lastmatch.png");
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                Console.WriteLine(e.StackTrace);
                Console.WriteLine(e.InnerException);
                throw;
            }

            // Envia o screenshot da tabela para o Discord usando o webhook
            // Fecha o Chromium Headless
            await browser.CloseAsync();
        }

        public async Task AssociateUser(SocketMessage message, string steamName)
        {
            var assoctionaExists = await _context.Set<DiscordUser>().FirstOrDefaultAsync(x => x.DiscordId == message.Author.Id.ToString());
            if (assoctionaExists != null)
            {
                await message.Channel.SendMessageAsync($"Usuário já está associado a um player");
                return;
            }

            if (long.TryParse(steamName, out var steamId))
            {
                var playerAlreadyAssociated = await _context.Set<DiscordUser>().FirstOrDefaultAsync(x => x.SteamID == steamId.ToString());
                if (playerAlreadyAssociated != null)
                {
                    await message.Channel.SendMessageAsync($"Você não pode se linkar a esse player porque ele já está com o cú todo arregassado!");
                    return;
                }
                await _context.Set<DiscordUser>().AddAsync(new DiscordUser { DiscordId = message.Author.Id.ToString(), Name = message.Author.Username, SteamID = steamId.ToString() });
                await message.Channel.SendMessageAsync($"Usuário {steamName} associado pelo steamId {steamId} com sucesso seu cara de buceta do caralho");
                await _context.SaveChangesAsync();
                return;
            }
            var user = _context.Set<PlayerStats>().FirstOrDefault(x => x.Name == steamName);
            if (user == null)
            {
                await message.Channel.SendMessageAsync($"Usuário {steamName} não encontrado, jogue ao menos uma partida e insira o nome com que você jogou durante a partida, cú enxuto do caralho!");
                return;
            }

            var playerAlreadyAssociatedByName = await _context.Set<DiscordUser>().FirstOrDefaultAsync(x => x.SteamID == user.SteamId64.ToString());
            if (playerAlreadyAssociatedByName != null)
            {
                await message.Channel.SendMessageAsync($"Você não pode se linkar a esse player porque ele já está com o cú todo arregassado!");
                return;
            }

            await _context.Set<DiscordUser>().AddAsync(new DiscordUser { DiscordId = message.Author.Id.ToString(), Name = message.Author.Username, SteamID = user.SteamId64.ToString() });
            await _context.SaveChangesAsync();
            await message.Channel.SendMessageAsync($"Usuário {message.Author.Username} associado ao player {user.Name} - {user.SteamId64}!");
        }

        public async Task DessociateUser(SocketMessage message)
        {
            var user = _context.Set<DiscordUser>().FirstOrDefault(x => x.DiscordId == message.Author.Id.ToString());
            if (user == null)
            {
                await message.Channel.SendMessageAsync($"Usuário {message.Author.Username} não está associado a nenhum player");
                return;
            }
            _context.Set<DiscordUser>().Remove(user);

            await _context.SaveChangesAsync();
            await message.Channel.SendMessageAsync($"Usuário {message.Author.Username} desassociado com sucesso.");
        }
    }
}
