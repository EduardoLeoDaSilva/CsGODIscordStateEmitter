using CsGOStateEmitter.Entities;
using Discord.WebSocket;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace CsGOStateEmitter
{
    public class HostzoneService
    {
        private readonly ApplicationContext _context;
        private readonly StateManagement _stateManagement;

        public HostzoneService(ApplicationContext context, StateManagement stateManagement)
        {
            _context = context;
            _stateManagement = stateManagement;
        }
        public async Task<string> Logar()
        {
            HttpResponseMessage message;

            using (var request = new HttpRequestMessage(HttpMethod.Post, "https://painel.hostzone.com.br/index.php"))
            using (var _httpClient = new HttpClient())
            {
                // request.Headers.Clear();
                var dataContent = new FormUrlEncodedContent(new[]
                {
                    new KeyValuePair<string, string>("ulogin", "eduardoleodasilva@gmail.com"),
                    new KeyValuePair<string, string>("upassword", "vSf2G7xo95"),
                    new KeyValuePair<string, string>("lang", "-"),
                    new KeyValuePair<string, string>("login", "Entrar")
                });

                request.Content = dataContent;
                request.Content.Headers.Remove("Content-type");
                request.Content.Headers.Add("content-type", "application/x-www-form-urlencoded");
                request.Content.Headers.Add("authority", "painel.hostzone.com.br");
                request.Content.Headers.TryAddWithoutValidation("accept-language", "pt-BR,pt;q=0.9,en-US;q=0.8,en;q=0.7,es-ES;q=0.6,es;q=0.5,en-XA;q=0.4");
                request.Content.Headers.TryAddWithoutValidation("cache-control", "max-age=0");
                request.Content.Headers.TryAddWithoutValidation("origin", "https://painel.hostzone.com.br");
                request.Content.Headers.TryAddWithoutValidation("referer", "https://painel.hostzone.com.br/index.php");

                message = await _httpClient.SendAsync(request);

                if (message.IsSuccessStatusCode)
                    Console.WriteLine("Login efetuado com sucesso");

                var tt = message.Headers.FirstOrDefault(x => x.Key == "Set-Cookie");
                return tt.Value.First();
            }
        }

        public async Task KikarJogador(string nome, string cookie)
        {

            HttpResponseMessage message;
            var cookieContainer = new CookieContainer();
            using (var handler = new HttpClientHandler() { CookieContainer = cookieContainer })
            using (var request = new HttpRequestMessage(HttpMethod.Post, "https://painel.hostzone.com.br/home.php?m=gamemanager&p=log&home_id-mod_id-ip-port=7796-4952-177.54.148.27-27294"))
            using (var _httpClient = new HttpClient(handler))
            {
                // request.Headers.Clear();
                var dataContent = new FormUrlEncodedContent(new[]
                {
                    new KeyValuePair<string, string>("command", $"kick {nome}"),
                    new KeyValuePair<string, string>("remote_send_rcon_command", "Enviar+comando"),
                });

                // request.Headers.Clear();
                var dataContent2 = new FormUrlEncodedContent(new[]
                {
                    new KeyValuePair<string, string>("command", $"say Rala pau no cu do caralho {nome}"),
                    new KeyValuePair<string, string>("remote_send_rcon_command", "Enviar+comando"),
                });

                request.Content = dataContent;
                request.Content.Headers.Remove("Content-type");


                var name = cookie.Split("=").First();
                var value = cookie.Split("=")[1].Split(";").First();

                cookieContainer.Add(new Uri("https://painel.hostzone.com.br"), new Cookie(name, value) { Domain = "painel.hostzone.com.br" });


                request.Content.Headers.Add("content-type", "application/x-www-form-urlencoded");
                request.Content.Headers.Add("authority", "painel.hostzone.com.br");
                request.Content.Headers.TryAddWithoutValidation("accept-language", "pt-BR,pt;q=0.9,en-US;q=0.8,en;q=0.7,es-ES;q=0.6,es;q=0.5,en-XA;q=0.4");
                request.Content.Headers.TryAddWithoutValidation("cache-control", "max-age=0");
                request.Content.Headers.TryAddWithoutValidation("origin", "https://painel.hostzone.com.br");
                request.Content.Headers.TryAddWithoutValidation("referer", "https://painel.hostzone.com.br/index.php");
                message = await _httpClient.SendAsync(request);


            }
            if (message.IsSuccessStatusCode)
                Console.WriteLine($"Jogador {nome} kickado");

        }

        public async Task RollbackOrder(SocketMessage socketMessage, int? roundRequested = null)
        {

            var match = _context.Set<Result>().OrderByDescending(x => x.MatchId).FirstOrDefault(x => x.EndTime == null);
            var admins = await _context.Set<AdminBot>().ToListAsync();

            if (match == null) {
                await socketMessage.Channel.SendMessageAsync($"Sem partida ativa para dar o rollback");
                return;
            }
            var round = roundRequested ?? ((match.Team1Score + match.Team2Score));
            var rollbackString = $"get5_backup0_match{match.MatchId}_map0_round{round}.cfg";

            _stateManagement.Rollback = rollbackString;

            await socketMessage.Channel.SendMessageAsync($"Rollback para o round {round} da {match.MapName} solicitado. Aguardando confirmação de um dos admins. {string.Join(",", admins.Select(x => x.Name))}");

        }

        public async Task ClearRollbackRequest(SocketMessage socketMessage)
        {
            _stateManagement.Rollback = "";

            await socketMessage.Channel.SendMessageAsync($"Pedido de rollback deletado");

        }

        public async Task ChangeMap(string cookie, SocketMessage socketMessage, string map)
        {

            var commandOwner = await _context.Set<AdminBot>().FirstOrDefaultAsync(x => x.Id == socketMessage.Author.Id.ToString());

            if (commandOwner == null)
            {
                await socketMessage.Channel.SendMessageAsync("Você não pode solicitar mudança de mapa, porque tu é um poita");
                return;
            }

            await socketMessage.Channel.SendMessageAsync($"Mapa {map} solicitado, fazendo a mudança.");
            var mapTreated = map.Contains("de_") ? map : $"de_{map}";
            HttpResponseMessage message;
            var cookieContainer = new CookieContainer();
            using (var handler = new HttpClientHandler() { CookieContainer = cookieContainer })
            using (var request = new HttpRequestMessage(HttpMethod.Post, "https://painel.hostzone.com.br/home.php?m=gamemanager&p=log&home_id-mod_id-ip-port=7796-4952-177.54.148.27-27294"))
            using (var _httpClient = new HttpClient(handler))
            {
                // request.Headers.Clear();
                var dataContent = new FormUrlEncodedContent(new[]
                {
                    new KeyValuePair<string, string>("command", $"sm_rcon map {mapTreated}"),
                    new KeyValuePair<string, string>("remote_send_rcon_command", "Enviar+comando"),
                });


                request.Content = dataContent;
                request.Content.Headers.Remove("Content-type");


                var name = cookie.Split("=").First();
                var value = cookie.Split("=")[1].Split(";").First();

                cookieContainer.Add(new Uri("https://painel.hostzone.com.br"), new Cookie(name, value) { Domain = "painel.hostzone.com.br" });


                request.Content.Headers.Add("content-type", "application/x-www-form-urlencoded");
                request.Content.Headers.Add("authority", "painel.hostzone.com.br");
                request.Content.Headers.TryAddWithoutValidation("accept-language", "pt-BR,pt;q=0.9,en-US;q=0.8,en;q=0.7,es-ES;q=0.6,es;q=0.5,en-XA;q=0.4");
                request.Content.Headers.TryAddWithoutValidation("cache-control", "max-age=0");
                request.Content.Headers.TryAddWithoutValidation("origin", "https://painel.hostzone.com.br");
                request.Content.Headers.TryAddWithoutValidation("referer", "https://painel.hostzone.com.br/index.php");
                message = await _httpClient.SendAsync(request);

                if (message.IsSuccessStatusCode)
                {
                    await socketMessage.Channel.SendMessageAsync("Mapa alterado com sucesso!");

                }
                else
                {
                    await socketMessage.Channel.SendMessageAsync("Erro ao mudar de mapa");
                }


            }
        }



        public async Task AddPlayerToMatch(string cookie, SocketMessage socketMessage, string auth, string team)
        {


            var commandOwner = await _context.Set<AdminBot>().FirstOrDefaultAsync(x => x.Id == socketMessage.Author.Id.ToString());

            if (commandOwner == null)
            {
                await socketMessage.Channel.SendMessageAsync("Você não pode adicionar um player a partida");
                return;
            }

            await socketMessage.Channel.SendMessageAsync($"Adicionando player ao {team}");
            HttpResponseMessage message;
            var cookieContainer = new CookieContainer();
            using (var handler = new HttpClientHandler() { CookieContainer = cookieContainer })
            using (var request = new HttpRequestMessage(HttpMethod.Post, "https://painel.hostzone.com.br/home.php?m=gamemanager&p=log&home_id-mod_id-ip-port=7796-4952-177.54.148.27-27294"))
            using (var _httpClient = new HttpClient(handler))
            {
                // request.Headers.Clear();
                var dataContent = new FormUrlEncodedContent(new[]
                {
                    new KeyValuePair<string, string>("command", $"get5_addplayer {auth} {team}"),
                    new KeyValuePair<string, string>("remote_send_rcon_command", "Enviar+comando"),
                });


                request.Content = dataContent;
                request.Content.Headers.Remove("Content-type");


                var name = cookie.Split("=").First();
                var value = cookie.Split("=")[1].Split(";").First();

                cookieContainer.Add(new Uri("https://painel.hostzone.com.br"), new Cookie(name, value) { Domain = "painel.hostzone.com.br" });


                request.Content.Headers.Add("content-type", "application/x-www-form-urlencoded");
                request.Content.Headers.Add("authority", "painel.hostzone.com.br");
                request.Content.Headers.TryAddWithoutValidation("accept-language", "pt-BR,pt;q=0.9,en-US;q=0.8,en;q=0.7,es-ES;q=0.6,es;q=0.5,en-XA;q=0.4");
                request.Content.Headers.TryAddWithoutValidation("cache-control", "max-age=0");
                request.Content.Headers.TryAddWithoutValidation("origin", "https://painel.hostzone.com.br");
                request.Content.Headers.TryAddWithoutValidation("referer", "https://painel.hostzone.com.br/index.php");
                message = await _httpClient.SendAsync(request);

                if (message.IsSuccessStatusCode)
                {
                    await socketMessage.Channel.SendMessageAsync("Player adicionado com sucesso!");

                }
                else
                {
                    await socketMessage.Channel.SendMessageAsync("Erro ao adicionar player");
                }


            }
        }

        public async Task ExecuteRollback(string cookie, SocketMessage socketMessage)
        {
            var commandOwner = await _context.Set<AdminBot>().FirstOrDefaultAsync(x => x.Id == socketMessage.Author.Id.ToString());


            if (commandOwner == null)
            {
                await socketMessage.Channel.SendMessageAsync("Você não pode confirmar a solicitação de rollback, porque você não é admin");
                return;
            }


            if (string.IsNullOrWhiteSpace(_stateManagement.Rollback))
            {
                await socketMessage.Channel.SendMessageAsync($"Sem rollback requisitado, solicite primeiro o rollback");
                return;
            }

            await socketMessage.Channel.SendMessageAsync($"Rollback confirmado, solicitação sendo executada.");


            HttpResponseMessage message;
            var cookieContainer = new CookieContainer();
            using (var handler = new HttpClientHandler() { CookieContainer = cookieContainer })
            using (var request = new HttpRequestMessage(HttpMethod.Post, "https://painel.hostzone.com.br/home.php?m=gamemanager&p=log&home_id-mod_id-ip-port=7796-4952-177.54.148.27-27294"))
            using (var _httpClient = new HttpClient(handler))
            {
                // request.Headers.Clear();
                var dataContent = new FormUrlEncodedContent(new[]
                {
                    new KeyValuePair<string, string>("command", $"get5_loadbackup {_stateManagement.Rollback}"),
                    new KeyValuePair<string, string>("remote_send_rcon_command", "Enviar+comando"),
                });


                request.Content = dataContent;
                request.Content.Headers.Remove("Content-type");


                var name = cookie.Split("=").First();
                var value = cookie.Split("=")[1].Split(";").First();

                cookieContainer.Add(new Uri("https://painel.hostzone.com.br"), new Cookie(name, value) { Domain = "painel.hostzone.com.br" });


                request.Content.Headers.Add("content-type", "application/x-www-form-urlencoded");
                request.Content.Headers.Add("authority", "painel.hostzone.com.br");
                request.Content.Headers.TryAddWithoutValidation("accept-language", "pt-BR,pt;q=0.9,en-US;q=0.8,en;q=0.7,es-ES;q=0.6,es;q=0.5,en-XA;q=0.4");
                request.Content.Headers.TryAddWithoutValidation("cache-control", "max-age=0");
                request.Content.Headers.TryAddWithoutValidation("origin", "https://painel.hostzone.com.br");
                request.Content.Headers.TryAddWithoutValidation("referer", "https://painel.hostzone.com.br/index.php");
                message = await _httpClient.SendAsync(request);

                if (message.IsSuccessStatusCode)
                {
                    await socketMessage.Channel.SendMessageAsync("Rollback executado com sucesso!");
                    _stateManagement.Rollback = "";

                }
                else
                {
                    await socketMessage.Channel.SendMessageAsync("Erro ao dar o rollback");
                    _stateManagement.Rollback = "";
                }


            }
        }


        public async Task<string> StatusMatch(string cookie)
        {

            HttpResponseMessage message;
            var cookieContainer = new CookieContainer();
            using (var handler = new HttpClientHandler() { CookieContainer = cookieContainer })
            using (var request = new HttpRequestMessage(HttpMethod.Post, "https://painel.hostzone.com.br/home.php?m=gamemanager&p=log&home_id-mod_id-ip-port=7796-4952-177.54.148.27-27294"))
            using (var _httpClient = new HttpClient(handler))
            {
                // request.Headers.Clear();
                var dataContent = new FormUrlEncodedContent(new[]
                {
                    new KeyValuePair<string, string>("command", $"status"),
                    new KeyValuePair<string, string>("remote_send_rcon_command", "Enviar+comando"),
                });


                request.Content = dataContent;
                request.Content.Headers.Remove("Content-type");


                var name = cookie.Split("=").First();
                var value = cookie.Split("=")[1].Split(";").First();

                cookieContainer.Add(new Uri("https://painel.hostzone.com.br"), new Cookie(name, value) { Domain = "painel.hostzone.com.br" });


                request.Content.Headers.Add("content-type", "application/x-www-form-urlencoded");
                request.Content.Headers.Add("authority", "painel.hostzone.com.br");
                request.Content.Headers.TryAddWithoutValidation("accept-language", "pt-BR,pt;q=0.9,en-US;q=0.8,en;q=0.7,es-ES;q=0.6,es;q=0.5,en-XA;q=0.4");
                request.Content.Headers.TryAddWithoutValidation("cache-control", "max-age=0");
                request.Content.Headers.TryAddWithoutValidation("origin", "https://painel.hostzone.com.br");
                request.Content.Headers.TryAddWithoutValidation("referer", "https://painel.hostzone.com.br/index.php");
                message = await _httpClient.SendAsync(request);

                if (message.IsSuccessStatusCode)
                    return await message.Content.ReadAsStringAsync();

                return "";
            }
        }
    }
}
