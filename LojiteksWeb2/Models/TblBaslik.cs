using System;
using System.Collections.Generic;

namespace LojiteksWeb.Models;

public partial class TblBaslik
{
    public long Bkno { get; set; }

    /// <summary>
    /// 0 - sevkiyat
    /// </summary>
    public int? Tipi { get; set; }

    public string? Aciklama { get; set; }

    /// <summary>
    /// tblMustrei-mkno ile bağlı
    /// </summary>
    public long? Musteri { get; set; }

    public DateOnly? GonderimTarihi { get; set; }

    public int? GonderiAdedi { get; set; }

    public DateTime? KayitTarihi { get; set; }

    public string? Kullanici { get; set; }

    public bool? Silindi { get; set; }

    public long? Firma { get; set; }

    public string? Sevkiyat { get; set; }

    public string? Po { get; set; }

    public long? CihazId { get; set; }
}
