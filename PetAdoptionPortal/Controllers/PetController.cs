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
using PetAdoptionPortal.Services;

namespace PetAdoptionPortal.Controllers;
[Authorize]
    public class PetController : Controller
    {
        private readonly PetService _petService;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IIdentityService _identityService;
        private readonly ClientService _clientService;
        private readonly AdoptionApplicationService _adoptionApplicationService;
        public PetController(PetService petService, IWebHostEnvironment webHostEnvironment, IIdentityService identityService, ClientService clientService, AdoptionApplicationService adoptionApplicationService)
        {
            _petService = petService;
            _webHostEnvironment = webHostEnvironment;
            _identityService = identityService;
            _clientService = clientService;
            _adoptionApplicationService = adoptionApplicationService;
        }

        // GET: Pet
        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var petSearchVM = new PetSearchViewModel();
            petSearchVM.ListPets = await _petService.GetAllAvailablePets();
            return View(petSearchVM);
        }
        
        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> Search(PetSearchViewModel petSearchVM)
        {
            var pets = await _petService.ListPetsWithFilterParameters(petSearchVM.PetName, petSearchVM.PetSize, petSearchVM.PetLocation, petSearchVM.PetBreed);
            petSearchVM.ListPets = pets; 
            return View(nameof(Index), petSearchVM);
        }
        
        // GET: Pet/Details/5
        [AllowAnonymous]
        public async Task<IActionResult> Details(int id)
        {
            var pet = await _petService.GetPetById(id);
            if (pet == null)
                return NotFound();
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
            if (petListingViewModelVm.ProfilePicture != null && petListingViewModelVm.ProfilePicture.Length > 0)
            {
                CustomValidationProfilePicture(petListingViewModelVm.ProfilePicture);
            }
            if (!ModelState.IsValid) 
            {
                return View(petListingViewModelVm);  // if validation fails, return to the form with errors
            }
            petListingViewModelVm.PictureUrl = await SaveProfilePictureAsync(petListingViewModelVm.ProfilePicture);
            var newListing = MapToNewPetInstance(petListingViewModelVm);
            await _petService.AddPet(newListing);
            return RedirectToAction(nameof(Index));
        }

        // GET: Pet/Edit/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int id)
        {
            var pet = await _petService.GetPetById(id);
            if (pet == null)
                return NotFound(); 
            var petViewModel = MapToPetViewModel(pet);
            // petViewModel.ProfilePicture = here the logic to pass the file if its possible
            return View(petViewModel);
        }

        // POST: Pet/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(PetListingViewModel petListingViewModelVm)
        {
            if (petListingViewModelVm.ProfilePicture != null && petListingViewModelVm.ProfilePicture.Length > 0)
            {
                CustomValidationProfilePicture(petListingViewModelVm.ProfilePicture);
            }
            if (!ModelState.IsValid) 
            {
                return View(petListingViewModelVm);  // if validation fails, return to the form with errors
            }
            var existingPet = await _petService.GetPetById(petListingViewModelVm.PetId);
            if(existingPet == null)
                    return NotFound();
            MapToExistingPet(existingPet, petListingViewModelVm);
            if (petListingViewModelVm.ProfilePicture != null && petListingViewModelVm.ProfilePicture.Length > 0)
            {
                    existingPet.PictureUrl = await SaveProfilePictureAsync(petListingViewModelVm.ProfilePicture);
            }
            await _petService.UpdatePet(existingPet);
            return RedirectToAction(nameof(Index));
        }

        // GET: Pet/Delete/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            var pet = await _petService.GetPetById(id);
            if (pet == null)
                return NotFound();
            return View(pet);
        }

        // POST: Pet/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var pet = await _petService.GetPetById(id);
            if (pet != null)
                await _petService.DeletePet(id);
            return RedirectToAction(nameof(Index));
        }
        
        [Authorize(Roles = "Admin, User")]
        public async Task<IActionResult> ApplyForAdoption(int id)
        {
            // if user is not authorized, redirect to Login page
            var user =  await _identityService.GetUserAsync(User);
            if (user == null)
                return RedirectToAction("Login", "Account");
            
            var pet = await _petService.GetPetById(id);
            if (pet == null)
                return NotFound();
            var client = await _clientService.FindClientByIdentityUser(user.Id); // find Client by string IdentityUserId
            if (client == null)
                return RedirectToAction("Login", "Account");

            var adoptionVM = new AdoptionPreviewViewModel()
            {
                PetId = pet.PetId,
                PetBreed = pet.Breed,
                PetName = pet.Name,
                ClientId = client.Id,
                ClientName = client.FirstName,
                ClientSurname = client.LastName,
                ClientEmail = client.Email,
                DogImage = pet.PictureUrl,
            };
            // if user has already applied for adoption of the dog, he can no longer apply for the same dog
            var applicationExists = await _adoptionApplicationService.UserAlreadyAppliedForAdoption(adoptionVM.ClientId, adoptionVM.PetId);
            ViewBag.ApplicationExists = applicationExists ? "You have already applied for adoption of me :)" : null;
            return View(adoptionVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "User")]
        public async Task<IActionResult> ConfirmAdoption(AdoptionPreviewViewModel adoptionVM)
        {
            if (!ModelState.IsValid)
                return View(nameof(ApplyForAdoption), adoptionVM);
            // update data of the client in database
            var client = await _clientService.FindClientById(adoptionVM.ClientId);
            if (client != null)
                await _clientService.UpdateClient(client);
            var application = new AppliedForAdoption()
            {
                    PetId = adoptionVM.PetId,
                    ClientId = adoptionVM.ClientId,
                    ApplicationDate = DateTime.Now,
                    Status = AdoptionStatus.Pending,
             };
            await _adoptionApplicationService.AddApplication(application);
                // here also TempData
            return RedirectToAction(nameof(Details), new { id = adoptionVM.PetId });
        }

        
        private void CustomValidationProfilePicture(IFormFile profilePicture)
        {
                // allowed file types 
                var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif" };
                var fileExtension = Path.GetExtension(profilePicture.FileName).ToLower();
                if (!allowedExtensions.Contains(fileExtension))
                {
                    ModelState.AddModelError("ProfilePicture", "Only image files (JPG, JPEG, PNG, GIF) are allowed.");
                    return;
                }

                // validating file size (limit to 5MB)
                var maxFileSize = 5 * 1024 * 1024; // 5MB
                if (profilePicture.Length > maxFileSize)
                {
                    ModelState.AddModelError("ProfilePicture", "The file size must be less than 5MB.");
                }
        }

        private async Task<string> SaveProfilePictureAsync(IFormFile profilePicture)
        {
            // handling file upload and saving to database
            string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "images");
            string uniqueFileName = Guid.NewGuid().ToString() + "_" + profilePicture.FileName;
            string filePath = Path.Combine(uploadsFolder, uniqueFileName);

            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                await profilePicture.CopyToAsync(fileStream);
            }
            return "/images/" + uniqueFileName;
        }

        private Pet MapToNewPetInstance(PetListingViewModel petListingViewModel)
        {
            return new Pet
            {
                Name = petListingViewModel.Name,
                Breed = petListingViewModel.Breed,
                Age = petListingViewModel.Age,
                AdoptionPrice = petListingViewModel.AdoptionPrice,
                Gender = petListingViewModel.Gender,
                IsCastrated = petListingViewModel.IsCastrated,
                Coat = petListingViewModel.Coat,
                Size = petListingViewModel.Size,
                IsAffectionate = petListingViewModel.IsAffectionate,
                Location = petListingViewModel.Location,
                ActivityLevel = petListingViewModel.ActivityLevel,
                Color = petListingViewModel.Color,
                Description = petListingViewModel.Description,
                Status = petListingViewModel.Status,
                PictureUrl = petListingViewModel.PictureUrl,
            };
        }
        
        private void MapToExistingPet(Pet existingPet, PetListingViewModel petListingViewModel)
        {
            existingPet.Name = petListingViewModel.Name;
            existingPet.Breed = petListingViewModel.Breed;
            existingPet.Age = petListingViewModel.Age;
            existingPet.AdoptionPrice = petListingViewModel.AdoptionPrice;
            existingPet.Gender = petListingViewModel.Gender;
            existingPet.IsCastrated = petListingViewModel.IsCastrated;
            existingPet.Coat = petListingViewModel.Coat;
            existingPet.Size = petListingViewModel.Size;
            existingPet.IsAffectionate = petListingViewModel.IsAffectionate;
            existingPet.Location = petListingViewModel.Location;
            existingPet.ActivityLevel = petListingViewModel.ActivityLevel;
            existingPet.Color = petListingViewModel.Color;
            existingPet.Description = petListingViewModel.Description;
            existingPet.Status = petListingViewModel.Status;
        }

        private PetListingViewModel MapToPetViewModel(Pet pet)
        {
            return new PetListingViewModel
            {
                PetId = pet.PetId,
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
        }
}
    

