using System.ComponentModel.DataAnnotations.Schema;

namespace PAPData.Entities;

public class Adopted
{
    public int Id { get; set; }
    [ForeignKey("Pet")]
    public int PetId { get; set; }
    [ForeignKey("Client")]
    public int ClientId { get; set; }
    public DateTime AdoptionDate { get; set; }
    
    //relationships
    public Client Client { get; set; }
    public Pet Pet { get; set; }
}