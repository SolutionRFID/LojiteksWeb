using LojiteksWeb.Models;
using System.Threading.Tasks;

namespace LojiteksWeb.Services
{
    public interface IAuthApiService
    {
        Task<ApiResponse<KullaniciModel>> LoginAsync(string kullaniciAdi, string sifre);
    }
}
