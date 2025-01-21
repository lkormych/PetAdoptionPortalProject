using Microsoft.AspNetCore.Identity;
using PetAdoptionPortal.Models;

namespace PetAdoptionPortal.Services;

public interface IIdentityService
{
    Task CheckRolesExistsAsync();
    Task AssignRoleToUserAsync(IdentityUser user, string roleName);
    Task<IdentityUser?>FindUserByEmailAsync(string email);
    // attempt signing in the user using their email and password
    Task<SignInResult> StatusLogIn(IdentityUser user, string password, bool rememberMe, bool lockoutOnFailure);
}