using System.ComponentModel.DataAnnotations.Schema;

namespace PAPData.Entities;

public class AppliedForAdoption
{
    public int Id { get; set; }
    [ForeignKey("Client")]
    public int ClientId { get; set; }
    [ForeignKey("Pet")]
    public int PetId { get; set; }
    public DateTime ApplicationDate { get; set; }
    public AdoptionStatus Status { get; set; }
    
    // relationships
    public Client Client { get; set; }
    public Pet Pet { get; set; }
}