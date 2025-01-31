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
// list of all pets with Status == 0 (Available)
    public async Task<List<Pet>> GetAllAvailablePets()
    {
        return await petRepository.GetAllAvailablePets();
    }
// get Pet object
    public async Task<Pet?> GetPetById(int id)
    {
        return await petRepository.GetPetById(id);
    }

    public async Task AddPet(Pet pet)
    {
        await petRepository.AddPet(pet);
    }

    public async Task UpdatePet(Pet pet)
    {
        await petRepository.UpdatePet(pet);
    }
  
}