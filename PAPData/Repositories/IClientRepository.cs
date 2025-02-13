namespace PAPData.Entities.Repositories;

public interface IClientRepository
{
    Task AddClient(Client client);
    Task<Client?> FindClientByIdentityUser(string identityUserId);
    
    Task UpdateClient(Client client);
    Task<Client?> FindClientById(int clientId);
    Task<List<Client>> GetClients();
    Task<Client?> GetClientByIdAndApplications(int clientId);
}