using CsGOStateEmitter.Entities;
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

        public async Task<(Result, List<PlayerStats>)> GetLastMatchAndPlayerStats2()
        {
            if (stateManagement.LastMatch == null)
            {
                stateManagement.LastMatch = await _context.Result.OrderBy(x => x.MatchId).LastOrDefaultAsync();
            }

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
                table1 += $"<td>{player.Name}</td>";
                table1 += $"<td>{player.Kills}</td>";
                table1 += $"<td>{player.Assists}</td>";
                table1 += $"<td>{player.Deaths}</td>";
                table1 += $"<td>{player.Kast}</td>";
                table1 += $"</tr>";

            }


            foreach (var player in team2)
            {
                table2 += $"<tr>";

                table2 += $"<td>{player.Name}</td>";
                table2 += $"<td>{player.Kills}</td>";
                table2 += $"<td>{player.Assists}</td>";
                table2 += $"<td>{player.Deaths}</td>";
                table2 += $"<td>{player.Kast}</td>";
                table2 += $"</tr>";

            }
            return (table1, table2, result.Item1);
        }

        private async Task<(string, string, Result)> BuildHtml2()
        {
            var result = await this.GetLastMatchAndPlayerStats2();
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
                table1 += $"<td>{player.Name}</td>";
                table1 += $"<td>{player.Kills}</td>";
                table1 += $"<td>{player.Assists}</td>";
                table1 += $"<td>{player.Deaths}</td>";
                table1 += $"<td>{player.Kast}</td>";
                table1 += $"</tr>";

            }


            foreach (var player in team2)
            {
                table2 += $"<tr>";

                table2 += $"<td>{player.Name}</td>";
                table2 += $"<td>{player.Kills}</td>";
                table2 += $"<td>{player.Assists}</td>";
                table2 += $"<td>{player.Deaths}</td>";
                table2 += $"<td>{player.Kast}</td>";
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

            var html = $"<div class=\"wrapper\"><label class=\"score score2\">{tableConte.Item3?.Team1Score ?? 0 } X {tableConte.Item3?.Team2Score ?? 0}</label>";
            html += "<div class=\"flex\">";

            html += "<div class=\"flex2\">" +
            "<table>" +
            "<tr>" +
            "<th>Player</th>" +
            "<th>Kilss</th>" +
            "<th>Assists</th>" +
            "<th>Deaths</th>" +
            "<th>Clutchs</th>" +
            "</tr>" +
tableConte.Item1 +
            "</table>" +
        "</div>";

            html += "<div class=\"flex2\">" +
"<table>" +
"<tr>" +
            "<th>Player</th>" +
            "<th>Kilss</th>" +
            "<th>Assists</th>" +
            "<th>Deaths</th>" +
            "<th>Clutchs</th>" +
"</tr>" +
tableConte.Item2+
"</table>" +
"</div>";
            html += "</div></div>";


            // Define o CSS para estilizar a tabela
            var css = @"body {
			background-color: #F9F9F9;
			color: #444444;
			font-family: Arial, sans-serif;
			display: flex;
    flex-direction: column;
    justify-content: center;
    align-items: center;
    width: fit-content;
		}

       .flex {
           display: flex;
           flex-direction: row;
           column-gap: 20px;
       }
       
       .flex2 {
           display: flex;
           flex-direction: column;
       }
       
       .score2{
           text-align: center;
       }
       
		table {
			border-collapse: separate;
			border-spacing: 0;
			display: inline-block;
			vertical-align: top;
			box-shadow: 0px 0px 10px rgba(0, 0, 0, 0.1);
			border-radius: 5px;
			overflow: hidden;
		}

		th {
			background-color: #444444;
			color: #FFFFFF;
			font-weight: bold;
			padding: 10px;
			text-align: center;
		}

		td {
			padding: 10px;
			text-align: center;
			border-bottom: 1px solid #E0E0E0;
		}

		.score {
			font-size: 30px;
			font-weight: bold;
			margin-bottom: 20px;
			text-align: center;
		}

		.team {
			font-size: 20px;
			font-weight: bold;
			text-align: left;
		}

		.team-name {
			font-size: 16px;
			font-weight: normal;
			text-align: left;
			color: #888888;
			margin-top: 5px;
		}

		.team-logo {
			display: inline-block;
			vertical-align: middle;
			margin-right: 10px;
			border-radius: 50%;
			overflow: hidden;
			height: 30px;
			width: 30px;
		}

        .wrapper{
            text-align: center;
        }   

		.team-logo img {
			max-width: 100%;
			height: auto;
		}";
            // Define o HTML completo, incluindo o CSS
            var htmlCompleto = $"<html><head><style>{css}</style></head><body>{html}</body></html>";

            // Define o conteúdo da página como o HTML completo
            await page.SetContentAsync(htmlCompleto);

            // Espera pelo elemento da tabela estilizada estar disponível na página
            await page.WaitForSelectorAsync("table");

            // Extrai o elemento da tabela estilizada
            var elementBody = await page.QuerySelectorAsync("body");
            var elementTable = await page.QuerySelectorAsync(".wrapper");

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

        public async Task SendMessage2(SocketMessage message)
        {
            var tableConte = await this.BuildHtml2();
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

            // Define o HTML da tabela

            var html = $"<div class=\"wrapper\"><label class=\"score score2\">{tableConte.Item3?.Team1Score ?? 0 } X {tableConte.Item3?.Team2Score ?? 0}</label>";
            html += "<div class=\"flex\">";

            html += "<div class=\"flex2\">" +
            "<table>" +
            "<tr>" +
            "<th>Player</th>" +
            "<th>Kilss</th>" +
            "<th>Assists</th>" +
            "<th>Deaths</th>" +
            "<th>Clutchs</th>" +
            "</tr>" +
tableConte.Item1 +
            "</table>" +
        "</div>";

            html += "<div class=\"flex2\">" +
"<table>" +
"<tr>" +
            "<th>Player</th>" +
            "<th>Kilss</th>" +
            "<th>Assists</th>" +
            "<th>Deaths</th>" +
            "<th>Clutchs</th>" +
"</tr>" +
tableConte.Item2 +
"</table>" +
"</div>";
            html += "</div></div>";


            // Define o CSS para estilizar a tabela
            var css = @"body {
			background-color: #F9F9F9;
			color: #444444;
			font-family: Arial, sans-serif;
			display: flex;
    flex-direction: column;
    justify-content: center;
    align-items: center;
    width: fit-content;
		}

       .flex {
           display: flex;
           flex-direction: row;
           column-gap: 20px;
       }
       
       .flex2 {
           display: flex;
           flex-direction: column;
       }
       
       .score2{
           text-align: center;
       }
       
		table {
			border-collapse: separate;
			border-spacing: 0;
			display: inline-block;
			vertical-align: top;
			box-shadow: 0px 0px 10px rgba(0, 0, 0, 0.1);
			border-radius: 5px;
			overflow: hidden;
		}

		th {
			background-color: #444444;
			color: #FFFFFF;
			font-weight: bold;
			padding: 10px;
			text-align: center;
		}

		td {
			padding: 10px;
			text-align: center;
			border-bottom: 1px solid #E0E0E0;
		}

		.score {
			font-size: 30px;
			font-weight: bold;
			margin-bottom: 20px;
			text-align: center;
		}

		.team {
			font-size: 20px;
			font-weight: bold;
			text-align: left;
		}

		.team-name {
			font-size: 16px;
			font-weight: normal;
			text-align: left;
			color: #888888;
			margin-top: 5px;
		}

		.team-logo {
			display: inline-block;
			vertical-align: middle;
			margin-right: 10px;
			border-radius: 50%;
			overflow: hidden;
			height: 30px;
			width: 30px;
		}

        .wrapper{
            text-align: center;
        }   

		.team-logo img {
			max-width: 100%;
			height: auto;
		}";
            // Define o HTML completo, incluindo o CSS
            var htmlCompleto = $"<html><head><style>{css}</style></head><body>{html}</body></html>";

            // Define o conteúdo da página como o HTML completo
            await page.SetContentAsync(htmlCompleto);

            // Espera pelo elemento da tabela estilizada estar disponível na página
            await page.WaitForSelectorAsync("table");

            // Extrai o elemento da tabela estilizada
            var elementBody = await page.QuerySelectorAsync("body");
            var elementTable = await page.QuerySelectorAsync(".wrapper");

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
    }
}
