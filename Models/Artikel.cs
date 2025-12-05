using System;
using System.Collections.Generic;

namespace E_Gostinc.Models;

public class Artikel
{
    public int ID { get; set; }
    public string Naziv { get; set; }
    public decimal Cena_brez_ddv { get; set; }


    public int VrstaID { get; set; }

    public Vrsta Vrsta { get; set; }


    public ICollection<IzdelekGreVn> IzdelekiGrejoVn { get; set; }

}
