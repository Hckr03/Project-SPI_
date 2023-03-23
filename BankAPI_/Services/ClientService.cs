using BankAPI_.Data;
using BankAPI_.Models;
using BankAPI_.Services.Implements;
using Microsoft.EntityFrameworkCore;

namespace BankAPI_.Services;

public class ClientService : IService<Client>
{
    private readonly BankDbContext bankDbContext;

    public ClientService(BankDbContext bankDbContext)
    {
        this.bankDbContext = bankDbContext;
    }
    public async Task<Client> Create(Client Client)
    {    
        await bankDbContext.Clients.AddAsync(Client);
        await bankDbContext.SaveChangesAsync();
        return Client;
    }

    public async Task Delete(string id)
    {
        var clientToDelete = await GetById(id);
        if(clientToDelete is not null)
        {
            bankDbContext.Clients.Remove(clientToDelete);
            await bankDbContext.SaveChangesAsync();
        }
    }

    public async Task<ICollection<Client>> GetAll()
    {
       return await bankDbContext.Clients
            .Include(a => a.Accounts)
            .Include(a => a.Transfers)
            .ToListAsync();
    }

    public async Task<Client?> GetById(string id)
    {
        return await bankDbContext.Clients
        .Where(c => c.ClientDocNum == id)
        .Include(a => a.Accounts)
        .Include(a => a.Transfers)
        .FirstOrDefaultAsync();
    }

    public Task Update(string id, Client client)
    {
        throw new NotImplementedException();
    }
}