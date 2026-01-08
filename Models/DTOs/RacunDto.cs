namespace E_Gostinc.Models.DTOs
{
    public class RacunDto
    {
        public int ID { get; set; }
        public DateTime Datum { get; set; }
        public string Izdal_uporabnik_id { get; set; }
        public decimal Skupaj_brez_ddv { get; set; }
        public decimal Skupaj_z_ddv { get; set; }
        public string Status { get; set; } 
        public Uporabnik Uporabnik { get; set; }
    }
}