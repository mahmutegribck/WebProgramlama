using System;
using System.Collections.Generic;

namespace YemekTarifleriWebProjesi.Models;

public partial class Kategoriler
{
    public int KategoriId { get; set; }

    public string? Kategoriadi { get; set; }

    public bool? Aktif { get; set; }

    public bool? Silindi { get; set; }

    public virtual ICollection<YemekTarifleri> YemekTarifleris { get; } = new List<YemekTarifleri>();
}
