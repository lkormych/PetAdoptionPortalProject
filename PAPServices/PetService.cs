using PAPData.Entities;
using PAPData.Entities.Repositories;

namespace PAPServices;

public class PetService
{
    private readonly IPetRepository petRepository;

    public PetService(IPetRepository petRepository)
    {
        this.petRepository = petRepository;
    }

    public async Task<List<Pet>> GetAllAvailablePets()
    {
        return await petRepository.GetAllAvailablePets();
    }
  
}