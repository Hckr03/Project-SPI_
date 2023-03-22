using BankAPI_.Data;
using BankAPI_.Models;
using BankAPI_.Services.Implements;
using Microsoft.EntityFrameworkCore;

namespace BankAPI_.Services;

public class TransferService : IService<Transfer>
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

    public Task Delete(Transfer transfer)
    {
        throw new NotImplementedException();
    }

    public async Task<ICollection<Transfer>> GetAll()
    {
        return await bankDbContext.Transfers
        .Include(a => a.Account)
        .ThenInclude(b => b.Bank)
        .Include(c => c.Client)
        .ToListAsync();
    }

    public Task<Transfer?> GetById(string id)
    {
        throw new NotImplementedException();
    }

    public Task Update(Transfer transfer)
    {
        throw new NotImplementedException();
    }
}