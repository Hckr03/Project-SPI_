using BankAPI_.Data;
using BankAPI_.Models;
using BankAPI_.Services.Implements;
using Microsoft.EntityFrameworkCore;

namespace BankAPI_.Services;

public class AccountService : IService<Account>
{
    private readonly BankDbContext bankDbContext;

    public AccountService(BankDbContext bankDbContext)
    {
        this.bankDbContext = bankDbContext;
    }

    public async Task<Account> Create(Account newAccount)
    {

        await bankDbContext.Accounts.AddAsync(newAccount);
        await bankDbContext.SaveChangesAsync();
        return newAccount;
    }

    public Task Delete(Account account)
    {
        throw new NotImplementedException();
    }

    public async Task<ICollection<Account>> GetAll()
    {
        return await bankDbContext.Accounts
        .Include(c => c.Client)
        .Include(b => b.Bank)
        .ToListAsync();
    }

    public async Task<Account?> GetById(string id)
    {
        return await bankDbContext.Accounts.FindAsync(id);
    }

    public async Task<Account?> GetByAccNum(string accountNum)
    {
        return await bankDbContext.Accounts
        .Where(a => a.AccountNum == accountNum)
        .FirstOrDefaultAsync();
    }
    public Task Update(Account account)
    {
        throw new NotImplementedException();
    }
    public async Task UpdateBalanceFrom(Account? account, decimal amount)
    {
        if(account is not null)
        {
            account.Balance = Decimal.Subtract(account.Balance, amount);
            await bankDbContext.SaveChangesAsync();
        }
    }

    public async Task UpdateBalanceTo(Account? account, decimal amount)
    {
        if(account is not null)
        {
            account.Balance = Decimal.Add(account.Balance, amount);
            await bankDbContext.SaveChangesAsync();
        }
    }
}