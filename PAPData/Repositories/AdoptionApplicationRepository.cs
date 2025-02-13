using Microsoft.EntityFrameworkCore;

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

    public async Task<AppliedForAdoption?> FindApplicationByPetIdUserId(int petId, int userId)
    {
        return await _context.Applications.Where(a => a.PetId == petId && a.ClientId == userId).FirstOrDefaultAsync();
    }

    public async Task<List<AppliedForAdoption>> GetAllApplications()
    {
        return await _context.Applications.Include(a => a.Pet).Include(a=> a.Client).ToListAsync();
    }

    public async Task<List<AppliedForAdoption>> GetFilteredApplications(List<AdoptionStatus> selectedStatuses)
    {
        return await _context.Applications
            .Include(a => a.Pet)
            .Include(a => a.Client)
            .Where(a => selectedStatuses.Count == 0 || selectedStatuses.Contains(a.Status))
            .ToListAsync();
    }

    public async Task<AppliedForAdoption?> GetApplicationById(int id)
    {
        return await _context.Applications
            .Include(a => a.Pet)
            .Include(a => a.Client)
            .FirstOrDefaultAsync(a => a.Id == id);
    }
    
    public async Task UpdateApplication(AppliedForAdoption application)
    {
        _context.Applications.Update(application);
        await _context.SaveChangesAsync();
    }
}