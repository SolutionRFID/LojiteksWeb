namespace LojiteksWeb.Entities.Dtos
{
    public class EpcDto
    {
        public long? KoliID { get; set; }
        public string? KoliBarkod { get; set; }
        public string? Epc { get; set; }
        public string? Upc { get; set; }
        public string? Beden { get; set; }
        public DateTime? KayitTarihi { get; set; }
    }
}
