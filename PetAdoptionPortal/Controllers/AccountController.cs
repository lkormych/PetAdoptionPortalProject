using Microsoft.AspNetCore.Mvc;
using PetAdoptionPortal.Models;
using PetAdoptionPortal.Services;

namespace PetAdoptionPortal.Controllers;

public class AccountController : Controller
{
    private readonly IIdentityService _identityService;

    public AccountController(IIdentityService identityService)
    {
        _identityService = identityService;
    }
    // GET
    public IActionResult Login()
    {
        // check if user is already authenticated, and if yes Redirect to Index() of HomeController
        if (User.Identity.IsAuthenticated) 
        {
            return RedirectToAction("Index", "Home");
        }
        var loginModel = new LoginViewModel();
        return View(loginModel);
    }
    
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Login(LoginViewModel loginModel)
    {
        if (ModelState.IsValid)
        {
            var user = await _identityService.FindUserByEmailAsync(loginModel.Email);
            if (user != null)
            {
                var result = await _identityService.StatusLogIn(user, loginModel.Password, loginModel.RememberMe, false);
                if (result.Succeeded)
                {
                    return RedirectToAction("Index", "Home");
                }
            }
            // otherwise show error message
            ModelState.AddModelError(string.Empty, "Invalid login attempt.");
        }
        return View(loginModel);
    }

    [HttpGet]
    public IActionResult Register()
    {
       var registerModel = new RegisterViewModel();
        return View(registerModel);
    }
}