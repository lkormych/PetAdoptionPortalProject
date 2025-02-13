using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PAPData.Entities;
using PAPData.Entities.Repositories;
using PAPServices;
using PetAdoptionPortal.Models;

namespace PetAdoptionPortal.Controllers;
[Authorize(Policy = "Admin")]
public class AdminController : Controller
{
    private readonly PetService _petService;
    private readonly ClientService _clientService;
    private readonly AdoptionApplicationService _adoptionApplicationService;
    private readonly IAdoptionRepository _adoptionRepository;

    public AdminController(PetService petService, ClientService clientService, AdoptionApplicationService adoptionApplicationService, IAdoptionRepository adoptionRepository)
    {
        _petService = petService;
        _clientService = clientService;
        _adoptionApplicationService = adoptionApplicationService;
        _adoptionRepository = adoptionRepository;
    }
    // GET
    public async Task<IActionResult> Index()
    {
        var viewModel = new ApplicationFilterViewModel
        {
            Applications = await _adoptionApplicationService.GetAllApplications(),
            ApplicationStatuses = Enum.GetValues(typeof(AdoptionStatus)).Cast<AdoptionStatus>().ToList()
        };
        return View(viewModel);
    }

    [HttpPost]
    public async Task<IActionResult> FilterApplications(ApplicationFilterViewModel viewModel)
    {
        var selectedStatuses = viewModel.SelectedAdoptionStatuses ?? new List<AdoptionStatus>();
        var filteredApplications = await _adoptionApplicationService.GetFilteredApplications(selectedStatuses);
        viewModel.Applications = filteredApplications;
        viewModel.ApplicationStatuses = Enum.GetValues(typeof(AdoptionStatus)).Cast<AdoptionStatus>().ToList();
        return View(nameof(Index), viewModel);
    }

    [HttpPost]
    public async Task<IActionResult> ApproveApplication(int id)
    {
        var application = await _adoptionApplicationService.GetApplicationById(id);
        if (application == null)
        {
            return NotFound();
        }
        if (application.Status == AdoptionStatus.Pending)
        {
            application.Status = AdoptionStatus.Approved;
            await _adoptionApplicationService.UpdateApplication(application);
            var newAdoption = new Adopted()
            {
                PetId = application.PetId,
                ClientId = application.ClientId,
                AdoptionDate = DateTime.Now
            };
            await _adoptionRepository.AddAdoption(newAdoption);
            var pet = await _petService.GetPetById(application.PetId);
            if (pet != null)
            {
                pet.Status = PetStatus.Adopted;
                await _petService.UpdatePet(pet);
            }
        }
        return RedirectToAction(nameof(Index));
    }
    
    [HttpPost]
    public async Task<IActionResult> RejectApplication(int id)
    {
        var application = await _adoptionApplicationService.GetApplicationById(id);
        if (application == null)
        {
            return NotFound();
        }

        if (application.Status == AdoptionStatus.Pending)
        {
            application.Status = AdoptionStatus.Rejected;
            await _adoptionApplicationService.UpdateApplication(application);
        }

        return RedirectToAction(nameof(Index));
    }

    [HttpGet]
    public async Task<IActionResult> Details(int id)
    {
        var application = await _adoptionApplicationService.GetApplicationById(id);
        if (application == null)
            return NotFound();
        return View(application);
    }

    [HttpGet]
    public async Task<IActionResult> Clients()
    {
        var clients = await _clientService.GetClients();
        return View(clients);
    }
}