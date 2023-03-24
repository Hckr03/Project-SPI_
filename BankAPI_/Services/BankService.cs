using BankAPI_.Data;
using BankAPI_.Dtos;
using BankAPI_.Models;
using Microsoft.EntityFrameworkCore;

namespace BankAPI_.Services;

public class BankService
{
    private readonly BankDbContext bankDbContext;

    public BankService(BankDbContext bankDbContext)
    {
        this.bankDbContext = bankDbContext;
    }

    public async Task<Bank> Create(Bank newBank)
    {
        await bankDbContext.Banks.AddAsync(newBank);
        await bankDbContext.SaveChangesAsync();
        return newBank;
    }

    public async Task Delete(Bank bank)
    {
        bankDbContext.Banks.Remove(bank);
        await bankDbContext.SaveChangesAsync();
    }

    public async Task<ICollection<Bank>> GetAll()
    {
        return await bankDbContext.Banks
        .ToListAsync();
    }

    public async Task<Bank?> GetById(string id)
    {
        return await bankDbContext.Banks.FindAsync(id);
    }

    public async Task<Bank?> GetByBankCode(string code)
    {
        return await bankDbContext.Banks
        .Where(b => b.BankCode == code)
        .Include(b => b.Accounts)
        .FirstOrDefaultAsync();
    }

    public async Task Update(string code, BankDtoIn bank)
    {
        var bankToUpdate = await GetByBankCode(code);
        if(bankToUpdate is not null)
        {
            bankToUpdate.BankCode = bank.BankCode;
            bankToUpdate.Name = bank.Name;
            bankToUpdate.Address = bank.Address;
            
            await bankDbContext.SaveChangesAsync();
        }

    }
}