using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LojiteksEntity.Entities
{
    [Table("tblCihaz")]
    public class TBL_Cihaz
    {
        [Column("ckno"), Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)] // No PK
        [Display(Name = "Device Record No")]
        public long CihazID { get; set; }

        [StringLength(50)]
        [Column("kod")]
        [Display(Name = "Code")]
        public string? Kod { get; set; }

        [StringLength(50)]
        [Column("tanim")]
        [Display(Name = "Description")]
        public string? Tanim { get; set; }

        [StringLength(50)]
        [Column("seriNo ")]
        [Display(Name = "Serial No")]
        public string? SeriNo { get; set; }

        /// <summary>0 sbox</summary>
        [Column("tipi")]
        [Display(Name = "Device Type")]
        public int? CihazTip { get; set; }

        /// <summary>0 lojiteks</summary>
        [Column("uygulama")]
        [DefaultValue(0)]
        [Display(Name = "Aplication Type")]
        public int? UygulamaTipi { get; set; }

        [StringLength(500)]
        [Column("apiUrl")]
        [Display(Name = "Api Url")]
        public string? ApiUrl { get; set; }

        [StringLength(500)]
        [Column("apiKey")]
        [Display(Name = "Api Key")]
        public string? ApiKey { get; set; }

        [Column("firma")]
        [Display(Name = "Company")]
        [ForeignKey("tblFirma")]
        public long? FirmaID { get; set; }
        [Column("firma")]
        [Display(Name = "Company")]
        public TBL_Firma? Firma { get; set; }
    }
}
