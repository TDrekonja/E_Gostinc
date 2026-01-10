using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using E_Gostinc.Data;
using E_Gostinc.Models;
using Microsoft.AspNetCore.Authorization;

namespace E_Gostinc.Controllers
{
    [Authorize] 
    public class BarController : Controller
    {
        private readonly ArtikelContext _context;

        public BarController(ArtikelContext context)
        {
            _context = context;
        }

        public IActionResult Zaloga()
        {
            var barSkladisce = _context.Skladisce.FirstOrDefault(s => s.Naziv == "Bar");
            var glavnoSkladisce = _context.Skladisce.FirstOrDefault(s => s.Naziv != "Bar");

            if (barSkladisce == null || glavnoSkladisce == null)
            {
                TempData["Error"] = "Skladišča niso pravilno nastavljena!";
                return RedirectToAction("Index", "Home");
            }

            var povezave = _context.ArtikelDobavniArtikelPovezava
                .Include(p => p.Artikel)
                .Include(p => p.DobavniArtikel)
                .ToList();

            var dobavniArtikli = povezave
                .Select(p => p.DobavniArtikel_Id)
                .Distinct()
                .ToList();

            var zaloge = new List<BarZalogaViewModel>();

            foreach (var dobavniArtikelId in dobavniArtikli)
            {
                var dobavniArtikel = _context.DobavniArtikel.Find(dobavniArtikelId);
                if (dobavniArtikel == null) continue;

                // Zaloga v glavnem skladišču
                var skladisceZaloga = _context.DobavaVSkladisce
                    .Where(d => d.Skladisce_id == glavnoSkladisce.ID &&
                                d.DobavniArtikel_Id == dobavniArtikelId)
                    .Sum(d => (int?)d.Kolicina) ?? 0;

                // Zaloga v baru
                var zalogaVBaru = _context.DobavaVSkladisce
                    .Where(d => d.Skladisce_id == barSkladisce.ID &&
                                d.DobavniArtikel_Id == dobavniArtikelId)
                    .Sum(d => (int?)d.Kolicina) ?? 0;

                var povezaniArtikli = povezave
                    .Where(p => p.DobavniArtikel_Id == dobavniArtikelId)
                    .Select(p => new PovezaniArtikel
                    {
                        Naziv = p.Artikel.Naziv,
                        FaktorPretvorbe = p.FaktorPretvorbe
                    })
                    .ToList();

                zaloge.Add(new BarZalogaViewModel
                {
                    DobavniArtikelId = dobavniArtikelId,
                    DobavniArtikelNaziv = dobavniArtikel.Naziv,
                    DobavniArtikelEnota = dobavniArtikel.Enota,
                    ZalogaVBaru = zalogaVBaru,
                    ZalogaVSkladiscu = skladisceZaloga,
                    PovezaniArtikli = povezaniArtikli
                });
            }

            ViewBag.BarSkladisceId = barSkladisce.ID;
            ViewBag.GlavnoSkladisceId = glavnoSkladisce.ID;

            return View(zaloge);
        }

        // Odstrani enoto iz bara
        [HttpPost]
        public IActionResult OdstraniIzBara(int dobavniArtikelId)
        {
            var barSkladisce = _context.Skladisce.FirstOrDefault(s => s.Naziv == "Bar");

            if (barSkladisce == null)
            {
                TempData["Error"] = "Bar skladišče ni najdeno!";
                return RedirectToAction("Zaloga");
            }

            try
            {
                var zaloga = _context.DobavaVSkladisce
                    .Where(d => d.Skladisce_id == barSkladisce.ID &&
                                d.DobavniArtikel_Id == dobavniArtikelId)
                    .Sum(d => (int?)d.Kolicina) ?? 0;

                if (zaloga <= 0)
                {
                    TempData["Error"] = "Ni zaloge za odstranitev!";
                    return RedirectToAction("Zaloga");
                }

                var uporabnikId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (string.IsNullOrEmpty(uporabnikId))
                {
                    TempData["Error"] = "Morate biti prijavljeni!";
                    return RedirectToAction("Login", "Account");
                }

                // Ustvari dobavo
                var dobava = new Dobava
                {
                    Datum = DateTime.Now,
                    Prevzel_uporabnik_id = uporabnikId
                };
                _context.Dobava.Add(dobava);
                _context.SaveChanges();

                // Odstrani 1 enoto iz bara
                _context.DobavaVSkladisce.Add(new DobavaVSkladisce
                {
                    Kolicina = -1,
                    Skladisce_id = barSkladisce.ID,
                    Dobava_id = dobava.ID,
                    DobavniArtikel_Id = dobavniArtikelId
                });

                _context.SaveChanges();

                var artikelNaziv = _context.DobavniArtikel.Find(dobavniArtikelId)?.Naziv;
                TempData["Success"] = $"Odstranjeno: 1x {artikelNaziv}";

                return RedirectToAction("Zaloga");
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"Napaka: {ex.Message}";
                return RedirectToAction("Zaloga");
            }
        }

        // Dodaj enoto v bar (iz skladišča)
        [HttpPost]
        public IActionResult DodajVBar(int dobavniArtikelId)
        {
            var barSkladisce = _context.Skladisce.FirstOrDefault(s => s.Naziv == "Bar");
            var glavnoSkladisce = _context.Skladisce.FirstOrDefault(s => s.Naziv != "Bar");

            if (barSkladisce == null || glavnoSkladisce == null)
            {
                TempData["Error"] = "Skladišča niso pravilno nastavljena!";
                return RedirectToAction("Zaloga");
            }

            try
            {
                var skladisceZaloga = _context.DobavaVSkladisce
                    .Where(d => d.Skladisce_id == glavnoSkladisce.ID &&
                                d.DobavniArtikel_Id == dobavniArtikelId)
                    .Sum(d => (int?)d.Kolicina) ?? 0;

                if (skladisceZaloga <= 0)
                {
                    TempData["Error"] = "Ni zaloge v skladišču!";
                    return RedirectToAction("Zaloga");
                }

                var uporabnikId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (string.IsNullOrEmpty(uporabnikId))
                {
                    TempData["Error"] = "Morate biti prijavljeni!";
                    return RedirectToAction("Login", "Account");
                }

                var dobava = new Dobava
                {
                    Datum = DateTime.Now,
                    Prevzel_uporabnik_id = uporabnikId
                };
                _context.Dobava.Add(dobava);
                _context.SaveChanges();

                // Odvzemi iz skladišča
                _context.DobavaVSkladisce.Add(new DobavaVSkladisce
                {
                    Kolicina = -1,
                    Skladisce_id = glavnoSkladisce.ID,
                    Dobava_id = dobava.ID,
                    DobavniArtikel_Id = dobavniArtikelId
                });

                // Dodaj v bar
                _context.DobavaVSkladisce.Add(new DobavaVSkladisce
                {
                    Kolicina = 1,
                    Skladisce_id = barSkladisce.ID,
                    Dobava_id = dobava.ID,
                    DobavniArtikel_Id = dobavniArtikelId
                });

                _context.SaveChanges();

                var artikelNaziv = _context.DobavniArtikel.Find(dobavniArtikelId)?.Naziv;
                TempData["Success"] = $"Dodano v bar: 1x {artikelNaziv}";

                return RedirectToAction("Zaloga");
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"Napaka: {ex.Message}";
                return RedirectToAction("Zaloga");
            }
        }
    }
}