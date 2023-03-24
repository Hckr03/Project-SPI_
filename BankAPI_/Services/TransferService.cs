using BankAPI_.Data;
using BankAPI_.Dtos;
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

    public async Task<Transfer?> GetById(string id)
    {
        return await bankDbContext.Transfers.FindAsync(id);
    }
    public async Task<ICollection<Transfer>> GetAllByStatus(string status)
    {
        return await bankDbContext.Transfers
        .Where(t => t.Status.ToLower() == status.ToLower())
        .Include(t => t.Account)
        .ThenInclude(a => a.Bank)
        .Include(t => t.Client)
        .ToListAsync();
    }

    public async Task<ICollection<Transfer>> GetAllSent()
    {
        return await bankDbContext.Transfers
        .Where(t => t.Amount < 0)
        .Include(t => t.Account)
        .Include(t => t.Client)
        .ToListAsync();
    }

    public async Task<ICollection<Transfer>> GetAllReceived()
    {
        return await bankDbContext.Transfers
        .Where(t => t.Amount > 0)
        .Include(t => t.Account)
        .Include(t => t.Client)
        .ToListAsync();
    }

    public async Task UpdateStatus(string id, TransferStatusDto transfer)
    {
        var statusToUpdate = await GetById(id);
        if(statusToUpdate is not null)
        {
            statusToUpdate.Status = transfer.Status;
            await bankDbContext.SaveChangesAsync();
        }
    }
}