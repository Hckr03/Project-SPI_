using BankAPI_.Dtos;
using BankAPI_.Models;
using BankAPI_.Services;
using Microsoft.AspNetCore.Mvc;

namespace BankAPI_.Controllers;


[ApiController]
[Route("[controller]")]
public class ClientController : ControllerBase
{
    private readonly ClientService clientService;

    public ClientController(AccountService accountService, BankService bankService, ClientService clientService)
    {
        this.clientService = clientService;
    }

    [HttpPost]
    public async Task<ActionResult<Client>> Create(Client client)
    {
        if(await clientService.GetById(client.ClientDocNum) is null)
        {
            var newClient = await clientService.Create(client);
            return Ok(new { message = $"Se creo cliente con exito!"});
        }
        return BadRequest(new { message = $"El cliente con nro. de CI: ({client.ClientDocNum}) ya existe!"} );
    }

    [HttpGet]
    public async Task<ICollection<Client>> GetAll()
    {
       return await clientService.GetAll();
    }

    [HttpGet("{docNum}")]
    public async Task<ActionResult<Client>> GetById(string docNum)
    {
        var client = await clientService.GetById(docNum);
        if(client is null)
        {
            return NotFound(new { message = $"El cliente con ID = ({docNum}) no existe!"} );
        }
        return client;
    }

    [HttpPut("{docNum}")]
    public async Task<ActionResult<Client>> Update(string docNum, Client client)
    {
        if(docNum != client.ClientDocNum)
        {
            return BadRequest( new { message = $"El cliente con nro = ({docNum}) de la URL no coincide con el nro = ({client.ClientDocNum}) del cuerpo solicitado!"} );
        }

        var clientToUpdate = await clientService.GetById(docNum);
        if(clientToUpdate is null)
        {
            return BadRequest( new { message = $"No existe el cliente con ID = ({docNum}) !"} );
        }

        await clientService.Update(docNum, client);
        return NoContent();        
    }

    [HttpDelete("{docNum}")]
    public async Task<ActionResult<Client>> Delete(string docNum)
    {
        var clientToDelete = await clientService.GetById(docNum);
        if(clientToDelete is null)
        {
            return BadRequest( new { message = $"El cliente con ID = ({docNum}) no existe!"});
        }
        await clientService.Delete(clientToDelete);
        return Ok(new { message = $"El cliente con Nro. de documento ({docNum}) ha sido eliminado!)"});
    }
}