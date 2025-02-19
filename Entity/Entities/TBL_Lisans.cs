using LojiteksEntity.DataAnnotaionAttribute;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LojiteksEntity.Entities
{
    [Table("tblLisans")]
    public class TBL_Lisans
    {
        [Column("lKno"), Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)] // No PK
        [Display(Name = "Licance Record No")]
        public long LisansID { get; set; }

        [Column("lisansTip")]
        [Display(Name = "Licance Type")]
        public int LisansTip { get; set; }

        [Column("cihaz")]
        [Display(Name = "Device")]
        [ForeignKey("tblCihaz")]
        public long CihazID { get; set; }
        [Column("cihaz")]
        [Display(Name = "Cihaz")]
        public TBL_Cihaz? Cihaz { get; set; }

        [Column("kayitTarih")]
        [Display(Name = "Record Date")]
        public DateTime KayitTarih { get; set; }

        [Column("baslangicTarih")]
        [Display(Name = "Start Date")]
        public DateTime? BaslangicTarih { get; set; }

        [Column("bitisTarih")]
        [Display(Name = "Finish Date")]
        [DateGreaterThan("BaslangicTarih")]
        public DateTime? BitisTarih { get; set; }

        [Column("firma")]
        [Display(Name = "Company")]
        [ForeignKey("tblFirma")]
        public long? FirmaID { get; set; }
        [Column("firma")]
        [Display(Name = "Company")]
        public TBL_Firma? Firma { get; set; }
    }
}
