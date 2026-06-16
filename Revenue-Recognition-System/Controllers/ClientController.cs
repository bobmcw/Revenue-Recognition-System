using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Revenue_Recognition_System.Exceptions;
using Revenue_Recognition_System.PostDTOs;
using Revenue_Recognition_System.Services;

namespace Revenue_Recognition_System.Controllers;

[ApiController]
[Route("api/clients")]
[Authorize]
public class ClientController(IClientService serv) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetClientsAsync()
    {
        return Ok(await serv.GetAllAsync());
    }

    [HttpPost]
    public async Task<IActionResult> PostClient([FromBody] CreateBaseClientDto dto)
    {
        try
        {
            await serv.PostClient(dto);
            return Created();
        }
        catch (UnknownClientTypeException e)
        {
            return BadRequest(e.Message);
        }
    }
}