using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using E_Gostinc.Data;
using E_Gostinc.Models;

namespace E_Gostinc.Controllers.Api
{
    [Route("api/v1/skladisce")]
    [ApiController]
    public class SkladisceApiController : ControllerBase
    {
        private readonly ArtikelContext _context;

        public SkladisceApiController(ArtikelContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Skladisce>>> GetSkladisca()
        {
            return await _context.Skladisce.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Skladisce>> GetSkladisce(int id)
        {
            var skladisce = await _context.Skladisce.FindAsync(id);

            if (skladisce == null)
            {
                return NotFound();
            }

            return skladisce;
        }

        [HttpGet("{id}/zaloga")]
        public async Task<ActionResult<object>> GetZaloga(int id)
        {
            var skladisce = await _context.Skladisce.FindAsync(id);
            if (skladisce == null)
            {
                return NotFound();
            }

            var zaloge = await _context.DobavaVSkladisce
                .Where(d => d.Skladisce_id == id)
                .Include(d => d.DobavniArtikel)
                    .ThenInclude(da => da.Vrsta)
                .GroupBy(d => new
                {
                    d.DobavniArtikel.ID,
                    d.DobavniArtikel.Naziv,
                    d.DobavniArtikel.Enota
                })
                .Select(g => new
                {
                    ArtikelId = g.Key.ID,
                    Naziv = g.Key.Naziv,
                    Enota = g.Key.Enota,
                    Zaloga = g.Sum(x => x.Kolicina)
                })
                .ToListAsync();

            return Ok(new
            {
                Skladisce = skladisce.Naziv,
                Zaloge = zaloge
            });
        }

        [HttpPost]
        public async Task<ActionResult<Skladisce>> PostSkladisce(Skladisce skladisce)
        {
            _context.Skladisce.Add(skladisce);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetSkladisce), new { id = skladisce.ID }, skladisce);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutSkladisce(int id, Skladisce skladisce)
        {
            if (id != skladisce.ID)
            {
                return BadRequest();
            }

            _context.Entry(skladisce).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SkladisceExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSkladisce(int id)
        {
            var skladisce = await _context.Skladisce.FindAsync(id);
            if (skladisce == null)
            {
                return NotFound();
            }

            _context.Skladisce.Remove(skladisce);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool SkladisceExists(int id)
        {
            return _context.Skladisce.Any(e => e.ID == id);
        }
    }
}