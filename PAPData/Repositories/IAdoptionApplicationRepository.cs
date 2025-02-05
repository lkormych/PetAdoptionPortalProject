namespace PAPData.Entities.Repositories;

public interface   IAdoptionApplicationRepository
{
    Task AddApplication(AppliedForAdoption application);
}