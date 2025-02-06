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

    public async Task<bool> UserAlreadyAppliedForAdoption(int petId, int userId)
    {
        var existingApplication = await _adoptionApplicationRepository.FindApplicationByPetIdUserId(userId, petId);
        return existingApplication != null;
    }
}