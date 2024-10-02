using DynamicObjectAPI.Api.DTOs;
using Microsoft.AspNetCore.Mvc;

[Route("api/[controller]")]
[ApiController]
public class DynamicController : ControllerBase
{
    private readonly IDynamicService _dynamicService;

    public DynamicController(IDynamicService dynamicService)
    {
        _dynamicService = dynamicService;
    }

 
    [HttpPost("create")]
    public async Task<IActionResult> Create([FromBody] CreateObjectRequest request)
    {
        try
        {
            var objectType = request.ObjectType;
            var data = request.Data;

            if (string.IsNullOrEmpty(objectType) || data == null || !data.Any())
            {
                return BadRequest("Invalid data format. 'objectType' and 'data' fields are required.");
            }

            int createdId = await _dynamicService.CreateObjectAsync(objectType, data);

            return Ok(new { Message = "Object created successfully", Id = createdId });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { Message = "An error occurred while creating the object", Error = ex.Message });
        }
    }

    [HttpGet("customers")]
    public async Task<IActionResult> GetAllCustomers()
    {
        try
        {
            var result = await _dynamicService.GetAllCustomersAsync();
            return Ok(result);
        }
        catch (System.Exception ex)
        {
            return StatusCode(500, new { Message = "An error occurred while retrieving data", Error = ex.Message });
        }
    }

    [HttpDelete("customers/{customerId}")]
    public async Task<IActionResult> DeleteCustomer(int customerId)
    {
        try
        {
            await _dynamicService.DeleteCustomerAsync(customerId);
            return Ok(new { Message = "Customer and related data deleted successfully" });
        }
        catch (System.Exception ex)
        {
            return StatusCode(500, new { Message = "An error occurred while deleting the customer", Error = ex.Message });
        }
    }
}
