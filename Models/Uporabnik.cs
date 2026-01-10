using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;

namespace E_Gostinc.Models;

    public class Uporabnik : IdentityUser
    {
        public string Delovno_mesto { get; set; }

        public ICollection<Racun> Racuni { get; set; }
        public ICollection<Dobava> Dobave { get; set; }

    }
