using LojiteksEntity.Entities;
using Microsoft.EntityFrameworkCore;

namespace LojiteksDataAccess.DBContext
{
    public class Context : DbContext
    {
        public Context(DbContextOptions<Context> options) : base(options) { }

        public virtual DbSet<TBL_Baslik> TBL_Basliklar { get; set; }
        public virtual DbSet<TBL_Cihaz> TBL_Cihazlar { get; set; }
        public virtual DbSet<TBL_Epc> TBL_Epcler { get; set; }
        public virtual DbSet<TBL_Firma> TBL_Firmalar { get; set; }
        public virtual DbSet<TBL_Koli> TBL_Koliler { get; set; }
        public virtual DbSet<TBL_Kullanici> TBL_Kullanicilar { get; set; }
        public virtual DbSet<TBL_Lisans> TBL_Lisans { get; set; }
        public virtual DbSet<TBL_Musteri> TBL_Musteriler { get; set; }
        public virtual DbSet<TBL_Musteri> TBL_Bildirimler { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<TBL_Kullanici>().Property(e => e.AdSoyad).IsUnicode(false);
            modelBuilder.Entity<TBL_Kullanici>().Property(e => e.KullaniciAdi).IsUnicode(false);
            modelBuilder.Entity<TBL_Kullanici>().Property(e => e.Sifre).IsUnicode(false);

            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<TBL_Firma>().HasData(
            new TBL_Firma { FirmaID = 1, Unvan = "Admin" }
            );

            modelBuilder.Entity<TBL_Kullanici>().HasData(
                new TBL_Kullanici { KullaniciID = 1, FirmaID = 1, KullaniciAdi = "Admin", Sifre = "123456", Yetki = 0, AdSoyad = "Admin" }
            );

            modelBuilder.Entity<TBL_Epc>()
                .HasOne(e => e.Koli)
                .WithMany()
                .HasForeignKey(e => e.KoliId);

            modelBuilder.Entity<TBL_Baslik>(x =>
            {
                x.ToTable("tblBaslik", y =>
                {
                    y.HasTrigger("trg_tblBaslik_Delete");
                    y.HasTrigger("trg_tblBaslik_Insert");
                    y.HasTrigger("trg_tblBaslik_Update");
                });
            });
        }
    }
}
