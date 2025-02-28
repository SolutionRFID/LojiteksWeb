using System;
using System.Collections.Generic;

namespace LojiteksWeb.Models;

public partial class TblEpc
{
    public long EKno { get; set; }

    public long? BaslikNo { get; set; }

    public long? KoliNo { get; set; }

    public string? Epc { get; set; }

    public string? Upc { get; set; }

    public string? Beden { get; set; }

    public DateTime? KayitTarihi { get; set; }

    public bool? Silindi { get; set; }

    public long? Firma { get; set; }
}
