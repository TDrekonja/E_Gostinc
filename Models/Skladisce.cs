using System;
using System.Collections.Generic;

namespace E_Gostinc.Models;

    public class Skladisce
    {
        public int ID { get; set; }
        public string Naziv { get; set; }

        public ICollection<IzdelekGreVn> IzdelekiGrejoVn { get; set; }

        public ICollection<DobavaVSkladisce> DobaveVSkladiscu { get; set; }

    }
