using BankAPI_.Dtos;
using BankAPI_.Models;
using BankAPI_.Services;
using Microsoft.AspNetCore.Mvc;

namespace BankAPI_.Controllers;


[ApiController]
[Route("[controller]")]
public class AccountController : ControllerBase
{
    private readonly AccountService accountService;
    private readonly BankService bankService;
    private readonly ClientService clientService;

    public AccountController(AccountService accountService, BankService bankService, ClientService clientService)
    {
        this.accountService = accountService;
        this.bankService = bankService;
        this.clientService = clientService;
    }

    [HttpPost]
    public async Task<ActionResult<Account>> Create(AccountDtoIn account)
    {
        var newAccount = new Account();
        newAccount.AccountNum = account.AccountNum;
        newAccount.Currency = account.Currency;
        newAccount.Client = await clientService.GetById(account.ClientDocNum);
        newAccount.Balance = account.Balance;
        newAccount.Bank = await bankService.GetByBankCode(account.BankCode);
        
        await accountService.Create(newAccount);
        return Ok(newAccount);
    }

    [HttpGet]
    public async Task<ICollection<Account>> GetAll()
    {
       return await accountService.GetAll();
    }
}