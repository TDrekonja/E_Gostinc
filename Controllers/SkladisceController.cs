using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using E_Gostinc.Models;
using Microsoft.EntityFrameworkCore;
using E_Gostinc.Data;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;

namespace E_Gostinc.Controllers;

[Authorize] 
public class SkladisceController : Controller
{

    private readonly ArtikelContext _context;
    private readonly UserManager<Uporabnik> _userManager;

    public SkladisceController(ArtikelContext context, UserManager<Uporabnik> userManager)
    {
        _context = context;
        _userManager = userManager;
    }

    public IActionResult Index(int? skladisceId, string sortBy = "naziv", string order = "asc")
    {
        var skladisca = _context.Skladisce.ToList();

        var query = _context.DobavaVSkladisce
            .Include(d => d.DobavniArtikel)
                .ThenInclude(da => da.Vrsta)
            .Include(d => d.Skladisce)
            .AsQueryable();

        if (skladisceId.HasValue && skladisceId.Value > 0)
        {
            query = query.Where(d => d.Skladisce_id == skladisceId.Value);
        }

        var zaloge = query
            .GroupBy(d => new
            {
                d.DobavniArtikel.ID,
                d.DobavniArtikel.Naziv,
                d.DobavniArtikel.Enota,
                VrstaNaziv = d.DobavniArtikel.Vrsta.Naziv,
                SkladisceNaziv = skladisceId.HasValue ? d.Skladisce.Naziv : null
            })
            .Select(g => new ZalogaViewModel
            {
                Artikel = g.Key.Naziv,
                Enota = g.Key.Enota,
                Zaloga = g.Sum(x => x.Kolicina),
                Vrsta = g.Key.VrstaNaziv,
                Skladisce = g.Key.SkladisceNaziv
            })
            .ToList();

        // Sortiranje
        zaloge = sortBy.ToLower() switch
        {
            "naziv" => order == "desc"
                ? zaloge.OrderByDescending(z => z.Artikel).ToList()
                : zaloge.OrderBy(z => z.Artikel).ToList(),

            "zaloga" => order == "desc"
                ? zaloge.OrderByDescending(z => z.Zaloga).ToList()
                : zaloge.OrderBy(z => z.Zaloga).ToList(),

            "vrsta" => order == "desc"
                ? zaloge.OrderByDescending(z => z.Vrsta).ThenBy(z => z.Artikel).ToList()
                : zaloge.OrderBy(z => z.Vrsta).ThenBy(z => z.Artikel).ToList(),

            _ => zaloge.OrderBy(z => z.Artikel).ToList()
        };

        var viewModel = new ZalogaFilterViewModel
        {
            Zaloge = zaloge,
            Skladisca = skladisca,
            IzbranaSkladisceId = skladisceId,
            Sortiranje = sortBy,
            VrstniRed = order
        };

        return View(viewModel);
    }

    public IActionResult ZalogaPoSkladiscih()
    {
        var zaloge = _context.DobavaVSkladisce
            .Include(d => d.DobavniArtikel)
                .ThenInclude(da => da.Vrsta)
            .Include(d => d.Skladisce)
            .GroupBy(d => new
            {
                d.Skladisce.ID,
                SkladisceNaziv = d.Skladisce.Naziv,
                ArtikelNaziv = d.DobavniArtikel.Naziv,
                d.DobavniArtikel.Enota,
                VrstaNaziv = d.DobavniArtikel.Vrsta.Naziv
            })
            .Select(g => new ZalogaViewModel
            {
                Skladisce = g.Key.SkladisceNaziv,
                Artikel = g.Key.ArtikelNaziv,
                Enota = g.Key.Enota,
                Zaloga = g.Sum(x => x.Kolicina),
                Vrsta = g.Key.VrstaNaziv
            })
            .OrderBy(z => z.Skladisce)
            .ThenBy(z => z.Vrsta)
            .ThenBy(z => z.Artikel)
            .ToList();

        return View(zaloge);
    }

    public IActionResult ZalogaPoVrstiIzdelkov()
    {
        var zaloge = _context.DobavaVSkladisce
            .Include(d => d.DobavniArtikel)
                .ThenInclude(da => da.Vrsta)
            .GroupBy(d => new
            {
                d.DobavniArtikel.Naziv,
                d.DobavniArtikel.Enota,
                VrstaNaziv = d.DobavniArtikel.Vrsta.Naziv
            })
            .Select(g => new ZalogaViewModel
            {
                Artikel = g.Key.Naziv,
                Enota = g.Key.Enota,
                Zaloga = g.Sum(x => x.Kolicina),
                Vrsta = g.Key.VrstaNaziv
            })
            .OrderBy(z => z.Vrsta)
            .ThenBy(z => z.Artikel)
            .ToList();

        return View(zaloge);
    }

    public IActionResult ZalogaPoKolicini(string order = "desc")
    {
        var zaloge = _context.DobavaVSkladisce
            .Include(d => d.DobavniArtikel)
                .ThenInclude(da => da.Vrsta)
            .GroupBy(d => new
            {
                d.DobavniArtikel.Naziv,
                d.DobavniArtikel.Enota,
                VrstaNaziv = d.DobavniArtikel.Vrsta.Naziv
            })
            .Select(g => new ZalogaViewModel
            {
                Artikel = g.Key.Naziv,
                Enota = g.Key.Enota,
                Zaloga = g.Sum(x => x.Kolicina),
                Vrsta = g.Key.VrstaNaziv
            })
            .ToList();

        zaloge = order == "asc"
            ? zaloge.OrderBy(z => z.Zaloga).ToList()
            : zaloge.OrderByDescending(z => z.Zaloga).ToList();

        ViewBag.Order = order;
        return View(zaloge);
    }

    public IActionResult PrenosMedSkladisci()
    {
        var skladisca = _context.Skladisce.ToList();
        ViewBag.Skladisca = skladisca;
        return View();
    }


    public IActionResult IzberiArtikle(int izSkladiscaId)
    {
        var izSkladisca = _context.Skladisce.Find(izSkladiscaId);
        if (izSkladisca == null)
        {
            return NotFound();
        }


        var zaloge = _context.DobavaVSkladisce
            .Include(d => d.DobavniArtikel)
                .ThenInclude(da => da.Vrsta)
            .Where(d => d.Skladisce_id == izSkladiscaId)
            .GroupBy(d => new
            {
                d.DobavniArtikel.ID,
                d.DobavniArtikel.Naziv,
                d.DobavniArtikel.Enota,
                VrstaNaziv = d.DobavniArtikel.Vrsta.Naziv
            })
            .Select(g => new SkladisceZalogaDto
            {
                DobavniArtikelId = g.Key.ID,
                Naziv = g.Key.Naziv,
                Enota = g.Key.Enota,
                TrenutnaZaloga = g.Sum(x => x.Kolicina),
                Vrsta = g.Key.VrstaNaziv,
                KolicinaPrenosa = 0,
                Izbrano = false
            })
            .Where(z => z.TrenutnaZaloga > 0)
            .OrderBy(z => z.Vrsta)
            .ThenBy(z => z.Naziv)
            .ToList();

        var viewModel = new PrenosMedSkladisciViewModel
        {
            Zaloge = zaloge,
            Skladisca = _context.Skladisce.Where(s => s.ID != izSkladiscaId).ToList(),
            IzSkladiscaId = izSkladiscaId,
            IzSkladiscaNaziv = izSkladisca.Naziv
        };

        return View(viewModel);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult IzvrsiPrenos(int izSkladiscaId, int vSkladisceId, List<int> izbraniArtikli, List<int> kolicine)
    {
        // DODAJ DEBUGGING
        Console.WriteLine($"=== PRENOS DEBUG ===");
        Console.WriteLine($"izSkladiscaId: {izSkladiscaId}");
        Console.WriteLine($"vSkladisceId: {vSkladisceId}");
        Console.WriteLine($"izbraniArtikli count: {izbraniArtikli?.Count ?? 0}");
        Console.WriteLine($"kolicine count: {kolicine?.Count ?? 0}");

        if (izbraniArtikli != null)
        {
            Console.WriteLine($"Artikli: {string.Join(", ", izbraniArtikli)}");
        }
        if (kolicine != null)
        {
            Console.WriteLine($"Kolicine: {string.Join(", ", kolicine)}");
        }

        // Validacije
        if (izSkladiscaId == vSkladisceId)
        {
            TempData["Error"] = "Ne morete prenesti v isto skladišče!";
            return RedirectToAction("IzberiArtikle", new { izSkladiscaId });
        }

        if (izbraniArtikli == null || !izbraniArtikli.Any())
        {
            TempData["Error"] = "Izberite vsaj en artikel za prenos!";
            return RedirectToAction("IzberiArtikle", new { izSkladiscaId });
        }

        if (kolicine == null || kolicine.Count != izbraniArtikli.Count)
        {
            TempData["Error"] = "Neskladje med izbranimi artikli in količinami!";
            return RedirectToAction("IzberiArtikle", new { izSkladiscaId });
        }

        // Pridobi uporabnika
        var uporabnikId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrEmpty(uporabnikId))
        {
            TempData["Error"] = "Morate biti prijavljeni!";
            return RedirectToAction("Login", "Account");
        }

        try
        {
            var trenutniDatum = DateTime.Now;

            for (int i = 0; i < izbraniArtikli.Count; i++)
            {
                var artikelId = izbraniArtikli[i];
                var kolicina = kolicine[i];

                if (kolicina <= 0) continue;

                // Preveri zalogo
                var trenutnaZaloga = _context.DobavaVSkladisce
                    .Where(d => d.Skladisce_id == izSkladiscaId && d.DobavniArtikel_Id == artikelId)
                    .Sum(d => d.Kolicina);

                if (kolicina > trenutnaZaloga)
                {
                    var artikel = _context.DobavniArtikel.Find(artikelId);
                    TempData["Error"] = $"Premalo zaloge za artikel '{artikel?.Naziv}'. Na voljo: {trenutnaZaloga}, želeno: {kolicina}";
                    return RedirectToAction("IzberiArtikle", new { izSkladiscaId });
                }

                // Ustvari novo dobavo
                var novaDobava = new Dobava
                {
                    Datum = trenutniDatum,
                    Prevzel_uporabnik_id = uporabnikId
                };
                _context.Dobava.Add(novaDobava);
                _context.SaveChanges();

                // Dodaj v ciljno skladišče
                _context.DobavaVSkladisce.Add(new DobavaVSkladisce
                {
                    Kolicina = kolicina,
                    Skladisce_id = vSkladisceId,
                    Dobava_id = novaDobava.ID,
                    DobavniArtikel_Id = artikelId
                });

                // Odvzemi iz izvornega skladišča
                _context.DobavaVSkladisce.Add(new DobavaVSkladisce
                {
                    Kolicina = -kolicina,
                    Skladisce_id = izSkladiscaId,
                    Dobava_id = novaDobava.ID,
                    DobavniArtikel_Id = artikelId
                });

                // Evidentiraj prenos
                _context.PrenosMedSkladisci.Add(new PrenosMedSkladisci
                {
                    Datum = trenutniDatum,
                    IzSkladisca_id = izSkladiscaId,
                    VSkladisce_id = vSkladisceId,
                    DobavniArtikel_Id = artikelId,
                    Kolicina = kolicina,
                    Uporabnik_id = uporabnikId,
                    Opomba = $"Prenos med skladišči"
                });
            }

            _context.SaveChanges();
            TempData["Success"] = $"Prenos uspešno izveden! Prenesenih {izbraniArtikli.Count} artiklov.";
            return RedirectToAction("Index");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"ERROR: {ex.Message}");
            Console.WriteLine($"Stack trace: {ex.StackTrace}");
            TempData["Error"] = $"Napaka pri prenosu: {ex.Message}";
            return RedirectToAction("IzberiArtikle", new { izSkladiscaId });
        }
    }

    // GET: Zgodovina prenosov
    public IActionResult ZgodovinaPrenosov()
    {
        var prenosi = _context.PrenosMedSkladisci
            .Include(p => p.IzSkladisca)
            .Include(p => p.VSkladisce)
            .Include(p => p.DobavniArtikel)
            .Include(p => p.Uporabnik)
            .OrderByDescending(p => p.Datum)
            .ToList();

        return View(prenosi);
    }
}