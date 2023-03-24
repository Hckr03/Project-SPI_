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
        if(await existsAccounts(transfer))
        {
            return BadRequest( new { message = $"La cuenta ({transfer.FromAccountNum}) o ({transfer.ToAccountNum}) no existe!"});
        }

        if(transferNullOrEmpty(transfer)){
            return BadRequest( new { message = $"La cuenta ({transfer.FromAccountNum}) no posee fondos suficiente para realizar esta operacion!"});
        }

        if(await equalBanks(transfer))
        {
             return BadRequest( new { message = $"Para realizar esta operacion, los bancos deben ser distintos!"});
        }

        makeTransfer(transfer);
        return Ok();
    }

    [HttpGet]
    public async Task<ICollection<Transfer>> GetAll()
    {
        return await transferService.GetAll();
    }

    [HttpGet("{status}")]
    public async Task<ICollection<Transfer>> GetAllByStatus(string status)
    {
        return await transferService.GetAllByStatus(status);
    }
    
    [HttpGet("sent")]
    public async Task<ICollection<Transfer>> GetAllSent()
    {
        return await transferService.GetAllSent();
    }

    [HttpGet("received")]
    public async Task<ICollection<Transfer>> GetAllReceived()
    {
        return await transferService.GetAllSent();
    }

    [HttpPut("status/{id}")]
    public async Task<ActionResult<Transfer>> UpdateStatus(string id, TransferStatusDto transfer)
    {
        var statusToUpdate = await transferService.GetById(id);
        if(statusToUpdate is not null)
        {
            await transferService.UpdateStatus(id, transfer);
            return Ok( new { message = $"Se actualizo el estado correctamente!"});
        }
        return BadRequest( new { message = $"La transferencia con ID = ({id}) no existe!"});
    }

    private async Task<bool> existsAccounts(TransferDtoIn transfer)
    {
        var fromAccount = await accountService.GetByAccNum(transfer.FromAccountNum);
        var ToAccount = await accountService.GetByAccNum(transfer.ToAccountNum);

        if(fromAccount is null || ToAccount is null)
        {
            return true;
        }
        return false;
    }
    
    private bool transferNullOrEmpty(TransferDtoIn transfer)
    {
        if(string.IsNullOrEmpty(transfer.ToString())
         || object.ReferenceEquals("", transfer))
        {
            return true;
        }
        return false;
    }

    private async Task<bool> equalBanks(TransferDtoIn transfer)
    {
        var fromAccount = await accountService.GetByAccNum(transfer.FromAccountNum);
        var toAccount = await accountService.GetByAccNum(transfer.ToAccountNum);

        if(fromAccount is not null && toAccount is not null)
        {
            if(fromAccount.Bank.Equals(toAccount.Bank))
            {
                return true;
            }
        }
        return false;
    }
    
    private async void makeTransfer(TransferDtoIn transfer)
    {
        var fromAccount = await accountService.GetByAccNum(transfer.FromAccountNum);
        var fromClient = await clientService.GetById(transfer.FromClientDocNumber);

        var toAccount = await accountService.GetByAccNum(transfer.ToAccountNum);
        var toClient = await clientService.GetById(transfer.ToClientDocNumber);

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
    }
}