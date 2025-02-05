namespace PAPData.Entities.Repositories;

public interface IClientRepository
{
    Task AddClient(Client client);
    Task<Client?> FindClientByIdentityUser(string identityUserId);
}