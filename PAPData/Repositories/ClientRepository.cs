using Microsoft.EntityFrameworkCore;

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
    public async Task<Client?> FindClientByIdentityUser(string identityUserId)
    {
        return await _context.Clients.FirstOrDefaultAsync(c => c.IdentityUserId == identityUserId);
    }
}