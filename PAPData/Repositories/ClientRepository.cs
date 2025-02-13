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

    public async Task UpdateClient(Client client)
    {
        _context.Clients.Update(client);
        await _context.SaveChangesAsync();
    }

    public async Task<Client?> FindClientById(int clientId)
    {
        return await _context.Clients.FindAsync(clientId);
    }

    public async Task<List<Client>> GetClients()
    {
        return await _context.Clients.ToListAsync();
    }

    public async Task<Client?> GetClientByIdAndApplications(int clientId)
    {
        return await _context.Clients.Include(a => a.AppliedForAdoptions).FirstOrDefaultAsync(c => c.Id == clientId);
    }
}