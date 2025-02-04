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
}