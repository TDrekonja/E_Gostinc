using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using E_Gostinc.Data;
using E_Gostinc.Models;

namespace E_Gostinc.Controllers.Api
{
    [Route("api/v1/artikel")]
    [ApiController]
    public class ArtikelApiController : ControllerBase
    {
        private readonly ArtikelContext _context;

        public ArtikelApiController(ArtikelContext context)
        {
            _context = context;
        }

        // GET: api/v1/artikel
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Artikel>>> GetArtikli()
        {
            return await _context.Artikel
                .Include(a => a.Vrsta)
                .ToListAsync();
        }

        // GET: api/v1/artikel/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Artikel>> GetArtikel(int id)
        {
            var artikel = await _context.Artikel
                .Include(a => a.Vrsta)
                .FirstOrDefaultAsync(m => m.ID == id);

            if (artikel == null)
            {
                return NotFound();
            }

            return artikel;
        }

        // PUT: api/v1/artikel/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutArtikel(int id, Artikel artikel)
        {
            if (id != artikel.ID)
            {
                return BadRequest();
            }

            _context.Entry(artikel).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ArtikelExists(id))
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

        // POST: api/v1/artikel
        [HttpPost]
        public async Task<ActionResult<Artikel>> PostArtikel(Artikel artikel)
        {
            _context.Artikel.Add(artikel);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetArtikel), new { id = artikel.ID }, artikel);
        }

        // DELETE: api/v1/artikel/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteArtikel(int id)
        {
            var artikel = await _context.Artikel.FindAsync(id);
            if (artikel == null)
            {
                return NotFound();
            }

            _context.Artikel.Remove(artikel);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ArtikelExists(int id)
        {
            return _context.Artikel.Any(e => e.ID == id);
        }
    }
}