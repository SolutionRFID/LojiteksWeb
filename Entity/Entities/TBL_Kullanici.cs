using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LojiteksEntity.Entities
{
    [Table("tblKullanici")]
    public class TBL_Kullanici
    {
        [Column("kKno"), Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)] // No PK
        [Display(Name = "User Record No")]
        public long KullaniciID { get; set; }

        [StringLength(50)]
        [Column("kullaniciAdi")]
        [Display(Name = "Username")]
        public string KullaniciAdi { get; set; }

        [StringLength(50)]
        [Column("sifre")]
        [Display(Name = "Password")]
        public string Sifre { get; set; }

        [Column("firma")]
        [Display(Name = "Company")]
        [ForeignKey("tblFirma")]
        public long FirmaID { get; set; }
        [Column("firma")]
        [Display(Name = "Company")]
        public TBL_Firma? Firma { get; set; }

        [StringLength(50)]
        [Column("adSoyad")]
        [Display(Name = "Full Name")]
        public string AdSoyad { get; set; }

        [Column("yetki")]
        [Display(Name = "Authorization")]
        public int Yetki { get; set; }

        [Display(Name = "Email")]
        public string? Email { get; set; }

        public bool EmailConfirmed { get; set; }
        public string? TwoFactorCode { get; set; }
        public DateTime? TwoFactorCodeExpiration { get; set; }
        public short? TwoFactorCounter { get; set; }
    }
}
