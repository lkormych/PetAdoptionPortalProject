namespace PAPData.Entities.Repositories;

public class AdoptionApplicationRepository : IAdoptionApplicationRepository
{
    private readonly PAPContext _context;
    public AdoptionApplicationRepository(PAPContext context)
    {
        _context = context;
    }
    
    public async Task AddApplication(AppliedForAdoption application)
    {
        _context.Applications.Add(application);
        await _context.SaveChangesAsync();
    }
}