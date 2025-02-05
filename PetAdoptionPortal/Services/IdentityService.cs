using System.Security.Claims;
using Microsoft.AspNetCore.Identity;

namespace PetAdoptionPortal.Services;

public class IdentityService : IIdentityService
{
    private readonly UserManager<IdentityUser> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly SignInManager<IdentityUser> _signInManager;

    public IdentityService(UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager, SignInManager<IdentityUser> signInManager)
    {
        _userManager = userManager;
        _roleManager = roleManager;
        _signInManager = signInManager;
    }
    
// checking if the role exists, creating new if not
    public async Task CheckRolesExistsAsync()
    {
        var roles = new[] { "Admin", "User" };
        foreach (var role in roles)
        {
            if (!await _roleManager.RoleExistsAsync(role))
            {
                await _roleManager.CreateAsync(new IdentityRole(role));
            }
        }
    }
    
    // assigning role to user
    public async Task AssignRoleToUserAsync(IdentityUser user, string roleName)
    {
        if (!await _userManager.IsInRoleAsync(user, roleName))
        {
            await _userManager.AddToRoleAsync(user, roleName);
        }
    }
    
    //finding user by email 
    public async Task<IdentityUser?> FindUserByEmailAsync(string email)
    {
        return await _userManager.FindByEmailAsync(email);
    }
    
    // attempt signing in the user using their email and password
    // returns status SignInResult
    public async Task<SignInResult> StatusLogIn(IdentityUser user, string password, bool rememberMe,
        bool lockoutOnFailure)
    {
        return await _signInManager.PasswordSignInAsync(user, password, rememberMe, lockoutOnFailure);
    }
    
    // creating Identity user
    public async Task<IdentityResult> CreateUserAsync(IdentityUser user, string password)
    {
        return await _userManager.CreateAsync(user, password);
    }
    
    // sign the user in 
    public async Task SignInAsync(IdentityUser user, bool isPersistent = false) // indicates that login session should not be persistent across browser sessions
    {
        await _signInManager.SignInAsync(user, isPersistent);
    }
    
    // adding user to Role
    public async Task<IdentityResult> AddToRoleAsync(IdentityUser user, string roleName)
    {
        // first checking if the role exists, if not, create a new role
        var roleExists = await _roleManager.RoleExistsAsync(roleName);
        if (!roleExists)
        {
                await _roleManager.CreateAsync(new IdentityRole(roleName));
        }
        return await _userManager.AddToRoleAsync(user, roleName);
    }

    public async Task SignOutAsync()
    {
        await _signInManager.SignOutAsync();
    }

    public int? GetUserId(ClaimsPrincipal user)
    {
        string userIdString =  _userManager.GetUserId(user);
        return (int.TryParse(userIdString, out int userId)) ? userId : null;
    }

    public async Task<IdentityUser?> GetUserAsync(ClaimsPrincipal user)
    {
        return await _userManager.GetUserAsync(user);
    }
}