using System.Net.Http;
using System.Threading.Tasks;
using Skote.Models;
using System.Text.Json;

namespace Skote.Services
{
    public class AuthApiService
    {
        private readonly HttpClient _httpClient;

        public AuthApiService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<ResponseResultMessage<KullaniciModel>> LoginAsync(string kullaniciAdi, string sifre)
        {
            // API URL'sini oluştur
            string apiUrl = $"http://45.147.47.14:8071/api/Auth/Login?kullaniciAdi={kullaniciAdi}&sifre={sifre}";

            // API'ye POST isteği at
            var response = await _httpClient.PostAsync(apiUrl, null);
            var responseContent = await response.Content.ReadAsStringAsync();

            // JSON yanıtı Deserialize et
            var result = JsonSerializer.Deserialize<ResponseResultMessage<KullaniciModel>>(responseContent, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            return result;
        }
    }
}
