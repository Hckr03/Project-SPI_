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
        if(await createAccount(account))
        {
            return Ok( new { message = $"La cuenta se creo exitosamente!"} );
        }        
        return BadRequest(new { message = $"La cuenta ({account.AccountNum}) ya existe!"});
    }

    [HttpGet]
    public async Task<ICollection<Account>> GetAll()
    {
       return await accountService.GetAll();
    }

    [HttpGet("{accountNum}")]
    public async Task<ActionResult<Account?>> GetByAccount(string accountNum)
    {
        if(await accountService.GetByAccNum(accountNum) is not null)
        {
            return Ok(await accountService.GetByAccNum(accountNum));
        }
        return BadRequest(new { message = $"La cuenta con nro. ({accountNum}) no existe!"});
    }

    [HttpPut("{accountNum}")]
    public async Task<ActionResult<Account>> Update(string accountNum, AccountDtoIn account)
    {
        if(accountNum == account.AccountNum)
        {
            await updateAccount(account);
            return Ok( new {message = "Datos actualizados correctamente!" });
        }
        return BadRequest( new {message = "Los datos proporcionados no son los correctos!" });
    }

    [HttpDelete("accountNum")]
    public async Task<ActionResult<Account>> Delete(string accountNum)
    {
        var accountToDelete = await accountService.GetByAccNum(accountNum);
        if(accountToDelete is not null)
        {
            await accountService.Delete(accountToDelete);
            return NoContent();
        }
        return BadRequest( new { message = $"La cuenta con nro ({accountNum}) no existe o ya ha sido eliminado!"});
    }


    //methods
    private async Task<bool> createAccount(AccountDtoIn account)
    {
        var client = await clientService.GetById(account.ClientDocNum);
        var bank = await bankService.GetByBankCode(account.BankCode);
        var auxAccount = await accountService.GetByAccNum(account.AccountNum);

        if(client is not null && bank is not null && auxAccount is null)
        {
            var newAccount = new Account();
            newAccount.AccountNum = account.AccountNum;
            newAccount.Currency = account.Currency; 
            newAccount.Balance = account.Balance;
            newAccount.Client = client;
            newAccount.Bank = bank;
        
            await accountService.Create(newAccount);
            return true;
        }
        return false;
    }

    private async Task<bool> updateAccount(AccountDtoIn account)
    {
        var accountToUpdate = await accountService.GetByAccNum(account.AccountNum);
        var clientToUpdate = await clientService.GetById(account.ClientDocNum);
        var bankToUpdate = await bankService.GetByBankCode(account.BankCode);

        if(accountToUpdate is not null 
            && clientService is not null
            && clientToUpdate is not null
            && bankToUpdate is not null)
        {
            accountToUpdate.AccountNum = account.AccountNum;
            accountToUpdate.Currency = account.Currency;
            accountToUpdate.Balance = account.Balance;

            await accountService.Update(account.AccountNum, accountToUpdate);
            return true;
        }
        return false;
    }
}