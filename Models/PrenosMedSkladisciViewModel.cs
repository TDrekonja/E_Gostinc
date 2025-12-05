namespace E_Gostinc.Models
{
    public class PrenosMedSkladisciViewModel
    {
        public List<SkladisceZalogaDto> Zaloge { get; set; }
        public List<Skladisce> Skladisca { get; set; }
        public int IzSkladiscaId { get; set; }
        public string IzSkladiscaNaziv { get; set; }
        
        public PrenosMedSkladisciViewModel()
        {
            Zaloge = new List<SkladisceZalogaDto>();
            Skladisca = new List<Skladisce>();
        }
    }
    
    public class SkladisceZalogaDto
    {
        public int DobavniArtikelId { get; set; }
        public string Naziv { get; set; }
        public string Enota { get; set; }
        public int TrenutnaZaloga { get; set; }
        public string Vrsta { get; set; }
        public int KolicinaPrenosa { get; set; }  // Koliko uporabnik želi prenesti
        public bool Izbrano { get; set; }          // Ali je artikel izbran za prenos
    }
    
    public class PrenosRequestDto
    {
        public int IzSkladiscaId { get; set; }
        public int VSkladisceId { get; set; }
        public List<PrenosArtikelDto> Artikli { get; set; }
        
        public PrenosRequestDto()
        {
            Artikli = new List<PrenosArtikelDto>();
        }
    }
    
    public class PrenosArtikelDto
    {
        public int DobavniArtikelId { get; set; }
        public int Kolicina { get; set; }
    }
}