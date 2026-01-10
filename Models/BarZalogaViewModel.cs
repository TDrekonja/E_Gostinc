namespace E_Gostinc.Models
{
    public class BarZalogaViewModel
    {
        public int DobavniArtikelId { get; set; }
        public string DobavniArtikelNaziv { get; set; }
        public string DobavniArtikelEnota { get; set; }
        
        public int ZalogaVBaru { get; set; }  // Število enot v baru
        public int ZalogaVSkladiscu { get; set; }  // Število enot v glavnem skladišču
        
        public List<PovezaniArtikel> PovezaniArtikli { get; set; }
        
        public BarZalogaViewModel()
        {
            PovezaniArtikli = new List<PovezaniArtikel>();
        }
    }
    
    public class PovezaniArtikel
    {
        public string Naziv { get; set; }
        public decimal FaktorPretvorbe { get; set; }
    }
    
    public class ZamenjajBucViewModel
    {
        public int DobavniArtikelId { get; set; }
        public string DobavniArtikelNaziv { get; set; }
        public int SkladisceZaloga { get; set; }
        public int BarSkladisceId { get; set; }
        public int GlavnoSkladisceId { get; set; }
    }
}