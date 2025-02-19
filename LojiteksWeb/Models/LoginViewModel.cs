using System.ComponentModel.DataAnnotations;

namespace LojiteksWeb.Models
{
    public class LoginViewModel
    {
        [Display(Name = "Username")]
        [StringLength(50)]
        public string KullaniciAdi { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Sifre { get; set; }
    }
}
