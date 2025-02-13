namespace PAPData.Entities.Repositories;

public interface IAdoptionRepository
{
    Task AddAdoption(Adopted adoption);
}