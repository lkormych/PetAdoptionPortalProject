using PAPData.Entities;
using PAPData.Entities.Repositories;

namespace PAPServices;

public class AdoptionService
{
    private readonly IAdoptionRepository _adoptionRepository;

    public AdoptionService(IAdoptionRepository adoptionRepository)
    {
        _adoptionRepository = adoptionRepository;
    }

    public async Task AddAdoption(Adopted adoption)
    {
        await _adoptionRepository.AddAdoption(adoption);
    }
}