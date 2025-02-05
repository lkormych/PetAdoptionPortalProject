using PAPData.Entities;
using PAPData.Entities.Repositories;

namespace PAPServices;

public class ClientService
{
    private readonly IClientRepository clientRepository;

    public ClientService(IClientRepository clientRepository)
    {
        this.clientRepository = clientRepository;
    }
    
    // adding Client to the database
    public async Task AddClient(Client client)
    {
        await clientRepository.AddClient(client);
    }

    public async Task<Client?> FindClientByIdentityUser(string identityUserId)
    {
        return await clientRepository.FindClientByIdentityUser(identityUserId);
    }
}