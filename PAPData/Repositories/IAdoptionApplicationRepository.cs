namespace PAPData.Entities.Repositories;

public interface   IAdoptionApplicationRepository
{
    Task AddApplication(AppliedForAdoption application);
    Task <AppliedForAdoption?> FindApplicationByPetIdUserId(int petId, int userId);
    Task<List<AppliedForAdoption>> GetAllApplications();
    
    Task<List<AppliedForAdoption>> GetFilteredApplications(List<AdoptionStatus> selectedStatuses);
    Task<AppliedForAdoption?> GetApplicationById(int id);
    Task UpdateApplication(AppliedForAdoption application);
}