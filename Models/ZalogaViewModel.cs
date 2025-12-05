namespace E_Gostinc.Models;

public class ZalogaViewModel
{
    public string Artikel { get; set; }
    public string Enota { get; set; }
    public int Zaloga { get; set; }
    public string Vrsta { get; set; }
    public string? Skladisce { get; set; }  // Null za skupno zalogo
}

public class ZalogaFilterViewModel
{
    public List<ZalogaViewModel> Zaloge { get; set; }
    public List<Skladisce> Skladisca { get; set; }
    public int? IzbranaSkladisceId { get; set; }
    public string Sortiranje { get; set; } = "naziv_asc";
    public string VrstniRed { get; set; } = "asc";
}