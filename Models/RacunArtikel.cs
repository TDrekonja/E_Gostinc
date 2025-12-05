namespace E_Gostinc.Models
{
    public class RacunArtikel
    {
        public int ID { get; set; }
        public int Racun_id { get; set; }
        public int Artikel_id { get; set; }
        public int Kolicina { get; set; }
        public decimal Cena_brez_ddv { get; set; }
        public decimal Cena_z_ddv { get; set; }
        
        public Racun Racun { get; set; }
        public Artikel Artikel { get; set; }
    }
}