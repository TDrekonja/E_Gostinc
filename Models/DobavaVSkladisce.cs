using System;
using System.Collections.Generic;

namespace E_Gostinc.Models;
public class DobavaVSkladisce
{
    public int Kolicina { get; set; }

    public int Dobava_id { get; set; }
    public int Skladisce_id { get; set; }
    public int DobavniArtikel_Id { get; set; }

    public Dobava Dobava { get; set; }
    public Skladisce Skladisce { get; set; }
    public DobavniArtikel DobavniArtikel { get; set; }
}

