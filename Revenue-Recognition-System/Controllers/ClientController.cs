using Microsoft.AspNetCore.Mvc;
using Revenue_Recognition_System.Services;

namespace Revenue_Recognition_System.Controllers;

[ApiController]
[Route("api/clients")]
public class ClientController(IClientService serv) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetClientsAsync()
    {
        return Ok(await serv.GetAllAsync());
    }
}