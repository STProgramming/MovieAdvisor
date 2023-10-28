
using MAModels.EntityFrameworkModels;
using Newtonsoft.Json;
using System.Net.Http.Json;

namespace MAUI.Services.ApiServices
{
    public class MovieTagsApiServices
    {
        private readonly HttpClient _httpClient;

        public MovieTagsApiServices(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<ICollection<MovieTag>> GetMovieTags()
        {
            var response = await _httpClient.GetAsync("MovieTag/GetMovieTags");
            response.EnsureSuccessStatusCode();
            string jsonResponse = await response.Content.ReadAsStringAsync();
            var tags = JsonConvert.DeserializeObject<ICollection<MovieTag>>(jsonResponse); 
            return tags;
        }
    }
}
