using System;
using System.Collections.Generic;

namespace E_Gostinc.Models;

    public class Racun
    {
        public int ID { get; set; }
        public DateTime Datum { get; set; }
        public string Izdal_uporabnik_id { get; set; }
        public decimal Skupaj_brez_ddv { get; set; }
        public decimal Skupaj_z_ddv { get; set; }
        public string Status { get; set; } // "Odprt", "Zakljucen", "Storniran"

        public Uporabnik Uporabnik { get; set; }

        public ICollection<IzdelekGreVn> IzdelekiGrejoVn { get; set; }
        public ICollection<RacunArtikel> RacunArtikli { get; set; }

    }
