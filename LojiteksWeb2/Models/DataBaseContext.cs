using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace LojiteksWeb.Models;

public partial class DataBaseContext : DbContext
{
    public DataBaseContext()
    {
    }

    public DataBaseContext(DbContextOptions<DataBaseContext> options)
        : base(options)
    {
    }

    public virtual DbSet<TblBaslik> TblBaslik { get; set; }

    public virtual DbSet<TblBildirim> TblBildirim { get; set; }

    public virtual DbSet<TblCihaz> TblCihaz { get; set; }

    public virtual DbSet<TblEpc> TblEpc { get; set; }

    public virtual DbSet<TblFirma> TblFirma { get; set; }

    public virtual DbSet<TblKoli> TblKoli { get; set; }

    public virtual DbSet<TblKullanici> TblKullanici { get; set; }

    public virtual DbSet<TblLisan> TblLisan { get; set; }

    public virtual DbSet<TblMusteri> TblMusteri { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=45.147.47.14,1453;Database=device;User Id=SolutionRFIDTest;Password=SolutionRFID2024!;TrustServerCertificate=true");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<TblBaslik>(entity =>
        {
            entity.HasKey(e => e.Bkno).HasName("PK_TblBaslik");

            entity.ToTable("tblBaslik", tb =>
                {
                    tb.HasTrigger("trg_tblBaslik_Delete");
                    tb.HasTrigger("trg_tblBaslik_Insert");
                    tb.HasTrigger("trg_tblBaslik_Update");
                });

            entity.Property(e => e.Bkno).HasColumnName("bkno");
            entity.Property(e => e.Aciklama)
                .HasMaxLength(500)
                .IsUnicode(false)
                .HasColumnName("aciklama");
            entity.Property(e => e.CihazId).HasColumnName("CihazID");
            entity.Property(e => e.Firma).HasColumnName("firma");
            entity.Property(e => e.GonderiAdedi).HasColumnName("gonderiAdedi");
            entity.Property(e => e.GonderimTarihi)
                .HasDefaultValueSql("(getdate())")
                .HasColumnName("gonderimTarihi");
            entity.Property(e => e.KayitTarihi)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("kayitTarihi");
            entity.Property(e => e.Kullanici)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("kullanici");
            entity.Property(e => e.Musteri)
                .HasComment("tblMustrei-mkno ile bağlı")
                .HasColumnName("musteri");
            entity.Property(e => e.Po)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("po");
            entity.Property(e => e.Sevkiyat)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("sevkiyat");
            entity.Property(e => e.Silindi)
                .HasDefaultValue(false)
                .HasColumnName("silindi");
            entity.Property(e => e.Tipi)
                .HasDefaultValue(0)
                .HasComment("0 - sevkiyat")
                .HasColumnName("tipi");
        });

        modelBuilder.Entity<TblBildirim>(entity =>
        {
            entity.HasKey(e => e.Bikno);

            entity.ToTable("tblBildirim");

            entity.Property(e => e.Bikno).HasColumnName("bikno");
            entity.Property(e => e.Bildirim)
                .HasMaxLength(250)
                .IsUnicode(false)
                .HasColumnName("bildirim");
            entity.Property(e => e.Durum)
                .HasDefaultValue((byte)0)
                .HasColumnName("durum");
            entity.Property(e => e.Kullanici)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("kullanici");
            entity.Property(e => e.Tarih)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("tarih");
        });

        modelBuilder.Entity<TblCihaz>(entity =>
        {
            entity.HasKey(e => e.Ckno);

            entity.ToTable("tblCihaz");

            entity.Property(e => e.Ckno).HasColumnName("ckno");
            entity.Property(e => e.ApiKey)
                .HasMaxLength(500)
                .IsUnicode(false)
                .HasColumnName("apiKey");
            entity.Property(e => e.ApiUrl)
                .HasMaxLength(500)
                .IsUnicode(false)
                .HasColumnName("apiUrl");
            entity.Property(e => e.Firma)
                .HasComment("tblfirma-fkno ile bağlı")
                .HasColumnName("firma");
            entity.Property(e => e.Kod)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("kod");
            entity.Property(e => e.SeriNo)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("seriNo");
            entity.Property(e => e.Tanim)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("tanim");
            entity.Property(e => e.Tipi)
                .HasComment("0 sbox")
                .HasColumnName("tipi");
            entity.Property(e => e.Uygulama)
                .HasDefaultValue(0)
                .HasComment("0 lojiteks ")
                .HasColumnName("uygulama");
        });

        modelBuilder.Entity<TblEpc>(entity =>
        {
            entity.HasKey(e => e.EKno);

            entity.ToTable("tblEpc");

            entity.Property(e => e.EKno).HasColumnName("eKno");
            entity.Property(e => e.BaslikNo).HasColumnName("baslikNo");
            entity.Property(e => e.Beden)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("beden");
            entity.Property(e => e.Epc)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Firma).HasColumnName("firma");
            entity.Property(e => e.KayitTarihi)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("kayitTarihi");
            entity.Property(e => e.KoliNo).HasColumnName("koliNo");
            entity.Property(e => e.Silindi)
                .HasDefaultValue(false)
                .HasColumnName("silindi");
            entity.Property(e => e.Upc)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("upc");
        });

        modelBuilder.Entity<TblFirma>(entity =>
        {
            entity.HasKey(e => e.Fkno);

            entity.ToTable("tblFirma");

            entity.Property(e => e.Fkno).HasColumnName("fkno");
            entity.Property(e => e.Unvan)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("unvan");
        });

        modelBuilder.Entity<TblKoli>(entity =>
        {
            entity.HasKey(e => e.SKno);

            entity.ToTable("tblKoli");

            entity.Property(e => e.SKno).HasColumnName("sKno");
            entity.Property(e => e.Adet).HasColumnName("adet");
            entity.Property(e => e.BaslikNo).HasColumnName("baslikNo");
            entity.Property(e => e.Firma).HasColumnName("firma");
            entity.Property(e => e.KoliBarkodu)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("koliBarkodu");
            entity.Property(e => e.KoliId).HasColumnName("koliId");
            entity.Property(e => e.Silindi)
                .HasDefaultValue(false)
                .HasColumnName("silindi");
            entity.Property(e => e.Tarih)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("tarih");
        });

        modelBuilder.Entity<TblKullanici>(entity =>
        {
            entity.HasKey(e => e.KKno);

            entity.ToTable("tblKullanici");

            entity.Property(e => e.KKno).HasColumnName("kKno");
            entity.Property(e => e.AdSoyad)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("adSoyad");
            entity.Property(e => e.Email)
                .HasMaxLength(250)
                .HasColumnName("email");
            entity.Property(e => e.Firma).HasColumnName("firma");
            entity.Property(e => e.KullaniciAdi)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("kullaniciAdi");
            entity.Property(e => e.Sifre)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("sifre");
            entity.Property(e => e.TwoFactorCode).HasMaxLength(6);
            entity.Property(e => e.TwoFactorCodeExpiration).HasColumnType("datetime");
            entity.Property(e => e.Yetki)
                .HasDefaultValue(0)
                .HasColumnName("yetki");
        });

        modelBuilder.Entity<TblLisan>(entity =>
        {
            entity.HasKey(e => e.LKno);

            entity.ToTable("tblLisans");

            entity.Property(e => e.LKno).HasColumnName("lKno");
            entity.Property(e => e.BaslangicTarih)
                .HasColumnType("datetime")
                .HasColumnName("baslangicTarih");
            entity.Property(e => e.BitisTarih)
                .HasColumnType("datetime")
                .HasColumnName("bitisTarih");
            entity.Property(e => e.Cihaz)
                .HasComment("tblCihaz.ckno ile eslestir")
                .HasColumnName("cihaz");
            entity.Property(e => e.Firma)
                .HasComment("tblFirma.fkno ile eslestir")
                .HasColumnName("firma");
            entity.Property(e => e.KayitTarih)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("kayitTarih");
            entity.Property(e => e.LisansTip)
                .HasComment("0: tarih aralikli lisans, 1: suresiz lisans")
                .HasColumnName("lisansTip");
        });

        modelBuilder.Entity<TblMusteri>(entity =>
        {
            entity.HasKey(e => e.Mkno);

            entity.ToTable("tblMusteri");

            entity.Property(e => e.Mkno).HasColumnName("mkno");
            entity.Property(e => e.Firma).HasColumnName("firma");
            entity.Property(e => e.Musteri)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("musteri");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
