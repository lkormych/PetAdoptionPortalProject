using System.ComponentModel.DataAnnotations;

namespace PetAdoptionPortal.Models;

public class AdoptionPreviewViewModel
{
    public int PetId { get; set; }
    public string PetName { get; set; }
    public string PetBreed { get; set; }
    public int ClientId { get; set; }
    [Required]
    [Display(Name = "First Name")]
    public string ClientName { get; set; }
    [Required]
    [Display(Name = "Last Name")]
    public string ClientSurname { get; set; }
    [Required]
    [EmailAddress]
    [Display(Name = "Email")]
    public string ClientEmail { get; set; }
    public string DogImage { get; set; }
}