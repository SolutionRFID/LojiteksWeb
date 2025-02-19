namespace LojiteksWeb.Entities.Dtos
{
    public class KoliDto
    {
        public long KoliID { get; set; }
        public string? KoliBarkod { get; set; }
        public int? Adet { get; set; }
        public string Firma { get; set; }
        public DateTime? KayitTarihi { get; set; }
    }
}
