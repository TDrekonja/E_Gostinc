namespace E_Gostinc.Models.DTOs
{
    public class ArtikelDto
    {
        public int Id { get; set; }
        public string Naziv { get; set; }
        public decimal Cena_brez_ddv { get; set; }
        public int VrstaID { get; set; }
        public string VrstaNaziv { get; set; }
        public decimal Davek { get; set; }
    }
}