namespace PAPData.Entities.Repositories;

public interface IPetRepository
{
   Task<List<Pet>> GetAllAvailablePets();
   Task<Pet?> GetPetById(int id);
   Task AddPet(Pet pet);
   
   Task UpdatePet(Pet pet);
   
   Task DeletePet(int id);
   
   Task<List<Pet>> ListPetsWithFilterParameters(string? name, string? size, string? location, string? breed);
}