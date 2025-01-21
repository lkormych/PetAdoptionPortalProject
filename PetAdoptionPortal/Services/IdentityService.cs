using Microsoft.AspNetCore.Identity;

namespace PetAdoptionPortal.Services;

public class IdentityService : IIdentityService
{
    private readonly UserManager<IdentityUser> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;

    public IdentityService(UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager)
    {
        _userManager = userManager;
        _roleManager = roleManager;
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
}