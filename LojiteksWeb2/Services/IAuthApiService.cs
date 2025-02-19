using Skote.Models;
using System.Threading.Tasks;

namespace LojiteksWeb2.Services
{
    public interface IAuthApiService
    {
        Task<ApiResponse<KullaniciModel>> LoginAsync(string kullaniciAdi, string sifre);
    }
}
