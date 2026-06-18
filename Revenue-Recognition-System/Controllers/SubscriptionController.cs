using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Revenue_Recognition_System.Exceptions;
using Revenue_Recognition_System.PostDTOs;
using Revenue_Recognition_System.Services;

namespace Revenue_Recognition_System.Controllers;

[ApiController]
[Route("api/subscriptions")]
[Authorize]
public class SubscriptionController(ISubscriptionService service) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> CreateSubscriptionAsync([FromBody] CreateSubscriptionDto dto)
    {
        try
        {
            var msg = await service.MakeNewSubscription(dto);
            return Created("", new { Message = msg });
        }
        catch (NoSuchClientException e)
        {
            return NotFound(e.Message);
        }
        catch (NoSuchProductException e)
        {
            return NotFound(e.Message);
        }
        catch (InvalidDataException e)
        {
            return BadRequest(e.Message);
        }
    }

    [HttpPost("payments")]
    public async Task<IActionResult> PayForSubscriptionAsync([FromBody] SubscriptionPaymentDto dto)
    {
        try
        {
            var msg = await service.PayForSubscription(dto);
            return Created("", new { Message = msg });
        }
        catch (InvalidPaymentException e)
        {
            return BadRequest(e.Message);
        }
        catch (ExpiredException e)
        {
            return Conflict(e.Message);
        }
    }
}
