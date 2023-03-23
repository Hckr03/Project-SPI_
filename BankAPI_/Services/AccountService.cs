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

    public async Task Delete(string account)
    {
        var accountToDelete = await GetByAccNum(account);
        if(accountToDelete is not null)
        {
            // bankDbContext.Accounts.Remove(accountToDelete);
            // await bankDbContext.SaveChangesAsync();
            bankDbContext.Clients.RemoveRange(accountToDelete.Client);
            bankDbContext.Banks.RemoveRange(accountToDelete.Bank);
            bankDbContext.Accounts.Remove(accountToDelete);
            await bankDbContext.SaveChangesAsync();
        }
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
        .Include(a => a.Client)
        .Include(a => a.Bank)
        .FirstOrDefaultAsync();
    }
    public async Task Update(string accountNum, Account account)
    {
        var existingClient = await GetByAccNum(account.AccountNum);
        if(existingClient is not null)
        {
            existingClient.AccountNum = account.AccountNum;
            existingClient.Currency = account.Currency;
            existingClient.Balance = account.Balance;
            await bankDbContext.SaveChangesAsync();
        }
    }
    public async Task UpdateBalanceFrom(Account account, decimal amount)
    {
        if(account is not null)
        {
            account.Balance = Decimal.Subtract(account.Balance, amount);
            await bankDbContext.SaveChangesAsync();
        }
    }

    public async Task UpdateBalanceTo(Account account, decimal amount)
    {
        if(account is not null)
        {
            account.Balance = Decimal.Add(account.Balance, amount);
            await bankDbContext.SaveChangesAsync();
        }
    }
}