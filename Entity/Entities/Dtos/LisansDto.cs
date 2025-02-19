namespace LojiteksWeb.Entities.Dtos
{
    public class LisansDto
    {
        public long LisansID { get; set; }

        public int LisansTip { get; set; }

        public string? Cihaz { get; set; }

        public DateTime KayitTarih { get; set; }

        public DateTime? BaslangicTarih { get; set; }

        public DateTime? BitisTarih { get; set; }

        public string Firma { get; set; }
    }
}
