using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PAPData.Entities;
using PAPServices;

namespace PetAdoptionPortal.Controllers
{
    public class PetController : Controller
    {
       
        private readonly PetService _petService;

        public PetController(PetService petService)
        {
            _petService = petService;
        }

        // GET: Pet
        public async Task<IActionResult> Index()
        {
            return View(await _petService.GetAllAvailablePets());
        }

        // GET: Pet/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            // if (id == null)
            // {
            //     return NotFound();
            // }
            //
            // var pet = await _context.Pets
            //     .FirstOrDefaultAsync(m => m.PetId == id);
            // if (pet == null)
            // {
            //     return NotFound();
            // }

            return View();
        }

        // GET: Pet/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Pet/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("PetId,Name,Breed,Age,Gender,AdoptionPrice,IsCastrated,Coat,Size,IsAffectionate,Location,ActivityLevel,Color,Description,Status,PictureUrl")] Pet pet)
        {
            // if (ModelState.IsValid)
            // {
            //     _context.Add(pet);
            //     await _context.SaveChangesAsync();
            //     return RedirectToAction(nameof(Index));
            // }
            return View();
        }

        // GET: Pet/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            // if (id == null)
            // {
            //     return NotFound();
            // }
            //
            // var pet = await _context.Pets.FindAsync(id);
            // if (pet == null)
            // {
            //     return NotFound();
            // }
            return View();
        }

        // POST: Pet/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("PetId,Name,Breed,Age,Gender,AdoptionPrice,IsCastrated,Coat,Size,IsAffectionate,Location,ActivityLevel,Color,Description,Status,PictureUrl")] Pet pet)
        {
            // if (id != pet.PetId)
            // {
            //     return NotFound();
            // }
            //
            // if (ModelState.IsValid)
            // {
            //     try
            //     {
            //         _context.Update(pet);
            //         await _context.SaveChangesAsync();
            //     }
            //     catch (DbUpdateConcurrencyException)
            //     {
            //         if (!PetExists(pet.PetId))
            //         {
            //             return NotFound();
            //         }
            //         else
            //         {
            //             throw;
            //         }
            //     }
            //     return RedirectToAction(nameof(Index));
            // }
            return View();
        }

        // GET: Pet/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            // if (id == null)
            // {
            //     return NotFound();
            // }
            //
            // var pet = await _context.Pets
            //     .FirstOrDefaultAsync(m => m.PetId == id);
            // if (pet == null)
            // {
            //     return NotFound();
            // }

            return View();
        }

        // POST: Pet/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            // var pet = await _context.Pets.FindAsync(id);
            // if (pet != null)
            // {
            //     _context.Pets.Remove(pet);
            // }
            //
            // await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // private bool PetExists(int id)
        // {
        //     return _context.Pets.Any(e => e.PetId == id);
        // }
    }
}
