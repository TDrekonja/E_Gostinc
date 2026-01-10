using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using E_Gostinc.Data;
using E_Gostinc.Models;
using E_Gostinc.Models.DTOs;

namespace E_Gostinc.Controllers.Api
{
    [Route("api/v1/artikel")]
    [ApiController]
    [AllowAnonymous]
    public class ArtikelApiController : ControllerBase
    {
        private readonly ArtikelContext _context;

        public ArtikelApiController(ArtikelContext context)
        {
            _context = context;
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult<IEnumerable<ArtikelDto>>> GetArtikli()
        {
            var artikli = await _context.Artikel
                .Include(a => a.Vrsta)
                .Select(a => new ArtikelDto
                {
                    Id = a.ID,
                    Naziv = a.Naziv,
                    Cena_brez_ddv = a.Cena_brez_ddv,
                    VrstaID = a.VrstaID,
                    VrstaNaziv = a.Vrsta.Naziv,
                    Davek = a.Vrsta.Davek
                })
                .ToListAsync();

            return Ok(artikli);
        }

        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<ActionResult<ArtikelDto>> GetArtikel(int id)
        {
            var artikel = await _context.Artikel
                .Include(a => a.Vrsta)
                .Where(a => a.ID == id)
                .Select(a => new ArtikelDto
                {
                    Id = a.ID,
                    Naziv = a.Naziv,
                    Cena_brez_ddv = a.Cena_brez_ddv,
                    VrstaID = a.VrstaID,
                    VrstaNaziv = a.Vrsta.Naziv,
                    Davek = a.Vrsta.Davek
                })
                .FirstOrDefaultAsync();

            if (artikel == null)
            {
                return NotFound();
            }

            return artikel;
        }

        [HttpPost]
        public async Task<ActionResult<Artikel>> PostArtikel(Artikel artikel)
        {
            _context.Artikel.Add(artikel);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetArtikel), new { id = artikel.ID }, artikel);
        }

    }
}