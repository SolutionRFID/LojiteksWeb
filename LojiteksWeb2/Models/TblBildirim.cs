using System;
using System.Collections.Generic;

namespace LojiteksWeb.Models;

public partial class TblBildirim
{
    public long Bikno { get; set; }

    public string? Kullanici { get; set; }

    public string? Bildirim { get; set; }

    public DateTime? Tarih { get; set; }

    public byte? Durum { get; set; }
}
