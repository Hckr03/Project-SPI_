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

    public BankController(BankService bankService)
    {
        this.bankService = bankService;
    }

    [HttpPost]
    public async Task<ActionResult<Bank>> Create(BankDtoIn bank)
    {
        Bank newBank = new Bank();
        newBank.BankCode = bank.BankCode;
        newBank.Name = bank.Name;
        newBank.Address = bank.Address;

        await bankService.Create(newBank);
        return Ok(newBank);
    }

    [HttpGet]
    public async Task<ICollection<Bank>> GetAll()
    {
       return await bankService.GetAll();
    }
}