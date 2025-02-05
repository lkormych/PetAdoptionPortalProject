using PAPData.Entities;
using PAPData.Entities.Repositories;

namespace PAPServices;

public class AdoptionApplicationService
{
    private readonly IAdoptionApplicationRepository _adoptionApplicationRepository;

    public AdoptionApplicationService(IAdoptionApplicationRepository adoptionApplicationRepository)
    {
        _adoptionApplicationRepository = adoptionApplicationRepository;
    }

    public async Task AddApplication(AppliedForAdoption application)
    {
        await _adoptionApplicationRepository.AddApplication(application);
    }
}