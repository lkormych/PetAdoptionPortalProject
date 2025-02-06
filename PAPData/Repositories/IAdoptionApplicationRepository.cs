namespace PAPData.Entities.Repositories;

public interface   IAdoptionApplicationRepository
{
    Task AddApplication(AppliedForAdoption application);
    Task <AppliedForAdoption?> FindApplicationByPetIdUserId(int petId, int userId);
}