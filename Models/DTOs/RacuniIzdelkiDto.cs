namespace E_Gostinc.Models.DTOs
{
    public class RacuniIzdelkiDto
    {
        public int ArtikelId { get; set; }
        public string Naziv { get; set; }
        public decimal CenaBrezDdv { get; set; }
        public int Kolicina { get; set; }
        public decimal Skupaj { get; set; }
    }

    public class UstvariRacunZahtevo
    {
        public List<int> ArtikelIdi { get; set; }
        public string UserId { get; set; }
    }
    
    public class RacunOdgovorDto
    {
        public int Id { get; set; }
        public DateTime Datum { get; set; }
        public decimal SkupajBrezDdv { get; set; }
        public decimal SkupajZDdv { get; set; }
        public string Status { get; set; }
        public string UporabnikEmail { get; set; }
        public List<RacuniIzdelkiDto> Artikli { get; set; }
    }
}