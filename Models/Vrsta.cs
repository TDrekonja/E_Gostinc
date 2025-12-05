using System;
using System.Collections.Generic;

namespace E_Gostinc.Models;
public class Vrsta
{
    public int ID { get; set; }
    public string Naziv { get; set; }

    public int Davek { get; set; }

    public ICollection<Artikel> Artikli { get; set; }
    public ICollection<DobavniArtikel> DobavniArtikli { get; set; }
}