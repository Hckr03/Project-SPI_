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

    public Task Delete(Client client)
    {
        throw new NotImplementedException();
    }

    public async Task<ICollection<Client>> GetAll()
    {
       return await bankDbContext.Clients.ToListAsync();
    }

    public async Task<Client?> GetById(string id)
    {
        return await bankDbContext.Clients
        .Where(c => c.ClientDocNum == id)
        .FirstOrDefaultAsync();
    }

    public Task Update(Client client)
    {
        throw new NotImplementedException();
    }
}