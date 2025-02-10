using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PAPData.Entities;
using PAPServices;
using PetAdoptionPortal.Models;

namespace PetAdoptionPortal.Controllers;
[Authorize(Policy = "Admin")]
public class AdminController : Controller
{
    private readonly AdoptionApplicationService _adoptionApplicationService;

    public AdminController(AdoptionApplicationService adoptionApplicationService)
    {
        _adoptionApplicationService = adoptionApplicationService;
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
        }

        return RedirectToAction(nameof(Index));
    }
}