using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using E_Gostinc.Data;
using E_Gostinc.Models;

namespace E_Gostinc.Controllers
{
    public class VrstaController : Controller
    {
        private readonly ArtikelContext _context;

        public VrstaController(ArtikelContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _context.Vrsta.ToListAsync());
        }
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var vrsta = await _context.Vrsta
                .FirstOrDefaultAsync(m => m.ID == id);
            if (vrsta == null)
            {
                return NotFound();
            }

            return View(vrsta);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,Naziv,Davek")] Vrsta vrsta)
        {
            if (ModelState.IsValid)
            {
                _context.Add(vrsta);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(vrsta);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var vrsta = await _context.Vrsta.FindAsync(id);
            if (vrsta == null)
            {
                return NotFound();
            }
            return View(vrsta);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,Naziv,Davek")] Vrsta vrsta)
        {
            if (id != vrsta.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(vrsta);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!VrstaExists(vrsta.ID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(vrsta);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var vrsta = await _context.Vrsta
                .FirstOrDefaultAsync(m => m.ID == id);
            if (vrsta == null)
            {
                return NotFound();
            }

            return View(vrsta);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var vrsta = await _context.Vrsta.FindAsync(id);
            if (vrsta != null)
            {
                _context.Vrsta.Remove(vrsta);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool VrstaExists(int id)
        {
            return _context.Vrsta.Any(e => e.ID == id);
        }
    }
}
