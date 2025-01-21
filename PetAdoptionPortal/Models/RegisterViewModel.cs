using System.ComponentModel.DataAnnotations;

namespace PetAdoptionPortal.Models;

public class RegisterViewModel
{
    [Required]
    public string Name { get; set; }
    [Required]
    [Display(Name = "Last name")]
    public string LastName { get; set; }
    [Required]
    [Display(Name = "Phone number")]
    public string PhoneNumber { get; set; }
    [Required]
    [EmailAddress]
    [Display(Name = "E-mail")]
    public string Email { get; set; }
    [Required]
    public string Address { get; set; }
    [Required]
    [StringLength(100, MinimumLength = 6)]
    [DataType(DataType.Password)]
    public string Password { get; set; }

    [DataType(DataType.Password)]
    [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
    [Display(Name = "Confirm password")]
    public string ConfirmPassword { get; set; }
}