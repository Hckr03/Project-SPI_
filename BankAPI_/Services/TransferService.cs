using BankAPI_.Data;
using BankAPI_.Models;
using BankAPI_.Services.Implements;
using Microsoft.EntityFrameworkCore;

namespace BankAPI_.Services;

public class TransferService
{
    private readonly BankDbContext bankDbContext;

    public TransferService(BankDbContext bankDbContext)
    {
        this.bankDbContext = bankDbContext;
    }

    public async Task<Transfer> Create(Transfer newTransfer)
    {
        await bankDbContext.Transfers.AddAsync(newTransfer);
        await bankDbContext.SaveChangesAsync();
        return newTransfer;
    }

    public Task Delete(string id)
    {
        throw new NotImplementedException();
    }

    public async Task<ICollection<Transfer>> GetAll()
    {
        return await bankDbContext.Transfers
        .Include(a => a.Account)
        .Include(a => a.Client)
        .ToListAsync();
    }

    public Task<Transfer?> GetById(string id)
    {
        throw new NotImplementedException();
    }

    public Task Update(string id, Transfer transfer)
    {
        throw new NotImplementedException();
    }
}