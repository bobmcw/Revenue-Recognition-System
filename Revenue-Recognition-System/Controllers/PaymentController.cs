using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Revenue_Recognition_System.Exceptions;
using Revenue_Recognition_System.PostDTOs;
using Revenue_Recognition_System.Services;

namespace Revenue_Recognition_System.Controllers;

[ApiController]
[Route("api/payments")]
[Authorize]
public class PaymentController(IPaymentService service) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> MakePayment([FromBody] PaymentDto dto)
    {
        try
        {
            var msg = await service.MakePaymentAsync(dto.ContractId, dto.Amount);
            if (string.IsNullOrEmpty(msg))
            {
                return Created();
            }
            return Created("", new {Message = msg});
        }
        catch (InvalidPaymentException e)
        {
            return BadRequest(e.Message);
        }
        catch (NoSuchContractException e)
        {
            return NotFound(e.Message);
        }
    }
}