using BankAPI_.Dtos;
using BankAPI_.Models;
using BankAPI_.Services;
using Microsoft.AspNetCore.Mvc;

namespace BankAPI_.Controllers;

[ApiController]
[Route("[controller]")]
public class BankController : ControllerBase
{
    private readonly BankService bankService;
    private readonly AccountService accountService;

    public BankController(BankService bankService, AccountService accountService)
    {
        this.bankService = bankService;
        this.accountService = accountService;
    }

    [HttpPost]
    public async Task<ActionResult<Bank>> Create(BankDtoIn bank)
    {
        var existBank = await bankService.GetByBankCode(bank.BankCode);
        if(existBank is null)
        {
            Bank newBank = new Bank();
            newBank.BankCode = bank.BankCode;
            newBank.Name = bank.Name;
            newBank.Address = bank.Address;

            await bankService.Create(newBank);
            return Ok(newBank);
        }
    return BadRequest( new { message = $"El codigo ({bank.BankCode}) de banco que introdujo no existe"});
    }

    [HttpGet]
    public async Task<ICollection<Bank>> GetAll()
    {
       return await bankService.GetAll();
    }
}