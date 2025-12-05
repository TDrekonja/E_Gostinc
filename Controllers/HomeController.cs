using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using E_Gostinc.Models;
using Microsoft.EntityFrameworkCore;
using E_Gostinc.Data;
using System.Text.Json;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;

namespace E_Gostinc.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly ArtikelContext _context;

        public HomeController(ArtikelContext context)
        {
            _context = context;
        }

        public IActionResult Index(int? vrstaId)
        {
            
            var vrste = _context.Vrsta.ToList();
            ViewBag.Vrste = vrste;

            
            var artikli = _context.Artikel
                .Include(a => a.Vrsta)
                .Where(a => !vrstaId.HasValue || a.VrstaID == vrstaId)
                .ToList();
            ViewBag.Artikli = artikli;

            
            var racunViewModel = PripravljanjRacuna();
            ViewBag.RacunViewModel = racunViewModel;

            return View();
        }

        [HttpPost]
        public IActionResult DodajNaRacun(int artikelId, int? vrstaId)
        {
            var racunJson = TempData["RacunIdi"] as string;
            var racunIdi = string.IsNullOrEmpty(racunJson)
                ? new List<int>()
                : JsonSerializer.Deserialize<List<int>>(racunJson);

            racunIdi.Add(artikelId);

            TempData["RacunIdi"] = JsonSerializer.Serialize(racunIdi);
            TempData.Keep("RacunIdi");

            return RedirectToAction("Index", new { vrstaId });
        }

        [HttpPost]
        public IActionResult OdstraniIzRacuna(int artikelId, int? vrstaId)
        {
            var racunJson = TempData["RacunIdi"] as string;
            var racunIdi = string.IsNullOrEmpty(racunJson)
                ? new List<int>()
                : JsonSerializer.Deserialize<List<int>>(racunJson);

            
            racunIdi.Remove(artikelId);

            TempData["RacunIdi"] = JsonSerializer.Serialize(racunIdi);
            TempData.Keep("RacunIdi");

            return RedirectToAction("Index", new { vrstaId });
        }

        [HttpPost]
        public IActionResult Ponastavi(int? vrstaId)
        {
            TempData.Remove("RacunIdi");
            return RedirectToAction("Index", new { vrstaId });
        }

        [HttpPost]
        public IActionResult ZakljuciRacun()
        {
            var racunJson = TempData["RacunIdi"] as string;
            var racunIdi = string.IsNullOrEmpty(racunJson)
                ? new List<int>()
                : JsonSerializer.Deserialize<List<int>>(racunJson);
            
            if (!racunIdi.Any())
            {
                TempData["Error"] = "Račun je prazen!";
                return RedirectToAction("Index");
            }
            
            var barSkladisce = _context.Skladisce.FirstOrDefault(s => s.Naziv == "Bar");
            if (barSkladisce == null)
            {
                TempData["Error"] = "Bar skladišče ni najdeno!";
                return RedirectToAction("Index");
            }
            
            try
            {
                var uporabnikId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (string.IsNullOrEmpty(uporabnikId))
                {
                    TempData["Error"] = "Morate biti prijavljeni!";
                    return RedirectToAction("Login", "Account");
                }
                
                var artikli = _context.Artikel
                    .Include(a => a.Vrsta)
                    .Where(a => racunIdi.Contains(a.ID))
                    .ToList();
                
                var artikliGrouped = racunIdi
                    .GroupBy(id => id)
                    .Select(g => new
                    {
                        ArtikelId = g.Key,
                        Kolicina = g.Count(),
                        Artikel = artikli.First(a => a.ID == g.Key)
                    })
                    .ToList();
                
                
                decimal skupajBrezDdv = 0;
                decimal skupajZDdv = 0;
                
                foreach (var item in artikliGrouped)
                {
                    var cenaBrezDdv = item.Artikel.Cena_brez_ddv * item.Kolicina;
                    var cenaZDdv = cenaBrezDdv * (1 + item.Artikel.Vrsta.Davek / 100);
                    
                    skupajBrezDdv += cenaBrezDdv;
                    skupajZDdv += cenaZDdv;
                }
                
                
                var novRacun = new Racun
                {
                    Datum = DateTime.Now,
                    Skupaj_brez_ddv = skupajBrezDdv,
                    Skupaj_z_ddv = skupajZDdv,
                    Izdal_uporabnik_id = uporabnikId,
                    Status = "Zakljucen"
                };
                
                _context.Racun.Add(novRacun);
                _context.SaveChanges();
                
                
                foreach (var item in artikliGrouped)
                {
                    var izdelekGreVn = new IzdelekGreVn
                    {
                        Racun_id = novRacun.ID,
                        Artikel_id = item.ArtikelId,
                        Skladisce_id = barSkladisce.ID,
                        Kolicina = item.Kolicina
                    };
                    _context.IzdelekGreVn.Add(izdelekGreVn);
                }
                
                _context.SaveChanges();
                
                TempData.Remove("RacunIdi");
                TempData["Success"] = $"Račun #{novRacun.ID} uspešno zaključen! Skupaj: {skupajZDdv:F2} € (z DDV)";
                
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"ERROR: {ex.Message}");
                TempData["Error"] = $"Napaka pri zaključku računa: {ex.Message}";
                return RedirectToAction("Index");
            }
        }

        
        private RacunViewModel PripravljanjRacuna()
        {
            var racunJson = TempData["RacunIdi"] as string;
            var racunIdi = string.IsNullOrEmpty(racunJson)
                ? new List<int>()
                : JsonSerializer.Deserialize<List<int>>(racunJson);

            TempData["RacunIdi"] = racunJson;
            TempData.Keep("RacunIdi");

            if (!racunIdi.Any())
            {
                return new RacunViewModel();
            }

            var artikli = _context.Artikel
                .Include(a => a.Vrsta)
                .Where(a => racunIdi.Contains(a.ID))
                .ToList();

            
            var artikliGrouped = racunIdi
                .GroupBy(id => id)
                .Select(g =>
                {
                    var artikel = artikli.First(a => a.ID == g.Key);
                    var kolicina = g.Count();
                    var cenaBrezDdv = artikel.Cena_brez_ddv;
                    var cenaZDdv = cenaBrezDdv * (1 + artikel.Vrsta.Davek / 100);

                    return new RacunArtikelDto
                    {
                        ArtikelId = artikel.ID,
                        Naziv = artikel.Naziv,
                        Cena_brez_ddv = cenaBrezDdv,
                        Cena_z_ddv = cenaZDdv,
                        Kolicina = kolicina,
                        Skupaj_brez_ddv = cenaBrezDdv * kolicina,
                        Skupaj_z_ddv = cenaZDdv * kolicina,
                        VrstaID = artikel.VrstaID,
                        Davek = artikel.Vrsta.Davek
                    };
                })
                .ToList();

            var viewModel = new RacunViewModel
            {
                Artikli = artikliGrouped,
                SkupajBrezDdv = artikliGrouped.Sum(a => a.Skupaj_brez_ddv),
                SkupajZDdv = artikliGrouped.Sum(a => a.Skupaj_z_ddv)
            };

            viewModel.SkupajDdv = viewModel.SkupajZDdv - viewModel.SkupajBrezDdv;

            return viewModel;
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}