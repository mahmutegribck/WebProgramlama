using System;
using System.Collections.Generic;

namespace YemekTarifleriWebProjesi.Models;

public partial class Menuler
{
    public int MenuId { get; set; }

    public string? Baslik { get; set; }

    public string? Urll { get; set; }

    public byte? Sira { get; set; }

    public int? UstId { get; set; }

    public bool? Aktif { get; set; }

    public bool? Silindi { get; set; }
}
