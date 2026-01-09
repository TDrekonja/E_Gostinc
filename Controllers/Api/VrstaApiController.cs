using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using E_Gostinc.Data;
using E_Gostinc.Models.DTOs;

namespace E_Gostinc.Controllers.Api
{
    [Route("api/v1/vrsta")]
    [ApiController]
    [AllowAnonymous]
    public class VrstaApiController : ControllerBase
    {
        private readonly ArtikelContext _context;

        public VrstaApiController(ArtikelContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<VrstaDto>>> GetVrste()
        {
            var vrste = await _context.Vrsta
                .Select(v => new VrstaDto
                {
                    Id = v.ID,
                    Naziv = v.Naziv,
                    Davek = v.Davek
                })
                .ToListAsync();

            return Ok(vrste);
        }

        [HttpGet("{id}/artikli")]
        public async Task<ActionResult<IEnumerable<ArtikelDto>>> GetArtikliByVrsta(int id)
        {
            var artikli = await _context.Artikel
                .Include(a => a.Vrsta)
                .Where(a => a.VrstaID == id)
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
    }
}