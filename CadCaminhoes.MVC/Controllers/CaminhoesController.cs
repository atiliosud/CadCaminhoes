using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CadCaminhoes.MVC.Data;
using CadCaminhoes.MVC.Models;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using CadCaminhoes.MVC.Helpers;
using Microsoft.AspNetCore.Authorization;

namespace CadCaminhoes.MVC.Controllers
{
    public class CaminhoesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CaminhoesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Caminhoes
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Caminhoes.Include(c => c.CreateBy);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Caminhoes/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null || _context.Caminhoes == null)
            {
                return NotFound();
            }

            var caminhao = await _context.Caminhoes
                .Include(c => c.CreateBy)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (caminhao == null)
            {
                return NotFound();
            }

            return View(caminhao);
        }


        [Authorize]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Caminhoes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Modelo,FabricacaoAno,ModeloAno")] Caminhao caminhao)
        {

            if (ModelState.ContainsKey("CreateById"))
            {
                ModelState["CreateById"]!.ValidationState = ModelValidationState.Skipped;
            }
            if (ModelState.IsValid)
            {
                caminhao.CreateById = User.GetUserId();
                caminhao.CreateAt = DateTimeOffset.Now;
                _context.Add(caminhao);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            return View(caminhao);
        }


        [Authorize]
        // GET: Caminhoes/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null || _context.Caminhoes == null)
            {
                return NotFound();
            }

            var caminhao = await _context.Caminhoes.FindAsync(id);
            if (caminhao == null)
            {
                return NotFound();
            }
            return View(caminhao);
        }

        // POST: Caminhoes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("Id,Modelo,FabricacaoAno,ModeloAno")] Caminhao caminhao)
        {
            if (id != caminhao.Id)
            {
                return NotFound();
            }

            ModelState["CreateById"].ValidationState = ModelValidationState.Skipped;
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(caminhao);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CaminhaoExists(caminhao.Id))
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
            return View(caminhao);
        }


        [Authorize]
        // GET: Caminhoes/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null || _context.Caminhoes == null)
            {
                return NotFound();
            }

            var caminhao = await _context.Caminhoes
                .Include(c => c.CreateBy)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (caminhao == null)
            {
                return NotFound();
            }

            return View(caminhao);
        }

        // POST: Caminhoes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            if (_context.Caminhoes == null)
            {
                return Problem("Caminhão não encontrado.");
            }
            var caminhao = await _context.Caminhoes.FindAsync(id);
            if (caminhao != null)
            {
                _context.Caminhoes.Remove(caminhao);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CaminhaoExists(Guid id)
        {
            return _context.Caminhoes.Any(e => e.Id == id);
        }
    }
}
