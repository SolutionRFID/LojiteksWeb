using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LojiteksWeb.Migrations
{
    /// <inheritdoc />
    public partial class UpdatePrimaryKey : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "tblBaslik",
                columns: table => new
                {
                    bkno = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    tipi = table.Column<int>(type: "int", nullable: true, defaultValue: 0, comment: "0 - sevkiyat"),
                    aciklama = table.Column<string>(type: "varchar(500)", unicode: false, maxLength: 500, nullable: true),
                    musteri = table.Column<long>(type: "bigint", nullable: true, comment: "tblMustrei-mkno ile bağlı"),
                    gonderimTarihi = table.Column<DateOnly>(type: "date", nullable: true, defaultValueSql: "(getdate())"),
                    gonderiAdedi = table.Column<int>(type: "int", nullable: true),
                    kayitTarihi = table.Column<DateTime>(type: "datetime", nullable: true, defaultValueSql: "(getdate())"),
                    kullanici = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true),
                    silindi = table.Column<bool>(type: "bit", nullable: true, defaultValue: false),
                    firma = table.Column<long>(type: "bigint", nullable: true),
                    sevkiyat = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true),
                    po = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true),
                    CihazID = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                });

            migrationBuilder.CreateTable(
                name: "tblBildirim",
                columns: table => new
                {
                    bikno = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    kullanici = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true),
                    bildirim = table.Column<string>(type: "varchar(250)", unicode: false, maxLength: 250, nullable: true),
                    tarih = table.Column<DateTime>(type: "datetime", nullable: true, defaultValueSql: "(getdate())"),
                    durum = table.Column<byte>(type: "tinyint", nullable: true, defaultValue: (byte)0)
                },
                constraints: table =>
                {
                });

            migrationBuilder.CreateTable(
                name: "tblCihaz",
                columns: table => new
                {
                    ckno = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    kod = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true),
                    tanim = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true),
                    seriNo = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true),
                    tipi = table.Column<int>(type: "int", nullable: true, comment: "0 sbox"),
                    firma = table.Column<long>(type: "bigint", nullable: true, comment: "tblfirma-fkno ile bağlı"),
                    uygulama = table.Column<int>(type: "int", nullable: true, defaultValue: 0, comment: "0 lojiteks "),
                    apiUrl = table.Column<string>(type: "varchar(500)", unicode: false, maxLength: 500, nullable: true),
                    apiKey = table.Column<string>(type: "varchar(500)", unicode: false, maxLength: 500, nullable: true)
                },
                constraints: table =>
                {
                });

            migrationBuilder.CreateTable(
                name: "tblEpc",
                columns: table => new
                {
                    eKno = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    baslikNo = table.Column<long>(type: "bigint", nullable: true),
                    koliNo = table.Column<long>(type: "bigint", nullable: true),
                    Epc = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true),
                    upc = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true),
                    beden = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true),
                    kayitTarihi = table.Column<DateTime>(type: "datetime", nullable: true, defaultValueSql: "(getdate())"),
                    silindi = table.Column<bool>(type: "bit", nullable: true, defaultValue: false),
                    firma = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                });

            migrationBuilder.CreateTable(
                name: "tblFirma",
                columns: table => new
                {
                    fkno = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    unvan = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                });

            migrationBuilder.CreateTable(
                name: "tblKoli",
                columns: table => new
                {
                    sKno = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    baslikNo = table.Column<long>(type: "bigint", nullable: true),
                    koliBarkodu = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true),
                    adet = table.Column<int>(type: "int", nullable: true),
                    koliId = table.Column<long>(type: "bigint", nullable: true),
                    silindi = table.Column<bool>(type: "bit", nullable: true, defaultValue: false),
                    firma = table.Column<long>(type: "bigint", nullable: true),
                    tarih = table.Column<DateTime>(type: "datetime", nullable: true, defaultValueSql: "(getdate())")
                },
                constraints: table =>
                {
                });

            migrationBuilder.CreateTable(
                name: "tblKullanici",
                columns: table => new
                {
                    kKno = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    kullaniciAdi = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true),
                    sifre = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true),
                    firma = table.Column<long>(type: "bigint", nullable: true),
                    adSoyad = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true),
                    yetki = table.Column<int>(type: "int", nullable: true, defaultValue: 0),
                    email = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    EmailConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    TwoFactorCode = table.Column<string>(type: "nvarchar(6)", maxLength: 6, nullable: true),
                    TwoFactorCodeExpiration = table.Column<DateTime>(type: "datetime", nullable: true),
                    TwoFactorCounter = table.Column<short>(type: "smallint", nullable: true)
                },
                constraints: table =>
                {
                });

            migrationBuilder.CreateTable(
                name: "tblLisans",
                columns: table => new
                {
                    lKno = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    firma = table.Column<long>(type: "bigint", nullable: false, comment: "tblFirma.fkno ile eslestir"),
                    lisansTip = table.Column<int>(type: "int", nullable: false, comment: "0: tarih aralikli lisans, 1: suresiz lisans"),
                    cihaz = table.Column<long>(type: "bigint", nullable: false, comment: "tblCihaz.ckno ile eslestir"),
                    kayitTarih = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())"),
                    baslangicTarih = table.Column<DateTime>(type: "datetime", nullable: true),
                    bitisTarih = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tblLisans", x => x.lKno);
                });

            migrationBuilder.CreateTable(
                name: "tblMusteri",
                columns: table => new
                {
                    mkno = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    musteri = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true),
                    firma = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "tblBaslik");

            migrationBuilder.DropTable(
                name: "tblBildirim");

            migrationBuilder.DropTable(
                name: "tblCihaz");

            migrationBuilder.DropTable(
                name: "tblEpc");

            migrationBuilder.DropTable(
                name: "tblFirma");

            migrationBuilder.DropTable(
                name: "tblKoli");

            migrationBuilder.DropTable(
                name: "tblKullanici");

            migrationBuilder.DropTable(
                name: "tblLisans");

            migrationBuilder.DropTable(
                name: "tblMusteri");
        }
    }
}
