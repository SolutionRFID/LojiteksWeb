namespace LojiteksWeb.Entities.Dtos
{
    public class RaporDto
    {
        public long BaslikID { get; set; }
        public DateTime? GonderimTarihi { get; set; }
        public int? GonderiAdedi { get; set; }
        public string? Kullanici { get; set; }
        public string? SevkiyatAd { get; set; }
        public string? PO_Number { get; set; }
        public string? Firma { get; set; }


    }
}
