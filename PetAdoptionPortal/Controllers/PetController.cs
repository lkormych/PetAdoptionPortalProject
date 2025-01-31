using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PAPData.Entities;
using PAPServices;
using PetAdoptionPortal.Models;

namespace PetAdoptionPortal.Controllers
{
    public class PetController : Controller
    {
        private readonly PetService _petService;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public PetController(PetService petService, IWebHostEnvironment webHostEnvironment)
        {
            _petService = petService;
            _webHostEnvironment = webHostEnvironment;
        }

        // GET: Pet
        public async Task<IActionResult> Index()
        {
            return View(await _petService.GetAllAvailablePets());
        }

        // GET: Pet/Details/5
        public async Task<IActionResult> Details(int id)
        {
            var pet = await _petService.GetPetById(id);
            return View(pet);
        }

        // GET: Pet/Create
        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            PetListingViewModel petListingViewModelVm = new PetListingViewModel();
            return View(petListingViewModelVm);
        }

        // POST: Pet/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create(PetListingViewModel petListingViewModelVm)
        {
            // custom validation for ProfilePicture
            if (petListingViewModelVm.ProfilePicture == null || petListingViewModelVm.ProfilePicture.Length == 0)
            {
                ModelState.AddModelError("ProfilePicture", "Please upload a profile picture.");
            }
            else
            {
                // allowed file types 
                var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif" };
                var fileExtension = Path.GetExtension(petListingViewModelVm.ProfilePicture.FileName).ToLower();
                if (!allowedExtensions.Contains(fileExtension))
                {
                    ModelState.AddModelError("ProfilePicture", "Only image files (JPG, JPEG, PNG, GIF) are allowed.");
                }

                // validating file size (limit to 5MB)
                var maxFileSize = 5 * 1024 * 1024; // 5MB
                if (petListingViewModelVm.ProfilePicture.Length > maxFileSize)
                {
                    ModelState.AddModelError("ProfilePicture", "The file size must be less than 5MB.");
                }
            }
            if (ModelState.IsValid)
            {
                string uniqueFileName = null;
               // handling file upload and saving to database
                   string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "images");
                   uniqueFileName = Guid.NewGuid().ToString() + "_" + petListingViewModelVm.ProfilePicture.FileName;
                   string filePath = Path.Combine(uploadsFolder, uniqueFileName);

                   using (var fileStream = new FileStream(filePath, FileMode.Create))
                   {
                       await petListingViewModelVm.ProfilePicture.CopyToAsync(fileStream);
                   }
                   
                   petListingViewModelVm.PictureUrl = "/images/" + uniqueFileName;
                   var newListing = new Pet()
                   {
                       Name = petListingViewModelVm.Name,
                       Breed = petListingViewModelVm.Breed,
                       Age = petListingViewModelVm.Age,
                       AdoptionPrice = petListingViewModelVm.AdoptionPrice,
                       Gender = petListingViewModelVm.Gender,
                       IsCastrated = petListingViewModelVm.IsCastrated,
                       Coat = petListingViewModelVm.Coat,
                       Size = petListingViewModelVm.Size,
                       IsAffectionate = petListingViewModelVm.IsAffectionate,
                       Location = petListingViewModelVm.Location,
                       ActivityLevel = petListingViewModelVm.ActivityLevel,
                       Color = petListingViewModelVm.Color,
                       Description = petListingViewModelVm.Description,
                       Status = petListingViewModelVm.Status,
                       PictureUrl = petListingViewModelVm.PictureUrl,
                   };
                   await _petService.AddPet(newListing);
                   return RedirectToAction(nameof(Index));
            }
            // if validation fails, return to the form with errors
            return View(petListingViewModelVm);
        }

        // GET: Pet/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var pet = await _petService.GetPetById(id);
            var petListingModel = new PetListingViewModel()
            {
                Name = pet.Name,
                Breed = pet.Breed,
                Age = pet.Age,
                Gender = pet.Gender,
                AdoptionPrice = pet.AdoptionPrice,
                IsCastrated = pet.IsCastrated,
                Coat = pet.Coat,
                Size = pet.Size,
                IsAffectionate = pet.IsAffectionate,
                Location = pet.Location,
                ActivityLevel = pet.ActivityLevel,
                Color = pet.Color,
                Description = pet.Description,
                Status = pet.Status,
                PictureUrl = pet.PictureUrl,
            };
            return View(petListingModel);
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
