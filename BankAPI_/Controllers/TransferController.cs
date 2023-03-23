using BankAPI_.Dtos;
using BankAPI_.Enums;
using BankAPI_.Models;
using BankAPI_.Services;
using Microsoft.AspNetCore.Mvc;

namespace BankAPI_.Controllers;

[ApiController]
[Route("[controller]")]
public class TransferController : ControllerBase
{
    private readonly TransferService transferService;
    private readonly AccountService accountService;
    private readonly ClientService clientService;

    public TransferController(TransferService transferService,
     AccountService accountService,
     ClientService clientService)
    {
        this.transferService = transferService;
        this.accountService = accountService;
        this.clientService = clientService;
    }

    [HttpPost]
    public async Task<ActionResult<Transfer>> Create(TransferDtoIn transfer)
    {
        var fromAccount = await accountService.GetByAccNum(transfer.FromAccountNum);
        var fromClient = await clientService.GetById(transfer.FromClientDocNumber);

        if(fromAccount is not null && fromClient is not null)
        {
            Transfer transferSent = new Transfer();
            transferSent.Account = fromAccount;
            transferSent.Client = fromClient;
            transferSent.Date = DateTime.Now.ToUniversalTime();
            transferSent.Amount = Decimal.Negate(transfer.Amount);
            transferSent.Status = transfer.Status;
            await transferService.Create(transferSent);
            await accountService.UpdateBalanceFrom(fromAccount, transfer.Amount);
        }
        else
        {
            return BadRequest();
        }

        var toAccount = await accountService.GetByAccNum(transfer.ToAccountNum);
        var toClient = await clientService.GetById(transfer.ToClientDocNumber);

        if(toAccount is not null && toClient is not null)
        {
            Transfer transferRecieved = new Transfer();
            transferRecieved.Account = toAccount;
            transferRecieved.Client = toClient;
            transferRecieved.Date = DateTime.Now.ToUniversalTime();
            transferRecieved.Amount = transfer.Amount;
            transferRecieved.Status = transfer.Status;
            await transferService.Create(transferRecieved);
            await accountService.UpdateBalanceTo(toAccount, transfer.Amount);
        }
        else
        {
            return BadRequest();
        }

        return Ok();
    }

    [HttpGet]
    public async Task<ICollection<Transfer>> GetAll()
    {
        return await transferService.GetAll();
    }
}