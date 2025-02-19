using LojiteksEntity.Entities;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace LojiteksApi.Models
{
    public class EpcModel
    {
        public long? BaslikNo { get; set; }
        public long? KoliId { get; set; }
        public string? Epc { get; set; }
        public string? Upc { get; set; }
        public string? Beden { get; set; }
        public DateTime? KayitTarihi { get; set; }
        public bool? SilindiMi { get; set; }
        public long? FirmaID { get; set; }
    }
}
