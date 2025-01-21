using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PAPData.Entities;
using PAPServices;
using PetAdoptionPortal.Models;
using PetAdoptionPortal.Services;

namespace PetAdoptionPortal.Controllers;

public class AccountController : Controller
{
    private readonly IIdentityService _identityService;
    private readonly ClientService _clientService;
    public AccountController(IIdentityService identityService, ClientService clientService)
    {
        _identityService = identityService;
        _clientService = clientService;
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

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Register(RegisterViewModel registerModel)
    {
        if (ModelState.IsValid)
        {
            var user = await _identityService.FindUserByEmailAsync(registerModel.Email);
            if (user != null)
            {
                ModelState.AddModelError("Email", "User with this e-mail address already exists.");
            }
            // Creating a new IdentityUser object and adding to the database
            var newUser = new IdentityUser()
            {
                UserName = registerModel.Email,
                Email = registerModel.Email,
                PhoneNumber = registerModel.PhoneNumber
            };
            var result = await _identityService.CreateUserAsync(newUser, registerModel.Password);
            if (result.Succeeded)
            {
                // ADD TO ROLE LOGIC
                // add validation token ?
                
                
                var createdUser = await _identityService.FindUserByEmailAsync(registerModel.Email);
                // create Client object and add to the database
                var newClient = new Client()
                {
                    FirstName = registerModel.Name,
                    LastName = registerModel.LastName,
                    Email = registerModel.Email,
                    PhoneNumber = registerModel.PhoneNumber,
                    Address = registerModel.Address,
                    IdentityUserId = createdUser.Id
                };
                await _clientService.AddClient(newClient);
                return RedirectToAction("Index", "Home");
            }
            // if creation of User identity failed, show Error message
            ModelState.AddModelError(string.Empty, "Invalid login attempt.");
        }
        return View(registerModel);
    }
}