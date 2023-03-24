using BankAPI_.Data;
using BankAPI_.Dtos;
using BankAPI_.Models;
using BankAPI_.Services.Implements;
using Microsoft.EntityFrameworkCore;

namespace BankAPI_.Services;

public class ClientService
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

    public async Task Delete(Client client)
    {
        bankDbContext.Clients.Remove(client);
        await bankDbContext.SaveChangesAsync();
    }

    public async Task<ICollection<Client>> GetAll()
    {
       return await bankDbContext.Clients
        .ToListAsync();
    }

    public async Task<Client?> GetById(string docNum)
    {
        return await bankDbContext.Clients.FindAsync(docNum);
    }

    public async Task Update(string docNum, Client client)
    {
        var clientToUpdate = await GetById(docNum);
        if(clientToUpdate is not null)
        {
            clientToUpdate.ClientDocNum = client.ClientDocNum;
            clientToUpdate.DocType = client.DocType;
            clientToUpdate.Fullname = client.Fullname;

            await bankDbContext.SaveChangesAsync();
        }
    }
}