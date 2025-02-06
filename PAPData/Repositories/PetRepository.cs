using Microsoft.EntityFrameworkCore;

namespace PAPData.Entities.Repositories;

public class PetRepository : IPetRepository
{
    private readonly PAPContext _context;
    public PetRepository(PAPContext context)
    {
        _context = context;
    }

    public async Task<List<Pet>> GetAllAvailablePets()
    {
        return await _context.Pets.Where(p => p.Status == 0).ToListAsync();
    }

    public async Task<Pet?> GetPetById(int id)
    {
        return await _context.Pets.FindAsync(id);
    }
    public async Task AddPet(Pet pet)
    {
        _context.Pets.Add(pet);
        await _context.SaveChangesAsync();
    }

    public async Task UpdatePet(Pet pet)
    {
        var existingPet = await _context.Pets.FindAsync(pet.PetId);
        _context.Pets.Update(pet);
        await _context.SaveChangesAsync();
    }

    public async Task DeletePet(int id)
    {
        var existingPet = await _context.Pets.FindAsync(id);
        if (existingPet != null)
        {
            _context.Pets.Remove(existingPet);
            await _context.SaveChangesAsync();
        }
    }

    // to filter the List of pets according to available parameters
    public async Task<List<Pet>> ListPetsWithFilterParameters(string? name, string? size, string? location, string? breed)
    {
        var query = _context.Pets.Where(p => p.Status == 0);
        if (!string.IsNullOrEmpty(name))
            query = query.Where(p => p.Name.Contains(name));
        if (!string.IsNullOrEmpty(size))
            query = query.Where(p => p.Size.Contains(size));
        if (!string.IsNullOrEmpty(location))
            query = query.Where(p => p.Location.Contains(location));
        if (!string.IsNullOrEmpty(breed))
            query = query.Where(p => p.Breed.Contains(breed));
         // if all parameters are null, return list of all pets
        return await query.ToListAsync();
    }
}