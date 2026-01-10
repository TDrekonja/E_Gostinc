namespace E_Gostinc.Models
{
    public class RacunArtikelDto
    {
        public int ArtikelId { get; set; }
        public string Naziv { get; set; }
        public decimal Cena_brez_ddv { get; set; }
        public decimal Cena_z_ddv { get; set; }
        public int Kolicina { get; set; }
        public decimal Skupaj_brez_ddv { get; set; }
        public decimal Skupaj_z_ddv { get; set; }
        public int VrstaID { get; set; }
        public decimal Davek { get; set; }
    }
    
    public class RacunViewModel
    {
        public List<RacunArtikelDto> Artikli { get; set; }
        public decimal SkupajBrezDdv { get; set; }
        public decimal SkupajZDdv { get; set; }
        public decimal SkupajDdv { get; set; }
        
        public RacunViewModel()
        {
            Artikli = new List<RacunArtikelDto>();
        }
    }
}