namespace LojiteksApi.Models
{
    public class KullaniciModel
    {
        public long KullaniciID { get; set; }
        public string KullaniciAdi { get; set; } = "";
        public string Sifre { get; set; } = "";
        public long FirmaID { get; set; }
        public string AdSoyad { get; set; } = "";
        public int Yetki { get; set; }
    }
}
