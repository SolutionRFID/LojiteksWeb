using System;
using System.Collections.Generic;

namespace LojiteksWeb.Models;

public partial class TblKullanici
{
    public long KKno { get; set; }

    public string? KullaniciAdi { get; set; }

    public string? Sifre { get; set; }

    public long? Firma { get; set; }

    public string? AdSoyad { get; set; }

    public int? Yetki { get; set; }

    public string? Email { get; set; }

    public bool EmailConfirmed { get; set; }

    public string? TwoFactorCode { get; set; }

    public DateTime? TwoFactorCodeExpiration { get; set; }

    public short? TwoFactorCounter { get; set; }
}
