using System.ComponentModel.DataAnnotations;

namespace PetAdoptionPortal.Models;

public class CreatePetListing
{
    [Required]
    [StringLength(20, ErrorMessage = "Name must be between 1 and 20 characters.", MinimumLength = 1)]
    public string Name { get; set; }
    [Required]
    [StringLength(30, ErrorMessage = "Breed must be between 1 and 30 characters.", MinimumLength = 1)]
    public string Breed { get; set; }
    [Required]
    [Range(0, 30, ErrorMessage = "Age must be between 0 and 30 years.")]
    public int Age { get; set; }
    [Required]
    public string Gender { get; set; } // options: Male, Female
    [Required]
    [Display(Name = "Adoption Price")]
    public decimal AdoptionPrice { get; set; }
    [Required]
    [Display(Name = "Castration")]
    public string IsCastrated { get; set; } // options = yes, No
    [Required]
    public string Coat { get; set; } // options: short, medium, long, curly
    [Required]
    public string Size { get; set; } // options: Small, large, Medium
    [Required]
    [Display(Name = "Affectionate")]
    public string IsAffectionate { get; set; } // options: yes, no
    [Required]
    [StringLength(15, ErrorMessage = "Location must be between 1 and 15 characters.", MinimumLength = 1)]
    public string Location { get; set; }
    [Required]
    public string ActivityLevel { get; set; } // Options: Active, Moderate, Low
    [Required]
    [StringLength(10, ErrorMessage = "Color must be between 3 and 10 characters.", MinimumLength = 3)]
    public string Color  { get; set; }
    [Required]
    [StringLength(1000, ErrorMessage = "Description must be between 50 and 1000 characters.", MinimumLength = 50)]
    public string Description { get; set; }
    [Required]
    public string Status {get; set;} // Options: 0, 1 (Enum)
    [Required]
    public string PictureUrl { get; set; }
}