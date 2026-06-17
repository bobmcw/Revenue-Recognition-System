using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Revenue_Recognition_System.Enums;
using Revenue_Recognition_System.PostDTOs;
using Revenue_Recognition_System.Services;

namespace Revenue_Recognition_System.Controllers;

[ApiController]
[Route("api/")]
[Authorize]
public class ProductController(IProductService service) : ControllerBase
{
    [HttpPost]
    [Route("products")]
    [Authorize(Roles = nameof(UserRole.Admin))]
    public async Task<IActionResult> PostProduct(CreateProductDto dto)
    {
        await service.AddNewProductAsync(dto);
        return Created();
    }
    [HttpPost]
    [Route("contract")]
    public async Task<IActionResult> CreateContractAsync([FromBody] CreateContractDto dto)
    {
        try
        {
            var msg = await service.CreateContractForProductAsync(dto);
            return Created("", new { Message = msg });
        }
        catch (InvalidDataException e)
        {
            return BadRequest(e.Message);
        }
    }
}