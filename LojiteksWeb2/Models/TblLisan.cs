using System;
using System.Collections.Generic;

namespace LojiteksWeb.Models;

public partial class TblLisan
{
    public long LKno { get; set; }

    /// <summary>
    /// tblFirma.fkno ile eslestir
    /// </summary>
    public long Firma { get; set; }

    /// <summary>
    /// 0: tarih aralikli lisans, 1: suresiz lisans
    /// </summary>
    public int LisansTip { get; set; }

    /// <summary>
    /// tblCihaz.ckno ile eslestir
    /// </summary>
    public long Cihaz { get; set; }

    public DateTime KayitTarih { get; set; }

    public DateTime? BaslangicTarih { get; set; }

    public DateTime? BitisTarih { get; set; }
}
