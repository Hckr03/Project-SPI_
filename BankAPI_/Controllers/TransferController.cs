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
        Transfer transferSent = new Transfer();
        transferSent.Account = await accountService.GetByAccNum(transfer.FromAccountNum);
        transferSent.Client = await clientService.GetById(transfer.FromClientDocNumber);
        transferSent.Date = DateTime.Now.ToUniversalTime();
        transferSent.Amount = transfer.Amount;
        transferSent.Status = transfer.Status;
        await transferService.Create(transferSent);
        await accountService.UpdateBalanceFrom((await accountService.GetByAccNum(transfer.FromAccountNum)), transfer.Amount);


        Transfer transferRecieved = new Transfer();
        transferRecieved.Account = await accountService.GetByAccNum(transfer.ToAccountNum);
        transferRecieved.Client = await clientService.GetById(transfer.ToClientDocNumber);
        transferRecieved.Date = DateTime.Now.ToUniversalTime();
        transferRecieved.Amount = transfer.Amount;
        transferRecieved.Status = transfer.Status;
        await transferService.Create(transferRecieved);
        await accountService.UpdateBalanceTo((await accountService.GetByAccNum(transfer.ToAccountNum)), transfer.Amount);

        return Ok();
    }

    [HttpGet]
    public async Task<ICollection<Transfer>> GetAll()
    {
        return await transferService.GetAll();
    }
}