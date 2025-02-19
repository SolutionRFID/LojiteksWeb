using Skote.Models;
using System;
using System.Configuration; // web.config'deki ayarlarý okumak için
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Skote.Models; // Models klasöründeki sýnýflar (LoginViewModel, KullaniciModel, ResponseResultMessage)

namespace YourProject.Services
{
    public class AuthService
    {
        private readonly string _baseUrl;

        public AuthService()
        {
            // web.config'deki ApiBaseUrl anahtarýný okuyun
            _baseUrl = ConfigurationManager.AppSettings["ApiBaseUrl"];
        }

        public async Task<ResponseResultMessage<KullaniciModel>> LoginAsync(LoginViewModel loginModel)
        {
            using (var client = new HttpClient())
            {
                // API'nin temel adresini ayarlýyoruz
                client.BaseAddress = new Uri(_baseUrl);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                // "login" endpoint'ine POST isteði gönderiyoruz.
                // Eðer "PostAsJsonAsync" metodu tanýnmýyorsa, NuGet'ten "Microsoft.AspNet.WebApi.Client" paketini eklemeniz gerekebilir.
                HttpResponseMessage response = await client.PostAsJsonAsync("login", loginModel);
                if (response.IsSuccessStatusCode)
                {
                    // .NET Framework'te ReadAsAsync<T> kullanýyoruz.
                    var result = await response.Content.ReadAsAsync<ResponseResultMessage<KullaniciModel>>();
                    return result;
                }
                else
                {
                    return new ResponseResultMessage<KullaniciModel>
                    {
                        isSuccess = false,
                        StatusCode = (int)response.StatusCode,
                        Message = "API ile baðlantý kurulamadý.",
                        Data = null
                    };
                }
            }
        }
    }
}
