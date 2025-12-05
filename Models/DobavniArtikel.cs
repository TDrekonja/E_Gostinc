using System;
using System.Collections.Generic;

namespace E_Gostinc.Models;


public class DobavniArtikel
{
    public int ID { get; set; }
    public string Naziv { get; set; }
    public string Enota { get; set; } 
    public int VrstaID { get; set; }
    public Vrsta Vrsta { get; set; }
    public ICollection<DobavaVSkladisce> DobaveVSkladiscu { get; set; }	
}