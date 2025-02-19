using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace LojiteksEntity.Entities
{
    [Table("tblFirma")]
    public class TBL_Firma
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("fkno"), Key]
        [Display(Name = "Company Record No")]
        public long FirmaID { get; set; }

        [StringLength(50)]
        [Column("unvan")]
        [Display(Name = "Title")]
        public string Unvan { get; set; } = "";
    }
}
