namespace E_Gostinc.Models
{
    public class RacunSeznamViewModel
    {
        public List<Racun> Racuni { get; set; }
        public int SkupajRacunov { get; set; }
        public decimal SkupajVrednost { get; set; }
        public decimal PovprecnaVrednost { get; set; }
        public DateTime? OdDatum { get; set; }
        public DateTime? DoDatum { get; set; }
        public string Status { get; set; }
    }
}