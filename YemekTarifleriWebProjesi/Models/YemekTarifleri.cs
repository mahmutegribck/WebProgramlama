﻿using System;
using System.Collections.Generic;

namespace YemekTarifleriWebProjesi.Models;

public partial class YemekTarifleri
{
    public int TarifId { get; set; }

    public string? Yemekadi { get; set; }

    public string? Tarif { get; set; }

    public byte? Sira { get; set; }

    public int? KategoriId { get; set; }

    public DateTime? EklemeTarihi { get; set; }

    public bool? Aktif { get; set; }

    public bool? Silindi { get; set; }

    public virtual Kategoriler? Kategori { get; set; }

    public virtual ICollection<Yorumlar> Yorumlars { get; } = new List<Yorumlar>();
}
