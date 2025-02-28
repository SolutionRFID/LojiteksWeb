using System;
using System.Collections.Generic;

namespace LojiteksWeb.Models;

public partial class TblCihaz
{
    public long Ckno { get; set; }

    public string? Kod { get; set; }

    public string? Tanim { get; set; }

    public string? SeriNo { get; set; }

    /// <summary>
    /// 0 sbox
    /// </summary>
    public int? Tipi { get; set; }

    /// <summary>
    /// tblfirma-fkno ile bağlı
    /// </summary>
    public long? Firma { get; set; }

    /// <summary>
    /// 0 lojiteks 
    /// </summary>
    public int? Uygulama { get; set; }

    public string? ApiUrl { get; set; }

    public string? ApiKey { get; set; }
}
