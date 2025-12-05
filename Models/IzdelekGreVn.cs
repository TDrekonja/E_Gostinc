using System;
using System.Collections.Generic;

namespace E_Gostinc.Models;

    public class IzdelekGreVn
    {
        public int ID { get; set; } 
        public int Kolicina { get; set; }

        public int Skladisce_id { get; set; }
        public int Racun_id { get; set; }
        public int Artikel_id { get; set; }

        public Skladisce Skladisce { get; set; }
        public Racun Racun { get; set; }
        public Artikel Artikel { get; set; }

    }
