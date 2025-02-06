using PAPData.Entities;

namespace PetAdoptionPortal.Models;

public class PetSearchViewModel
{
    public string? PetName { get; set; }
    public string? PetSize {get; set;}
    public string? PetLocation {get; set;}
    
    public string? PetBreed {get; set;}
    public List<Pet>? ListPets { get; set; } = null;
}