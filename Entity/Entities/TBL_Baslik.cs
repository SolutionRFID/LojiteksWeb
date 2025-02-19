using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LojiteksEntity.Entities
{
    [Table("tblBaslik")]
    public class TBL_Baslik
    {
        [Column("bkno"), Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)] // No PK
        [Display(Name = "Title Record No")]
        public long BaslikID { get; set; }

        /// <summary>0 - sevkiyat</summary>
        [Column("tipi")]
        [Display(Name = "Type")]
        public int? Tipi { get; set; }

        [StringLength(500)]
        [Column("aciklama")]
        [Display(Name = "Description")]
        public string? Aciklama { get; set; }

        [Column("musteri")]
        [Display(Name = "Customer")]
        [ForeignKey("tblMusteri")]
        public long? MusteriID { get; set; }
        [Column("musteri")]
        [Display(Name = "Customer")]
        public TBL_Musteri? Musteri { get; set; }

        [Column("gonderimTarihi")]
        [Display(Name = "Shipment Date")]
        public DateTime? GonderimTarihi { get; set; }

        [Column("gonderiAdedi")]
        [Display(Name = "Shipment Quantity")]
        public int? GonderiAdedi { get; set; }
        
        [Column("kayitTarihi")]
        [Display(Name = "Record Date")]
        public DateTime? KayitTarihi { get; set; }

        [Column("kullanici")]
        [Display(Name = "User")]
        [StringLength(50)]
        public string? Kullanici { get; set; }

        [Column("silindi")]
        [Display(Name = "Deleted?")]
        public bool? SilindiMi { get; set; }

        [Column("firma")]
        [Display(Name = "Company")]
        [ForeignKey("tblFirma")]
        public long? FirmaID { get; set; }
        [Column("firma")]
        [Display(Name = "Company")]
        public TBL_Firma? Firma { get; set; }

        [StringLength(50)]
        [Column("sevkiyat")]
        [Display(Name = "Shipment Name")]
        public string? SevkiyatAd { get; set; }

        [StringLength(50)]
        [Column("po")]
        [Display(Name = "PO Number")]
        public string? PO_Number { get; set; }

        public long? CihazID { get; set; }
        public TBL_Cihaz? Cihaz { get; set; }

        public List<TBL_Epc> EpcModels { get; set; } = new List<TBL_Epc>();
    }
}
