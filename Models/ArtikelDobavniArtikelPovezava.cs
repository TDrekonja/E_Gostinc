namespace E_Gostinc.Models
{
    public class ArtikelDobavniArtikelPovezava
    {
        public int ID { get; set; }
        public int Artikel_id { get; set; }  // Maloprodajni artikel (v baru)
        public int DobavniArtikel_Id { get; set; }  // Veleprodajni artikel (v skladišču)
        public decimal FaktorPretvorbe { get; set; }  // Koliko maloprodajnih je v enem veleprodajnem
        
        // Primer: 1x sod 30L = 60x točeno 0.5L → FaktorPretvorbe = 60
        // Primer: 1x steklenica vodke 1L = 33.33x shot 0.03L → FaktorPretvorbe = 33.33
        
        public Artikel Artikel { get; set; }
        public DobavniArtikel DobavniArtikel { get; set; }
    }
}