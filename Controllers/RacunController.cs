using System;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using E_Gostinc.Data;
using E_Gostinc.Models;

namespace E_Gostinc.Controllers
{
    [Authorize]  
    public class RacunController : Controller
    {
        private readonly ArtikelContext _context;
        
        public RacunController(ArtikelContext context)
        {
            _context = context;
        }
        
        public IActionResult Index(DateTime? odDatum, DateTime? doDatum, string status)
        {
            var query = _context.Racun
                .Include(r => r.Uporabnik)
                .AsQueryable();
            
            if (odDatum.HasValue)
            {
                query = query.Where(r => r.Datum >= odDatum.Value);
            }
            
            if (doDatum.HasValue)
            {
                var doDatumEnd = doDatum.Value.Date.AddDays(1).AddTicks(-1);
                query = query.Where(r => r.Datum <= doDatumEnd);
            }
            
            if (!string.IsNullOrEmpty(status))
            {
                query = query.Where(r => r.Status == status);
            }
            
            var racuni = query
                .OrderByDescending(r => r.Datum)
                .ToList();
            
            ViewBag.SkupajRacunov = racuni.Count;
            ViewBag.SkupajVrednost = racuni.Sum(r => r.Skupaj_z_ddv);
            ViewBag.PovprecnaVrednost = racuni.Any() ? racuni.Average(r => r.Skupaj_z_ddv) : 0;
            
            ViewBag.OdDatum = odDatum;
            ViewBag.DoDatum = doDatum;
            ViewBag.Status = status;
            
            return View(racuni);
        }
        
        public IActionResult Podrobnosti(int id)
        {
            var racun = _context.Racun
                .Include(r => r.Uporabnik)
                .Include(r => r.IzdelekiGrejoVn)
                    .ThenInclude(i => i.Artikel)
                        .ThenInclude(a => a.Vrsta)
                .Include(r => r.IzdelekiGrejoVn)
                    .ThenInclude(i => i.Skladisce)
                .FirstOrDefault(r => r.ID == id);
            
            if (racun == null)
            {
                TempData["Error"] = "Račun ne obstaja!";
                return RedirectToAction("Index");
            }
            
            return View(racun);
        }
        
        [HttpPost]
        [Authorize(Roles = "Admin,Manager")]
        public IActionResult Storniraj(int id, string razlog)
        {
            var racun = _context.Racun.Find(id);
            
            if (racun == null)
            {
                TempData["Error"] = "Račun ne obstaja!";
                return RedirectToAction("Index");
            }
            
            if (racun.Status == "Storniran")
            {
                TempData["Error"] = "Račun je že storniran!";
                return RedirectToAction("Podrobnosti", new { id });
            }
            
            try
            {
                racun.Status = "Storniran";
                
                _context.SaveChanges();
                
                TempData["Success"] = $"Račun #{id} je bil storniran.";
                return RedirectToAction("Podrobnosti", new { id });
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"Napaka: {ex.Message}";
                return RedirectToAction("Podrobnosti", new { id });
            }
        }
        
        public IActionResult DnevnoPorocilo(DateTime? datum)
        {
            var izbraniDatum = datum ?? DateTime.Today;
            
            var racuni = _context.Racun
                .Include(r => r.Uporabnik)
                .Include(r => r.IzdelekiGrejoVn)
                    .ThenInclude(i => i.Artikel)
                        .ThenInclude(a => a.Vrsta)
                .Where(r => r.Datum.Date == izbraniDatum.Date && r.Status == "Zakljucen")
                .OrderBy(r => r.Datum)
                .ToList();
            
            var poVrstah = racuni
                .SelectMany(r => r.IzdelekiGrejoVn)
                .GroupBy(i => i.Artikel.Vrsta.Naziv)
                .Select(g => new
                {
                    Vrsta = g.Key,
                    Kolicina = g.Sum(i => i.Kolicina),
                    Vrednost = g.Sum(i => i.Kolicina * i.Artikel.Cena_brez_ddv * (1 + i.Artikel.Vrsta.Davek / 100))
                })
                .OrderByDescending(x => x.Vrednost)
                .ToList();
            
            var najArtikli = racuni
                .SelectMany(r => r.IzdelekiGrejoVn)
                .GroupBy(i => i.Artikel.Naziv)
                .Select(g => new
                {
                    Artikel = g.Key,
                    Kolicina = g.Sum(i => i.Kolicina)
                })
                .OrderByDescending(x => x.Kolicina)
                .Take(10)
                .ToList();
            
            ViewBag.IzbraniDatum = izbraniDatum;
            ViewBag.SkupajRacunov = racuni.Count;
            ViewBag.SkupajVrednost = racuni.Sum(r => r.Skupaj_z_ddv);
            ViewBag.PovprecnaVrednost = racuni.Any() ? racuni.Average(r => r.Skupaj_z_ddv) : 0;
            ViewBag.PoVrstah = poVrstah;
            ViewBag.NajArtikli = najArtikli;
            
            return View(racuni);
        }
    }
}