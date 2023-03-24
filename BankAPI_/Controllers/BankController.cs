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
            createBank(bank);
            return Ok( new { message = $"El Banco se creo exitosamente!"} );
        }
    return BadRequest( new { message = $"El codigo ({bank.BankCode}) de banco que introdujo ya existe!"});
    }

    [HttpGet]
    public async Task<ICollection<Bank>> GetAll()
    {
       return await bankService.GetAll();
    }

    [HttpGet("{code}")]
    public async Task<ActionResult<Bank?>> GetByBankCode(string code)
    {
        return await bankService.GetByBankCode(code);
    }

    [HttpPut("{code}")]
    public async Task<ActionResult<Bank>> Update(string code, BankDtoIn bank)
    {
        var bankToUpdate = await bankService.GetByBankCode(code);
        if(code != bank.BankCode)
        {
            return BadRequest( new { message = $"El codigo de Banco ({code}) de la URL no coincide con el codigo de Banco ({bank.BankCode}) del cuerpo solicitado"});
        }
        if(bankToUpdate is not null)
        {
            await bankService.Update(code, bank);
            return Ok( new { message = $"La actulizacion se realizo con exito!"});
        }
        return BadRequest( new { message = $"El Banco no existe!"});
    }

    [HttpDelete("{code}")]
    public async Task<ActionResult<Bank>> Delete(string code)
    {
        var bankToDelete = await bankService.GetByBankCode(code);
        if(bankToDelete is not null)
        {
            await bankService.Delete(bankToDelete);
            return NoContent();
        }
        return BadRequest( new { message = $"El banco no existe!"} );
    }


    //methods
    private async void createBank(BankDtoIn bank)
    {        
        Bank newBank = new Bank();
        newBank.BankCode = bank.BankCode;
        newBank.Name = bank.Name;
        newBank.Address = bank.Address;
        await bankService.Create(newBank);
    }
}