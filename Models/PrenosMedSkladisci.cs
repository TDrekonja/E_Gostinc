using System;

namespace E_Gostinc.Models
{
    public class PrenosMedSkladisci
    {
        public int ID { get; set; }
        public DateTime Datum { get; set; }
        public int IzSkladisca_id { get; set; }
        public int VSkladisce_id { get; set; }
        public int DobavniArtikel_Id { get; set; }
        public int Kolicina { get; set; }
        
        // ✅ SPREMENI NA string namesto int (Identity uporablja string ID-je)
        public string Uporabnik_id { get; set; }
        public string? Opomba { get; set; }
        
        // Navigation properties
        public Skladisce IzSkladisca { get; set; }
        public Skladisce VSkladisce { get; set; }
        public DobavniArtikel DobavniArtikel { get; set; }
        public Uporabnik Uporabnik { get; set; }
    }
}