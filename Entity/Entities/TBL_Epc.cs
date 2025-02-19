using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;

namespace LojiteksEntity.Entities
{
    [Table("tblEpc")]
    public class TBL_Epc
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("eKno"), Key]
        [Display(Name = "EPC Record No")]
        public long? EpcID { get; set; }

        [Column("baslikNo")]
        [Display(Name = "Title No")]
        public long? BaslikID { get; set; }
        public TBL_Baslik? Baslik { get; set; }

        [Column("koliNo")]
        [Display(Name = "Box No")]
        public long? KoliId { get; set; }
        [Display(Name = "Box")]
        public TBL_Koli? Koli { get; set; }

        [StringLength(50)]
        [Column("Epc")]
        public string? Epc { get; set; }

        [StringLength(50)]
        [Column("upc")]
        public string? Upc { get; set; }

        [StringLength(50)]
        [Column("beden")]
        [Display(Name = "Size")]
        public string? Beden { get; set; }

        [Column("kayitTarihi")]
        [DefaultValue(typeof(DateTime), "getdate()")]
        [Display(Name = "Record Date")]
        public DateTime? KayitTarihi { get; set; }

        [Column("silindi")]
        [DefaultValue(false)]
        [Display(Name = "Deleted?")]
        public bool? SilindiMi { get; set; }

        [Column("firma")]
        [Display(Name = "Company")]
        [ForeignKey("Firma")]
        public long? FirmaID { get; set; }
        [Column("firma")]
        [Display(Name = "Company")]
        public TBL_Firma? Firma { get; set; }
    }
}
