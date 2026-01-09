namespace E_Gostinc.Models.DTOs
{
    public class RacunDto
    {
        public int Id { get; set; }
        public DateTime Datum { get; set; }
        public decimal Skupaj_brez_ddv { get; set; }
        public decimal Skupaj_z_ddv { get; set; }
        public string Status { get; set; }
        public string UporabnikEmail { get; set; }
    }
}