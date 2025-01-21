using Microsoft.AspNetCore.Mvc;
using PetAdoptionPortal.Services;

namespace PetAdoptionPortal.Controllers;

public class IdentityController : Controller
{
    private readonly IIdentityService _identityService;

    public IdentityController(IIdentityService identityService)
    {
        _identityService = identityService;
    }
    // GET
    public IActionResult Index()
    {
        return View();
    }
}