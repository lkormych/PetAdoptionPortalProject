namespace PAPData.Entities;

public class Pet
{
    public int PetId { get; set; }
    public string Name { get; set; }
    public string Breed { get; set; }
    public int Age { get; set; }
    public string Gender { get; set; }
    public decimal AdoptionPrice { get; set; }
    public string IsCastrated { get; set; }
    public string Coat { get; set; }
    public string Size { get; set; }
    public string IsAffectionate { get; set; }
    public string Location  { get; set; }
    public string ActivityLevel { get; set; }
    public string Color { get; set; }
    public string Description {get; set;}
    public PetStatus Status { get; set; }
    
    // relationships
    public ICollection<AppliedForAdoption> AppliedForAdoptions { get; set; }
    public ICollection<Adopted> Adoptions { get; set; }
}