namespace PAPData.Entities.Repositories;

public class AdoptionRepository : IAdoptionRepository
{
    private readonly PAPContext _context;

    public AdoptionRepository(PAPContext context)
    {
        _context = context;
    }

    public async Task AddAdoption(Adopted adoption)
    {
        _context.Adoptions.Add(adoption);
        await _context.SaveChangesAsync();
    }
}