using BankAPI_.Dtos;
using BankAPI_.Models;
using BankAPI_.Services;
using Microsoft.AspNetCore.Mvc;

namespace BankAPI_.Controllers;


[ApiController]
[Route("[controller]")]
public class ClientController : ControllerBase
{
    // private readonly AccountService accountService;
    // private readonly BankService bankService;
    private readonly ClientService clientService;

    public ClientController(AccountService accountService, BankService bankService, ClientService clientService)
    {
        // this.accountService = accountService;
        // this.bankService = bankService;
        this.clientService = clientService;
    }

    [HttpPost]
    public async Task<ActionResult<Client>> Create(Client client)
    {
        if(await clientService.GetById(client.ClientDocNum) is null)
        {
            return BadRequest(new { message = $"El cliente con nro. de CI: ({client.ClientDocNum}) ya existe!"} );
        }
        await clientService.Create(client);
        return CreatedAtAction(nameof(GetById), new {id = client.ClientDocNum}, client);
    }

    [HttpGet]
    public async Task<ICollection<Client>> GetAll()
    {
       return await clientService.GetAll();
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Client>> GetById(Client client)
    {
        var newClient = await clientService.GetById(client.ClientDocNum);
        if(newClient is null)
        {
            return NotFound(new { message = $"El cliente con ID = ({client.ClientDocNum}) no existe!"} );
        }
        return newClient;
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<Client>> Update(string id)
    {
        var clientToUpdate = await clientService.GetById(id);
        if(clientToUpdate is not null)
        {
            await clientService.Update(clientToUpdate);
            return NoContent();
        }
        return BadRequest( new { message = $"No existe el cliente con ID = ({id}) !"} );
    }
}