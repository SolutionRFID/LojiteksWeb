using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LojiteksEntity.Entities
{
    [Table("tblBildirim")]
    public class TBL_Bildirim
    {
        [Column("bikno"), Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)] // No PK
        public long BildirimID { get; set; }

        [Column("kullanici")]
        public string? Kullanici { get; set; }

        [Column("bildirim")]
        public string? Bildirim { get; set; }

        [Column("tarih")]
        public DateTime? Tarih { get; set; }

        [Column("durum")]
        public byte? Durum { get; set; }
    }
}
