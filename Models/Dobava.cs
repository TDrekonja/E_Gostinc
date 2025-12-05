using System;
using System.Collections.Generic;

namespace E_Gostinc.Models;

    public class Dobava
    {
        public int ID { get; set; }
        public DateTime Datum { get; set; }
        public string Prevzel_uporabnik_id { get; set; }

        public Uporabnik Uporabnik { get; set; }

        public ICollection<DobavaVSkladisce> Skladisca { get; set; }

    }
