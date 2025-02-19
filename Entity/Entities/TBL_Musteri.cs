using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LojiteksEntity.Entities
{
    [Table("tblMusteri")]
    public class TBL_Musteri
    {
        [Column("mkno"), Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)] // No PK
        [Display(Name = "Customer Record No")]
        public long MusteriID { get; set; }

        [StringLength(50)]
        [Column("musteri")]
        [Display(Name = "Customer Name")]
        public string MusteriAd { get; set; }

        [Column("firma")]
        [Display(Name = "Company")]
        [ForeignKey("tblFirma")]
        public long FirmaID { get; set; }
        [Column("firma")]
        [Display(Name = "Company")]
        public TBL_Firma? Firma { get; set; }
    }
}
