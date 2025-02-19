namespace LojiteksApi.Models
{
    public class CihazModel
    {
        public long CihazID { get; set; } = 0;
        public string? Kod { get; set; } = "";
        public string? Tanim { get; set; } = "";
        public string? SeriNo { get; set; } = "";
        public int? CihazTip { get; set; } = 0;
        public int? UygulamaTipi { get; set; } = 0;
        public string? ApiUrl { get; set; } = "";
        public string? ApiKey { get; set; } = "";
        public long? FirmaID { get; set; } = 0;
    }
}
