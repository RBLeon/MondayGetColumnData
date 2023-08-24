using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace MondayGetColumns.Data
{
    public class MondayService
    {
        private readonly IConfiguration _configuration;
        private readonly string _token;

        public MondayService(IConfiguration configuration)
        {
            _configuration = configuration;
            _token = _configuration["MondayToken"];
        }

        public async Task<JsonElement> FetchBoardDataAsync()
        {
            using (HttpClient client = new HttpClient())
            {
                HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, "https://api.monday.com/v2/");
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _token);

                string json = JsonSerializer.Serialize(
                    new
                    {
                        query = "{ boards(ids: YOUR_BOARD_ID) { items { id name column_values { id title value } } } }"
                    }
                );
                request.Content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await client.SendAsync(request);
                var responseText = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<JsonElement>(responseText);
            }
        }

        //public async Task<JsonElement> UpdateBoardDataAsync(string itemId, string columnId, string newValue)
        //{
        //    using (HttpClient client = new HttpClient())
        //    {
        //        HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, "https://api.monday.com/v2/");
        //        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _token);

        //        string json = JsonSerializer.Serialize(
        //            new
        //            {
        //                query = $"mutation {{ change_column_value(board_id: YOUR_BOARD_ID, item_id: \"{itemId}\", column_id: \"{columnId}\", value: \"{newValue}\") {{ id }} }}"
        //            }
        //        );
        //        request.Content = new StringContent(json, Encoding.UTF8, "application/json");

        //        var response = await client.SendAsync(request);
        //        var responseText = await response.Content.ReadAsStringAsync();
        //        return JsonSerializer.Deserialize<JsonElement>(responseText);
        //    }
        //}
    }
}
