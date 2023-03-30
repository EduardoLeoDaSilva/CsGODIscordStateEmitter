using CsGOStateEmitter.Models.SkinkiDriver;
using Newtonsoft.Json;
using System.Text;

namespace CsGOStateEmitter.Services
{
    public class SkinkiDriverService
    {
        const string BASE_URL = "http://mixcsgo.servegame.com:27016";
        //const string BASE_URL = "http://localhost:3000";

        public async Task<List<string>> GetFilesAsync(string path, string? steamId = null)
        {
            HttpResponseMessage message;

            using (var request = new HttpRequestMessage(HttpMethod.Post, $"{BASE_URL}/SkinkiDriver/get-files"))
            using (var _httpClient = new HttpClient())
            {
                request.Content = new StringContent(JsonConvert.SerializeObject(new { Path = path }),
                                    Encoding.UTF8,
                                    "application/json");
                message = await _httpClient.SendAsync(request);

                if (!message.IsSuccessStatusCode)
                {
                    return await Task.FromResult(new List<string>());
                }
            }

            var responseJson = await message.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<List<string>>(responseJson);

            if (result == null)
                return await Task.FromResult(new List<string>());

            return await Task.FromResult(result);
        }
    }
}
