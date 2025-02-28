using System;
using System.Collections.Generic;

namespace LojiteksWeb.Models;

public partial class TblKoli
{
    public long SKno { get; set; }

    public long? BaslikNo { get; set; }

    public string? KoliBarkodu { get; set; }

    public int? Adet { get; set; }

    public long? KoliId { get; set; }

    public bool? Silindi { get; set; }

    public long? Firma { get; set; }

    public DateTime? Tarih { get; set; }
}
