using Microsoft.AspNetCore.Identity;

namespace PetAdoptionPortal.Services;

public interface IIdentityService
{
    Task CheckRolesExistsAsync();
    Task AssignRoleToUserAsync(IdentityUser user, string roleName);
    
}