namespace PAPData.Entities.Repositories;

public interface IPetRepository
{
   Task<List<Pet>> GetAllAvailablePets();
   Task<Pet?> GetPetById(int id);
}