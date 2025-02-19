using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LojiteksEntity.Entities
{
    [Table("tblKoli")]
    public class TBL_Koli
    {
        [Column("sKno"), Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)] // No PK
        [Display(Name = "Box Record No")]
        public long KoliID { get; set; }

        [StringLength(50)]
        [Column("baslikNo")]
        [Display(Name = "BaslikID")]
        public long? BaslikID { get; set; }

        [StringLength(50)]
        [Column("koliBarkodu")]
        [Display(Name = "Box Barcode")]
        public string? KoliBarkod { get; set; }

        [Column("adet")]
        [Display(Name = "Piece")]
        public int? Adet { get; set; }

        [Column("koliId")]
        [Display(Name = "Box Id")]
        public long? KoliId { get; set; }

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

        [Column("tarih")]
        [DefaultValue(typeof(DateTime), "getdate()")]
        [Display(Name = "Record Date")]
        public DateTime? KayitTarihi { get; set; }
    }
}
