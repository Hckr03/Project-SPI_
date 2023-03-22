using BankAPI_.Data;
using BankAPI_.Models;
using BankAPI_.Services.Implements;
using Microsoft.EntityFrameworkCore;

namespace BankAPI_.Services;

public class BankService : IService<Bank>
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

    public Task Delete(Bank bank)
    {
        throw new NotImplementedException();
    }

    public async Task<ICollection<Bank>> GetAll()
    {
        return await bankDbContext.Banks.ToListAsync();
    }

    public async Task<Bank?> GetById(string id)
    {
        return await bankDbContext.Banks.FindAsync(id);
    }

    public async Task<Bank?> GetByBankCode(string code)
    {
        return await bankDbContext.Banks
        .Where(b => b.BankCode == code)
        .FirstOrDefaultAsync();
    }

    public Task Update(Bank bank)
    {
        throw new NotImplementedException();
    }
}