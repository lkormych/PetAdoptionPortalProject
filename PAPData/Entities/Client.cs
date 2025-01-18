namespace PAPData.Entities;

public class Client
{
    public int Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public string PhoneNumber { get; set; }
    public string Address { get; set; }
    
    // relationships
    public ICollection<AppliedForAdoption> AppliedForAdoptions { get; set; }
    public ICollection<Adopted> Adoptions { get; set; }
}