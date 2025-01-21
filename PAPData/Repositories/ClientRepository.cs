namespace PAPData.Entities.Repositories;

public class ClientRepository : IClientRepository
{
    private readonly PAPContext _context;

    public ClientRepository(PAPContext context)
    {
        _context = context;
    }

    public async Task AddClient(Client client)
    {
        _context.Clients.Add(client);
        await _context.SaveChangesAsync();
    }
}