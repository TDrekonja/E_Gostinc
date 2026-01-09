using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using E_Gostinc.Data;
using E_Gostinc.Models;
using E_Gostinc.Models.DTOs;

namespace E_Gostinc.Controllers.Api
{
    [Route("api/v1/racun")]
    [ApiController]
    public class RacunApiController : ControllerBase
    {
        private readonly ArtikelContext _context;

        public RacunApiController(ArtikelContext context)
        {
            _context = context;
        }

        // GET: api/v1/racun/dnevni
        [HttpGet("dnevni")]
        [AllowAnonymous]
        public async Task<ActionResult<IEnumerable<RacunDto>>> GetDnevniRacuni()
        {
            var danes = DateTime.Today;
            
            var racuni = await _context.Racun
                .Include(r => r.Uporabnik)
                .Where(r => r.Datum.Date == danes && r.Status == "Zakljucen")
                .OrderByDescending(r => r.Datum)
                .Select(r => new RacunDto
                {
                    Id = r.ID,
                    Datum = r.Datum,
                    Skupaj_brez_ddv = r.Skupaj_brez_ddv,
                    Skupaj_z_ddv = r.Skupaj_z_ddv,
                    Status = r.Status,
                    UporabnikEmail = r.Uporabnik.Email ?? "N/A"
                })
                .ToListAsync();

            return Ok(racuni);
        }

        // GET: api/v1/racun/{id}
        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<ActionResult<RacunOdgovorDto>> GetRacun(int id)
        {
            var racun = await _context.Racun
                .Include(r => r.IzdelekiGrejoVn)
                    .ThenInclude(i => i.Artikel)
                        .ThenInclude(a => a.Vrsta)
                .FirstOrDefaultAsync(r => r.ID == id);

            if (racun == null)
            {
                return NotFound();
            }

            var response = new RacunOdgovorDto
            {
                Id = racun.ID,
                Datum = racun.Datum,
                SkupajBrezDdv = racun.Skupaj_brez_ddv,
                SkupajZDdv = racun.Skupaj_z_ddv,
                Status = racun.Status,
                Artikli = racun.IzdelekiGrejoVn
                    .GroupBy(i => new { i.Artikel_id, i.Artikel.Naziv, i.Artikel.Cena_brez_ddv })
                    .Select(g => new RacuniIzdelkiDto
                    {
                        ArtikelId = g.Key.Artikel_id,
                        Naziv = g.Key.Naziv,
                        CenaBrezDdv = g.Key.Cena_brez_ddv,
                        Kolicina = g.Sum(x => x.Kolicina),
                        Skupaj = g.Key.Cena_brez_ddv * g.Sum(x => x.Kolicina)
                    })
                    .ToList()
            };

            return Ok(response);
        }


        [HttpPost]
        [AllowAnonymous] 
        public async Task<ActionResult<RacunOdgovorDto>> CreateRacun(UstvariRacunZahtevo request)
        {
            if (request.ArtikelIdi == null || !request.ArtikelIdi.Any())
            {
                return BadRequest(new { error = "Račun mora vsebovati vsaj en artikel" });
            }
            if (string.IsNullOrEmpty(request.UserId))
            {
                return BadRequest(new { error = "Uporabnik ni prijavljen" });
            }

            try
            {
                var barSkladisce = await _context.Skladisce.FirstOrDefaultAsync(s => s.Naziv == "Bar");
                if (barSkladisce == null)
                {
                    return BadRequest(new { error = "Bar skladišče ne obstaja" });
                }

                var uporabnik = await _context.Users.FindAsync(request.UserId);
                if (uporabnik == null)
                {
                    return BadRequest(new { error = "Uporabnik ne obstaja" });
                }

                var artikli = await _context.Artikel
                    .Include(a => a.Vrsta)
                    .Where(a => request.ArtikelIdi.Contains(a.ID))
                    .ToListAsync();

                if (!artikli.Any())
                {
                    return BadRequest(new { error = "Artikli ne obstajajo" });
                }

                var artikliGrouped = request.ArtikelIdi
                    .GroupBy(id => id)
                    .Select(g => new
                    {
                        ArtikelId = g.Key,
                        Kolicina = g.Count(),
                        Artikel = artikli.FirstOrDefault(a => a.ID == g.Key)
                    })
                    .Where(x => x.Artikel != null)
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
                    Izdal_uporabnik_id = uporabnik.Id,
                    Status = "Zakljucen"
                };

                _context.Racun.Add(novRacun);
                await _context.SaveChangesAsync();

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

                await _context.SaveChangesAsync();

                var response = new RacunOdgovorDto
                {
                    Id = novRacun.ID,
                    Datum = novRacun.Datum,
                    SkupajBrezDdv = novRacun.Skupaj_brez_ddv,
                    SkupajZDdv = novRacun.Skupaj_z_ddv,
                    Status = novRacun.Status,
                    Artikli = artikliGrouped.Select(item => new RacuniIzdelkiDto
                    {
                        ArtikelId = item.ArtikelId,
                        Naziv = item.Artikel.Naziv,
                        CenaBrezDdv = item.Artikel.Cena_brez_ddv,
                        Kolicina = item.Kolicina,
                        Skupaj = item.Artikel.Cena_brez_ddv * item.Kolicina
                    }).ToList()
                };

                return CreatedAtAction(nameof(GetRacun), new { id = novRacun.ID }, response);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "Napaka pri ustvarjanju računa: " + ex.Message });
            }
}
    }
}