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
}