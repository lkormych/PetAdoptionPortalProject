namespace PAPData.Entities.Repositories;

public interface IPetRepository
{
   Task<List<Pet>> GetAllAvailablePets();
}