namespace LojiteksApi.Models
{
    public class BaslikDetailModel
    {
        public long BaslikID { get; set; } = 0;
        public int? Tipi { get; set; } = 0;
        public string? Aciklama { get; set; } = "";
        public long? MusteriID { get; set; } = 0;
        public DateTime? GonderimTarihi { get; set; } = DateTime.MinValue;
        public int? GonderiAdedi { get; set; } = 0;
        //public DateTime? KayitTarihi { get; set; }
        public string? Kullanici { get; set; } = "";
        public bool? SilindiMi { get; set; } = false;
        public long? FirmaID { get; set; } = 0;
        public string? SevkiyatAd { get; set; } = "";
        public string? PO_Number { get; set; } = "";
        public long? CihazID { get; set; } = 0;
        public List<EpcModel> EpcModels { get; set; } = new List<EpcModel>();
    }
}
