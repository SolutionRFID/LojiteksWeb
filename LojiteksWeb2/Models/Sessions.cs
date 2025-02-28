using System.ComponentModel.DataAnnotations;

namespace LojiteksWeb.Models
{
    public class Sessions
    {
        [Key]
        public long KKno { get; set; }
        public long IsLocked { get; set; } = 0;
        public string? KullaniciAdi { get; set; }
        
        public string? Sifre { get; set; }

        public long? Firma { get; set; }

        public string? AdSoyad { get; set; }

        public int? Yetki { get; set; }

        public string? Email { get; set; }

        public bool EmailConfirmed { get; set; }

        public string? TwoFactorCode { get; set; }

        public DateTime? TwoFactorCodeExpiration { get; set; }

        public short? TwoFactorCounter { get; set; }
    }
}
