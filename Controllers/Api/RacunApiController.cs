using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using E_Gostinc.Data;
using E_Gostinc.Models;

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

        // GET: api/v1/racun
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Racun>>> GetRacuni()
        {
            return await _context.Racun
                .Include(r => r.Uporabnik)
                .Include(r => r.IzdelekiGrejoVn)
                    .ThenInclude(i => i.Artikel)
                .OrderByDescending(r => r.Datum)
                .ToListAsync();
        }

        // GET: api/v1/racun/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Racun>> GetRacun(int id)
        {
            var racun = await _context.Racun
                .Include(r => r.Uporabnik)
                .Include(r => r.IzdelekiGrejoVn)
                    .ThenInclude(i => i.Artikel)
                        .ThenInclude(a => a.Vrsta)
                .FirstOrDefaultAsync(r => r.ID == id);

            if (racun == null)
            {
                return NotFound();
            }

            return racun;
        }

        // GET: api/v1/racun/dnevno
        [HttpGet("dnevno")]
        public async Task<ActionResult<object>> GetDnevnoPorocilo([FromQuery] DateTime? datum)
        {
            var izbraniDatum = datum ?? DateTime.Today;

            var racuni = await _context.Racun
                .Where(r => r.Datum.Date == izbraniDatum.Date && r.Status == "Zakljucen")
                .Include(r => r.IzdelekiGrejoVn)
                    .ThenInclude(i => i.Artikel)
                .ToListAsync();

            var statistika = new
            {
                Datum = izbraniDatum,
                SkupajRacunov = racuni.Count,
                SkupajVrednost = racuni.Sum(r => r.Skupaj_z_ddv),
                PovprecnaVrednost = racuni.Any() ? racuni.Average(r => r.Skupaj_z_ddv) : 0
            };

            return Ok(statistika);
        }

        // POST: api/v1/racun
        [HttpPost]
        public async Task<ActionResult<Racun>> PostRacun(Racun racun)
        {
            _context.Racun.Add(racun);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetRacun), new { id = racun.ID }, racun);
        }

        // DELETE: api/v1/racun/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRacun(int id)
        {
            var racun = await _context.Racun.FindAsync(id);
            if (racun == null)
            {
                return NotFound();
            }

            _context.Racun.Remove(racun);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool RacunExists(int id)
        {
            return _context.Racun.Any(e => e.ID == id);
        }
    }
}