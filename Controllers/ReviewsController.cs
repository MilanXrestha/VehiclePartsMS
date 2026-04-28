using Microsoft.AspNetCore.Mvc;
using VehiclePartsMS.Application.DTOs;
using VehiclePartsMS.Application.Interfaces.IServices;

namespace VehiclePartsMS.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ReviewsController(IReviewService service) : ControllerBase
{
    // GET: api/reviews
    [HttpGet]
    public async Task<ActionResult<IEnumerable<ReviewResponseDto>>> GetReviews()
        => Ok(await service.GetAllAsync());

    // GET: api/reviews/customer/{customerProfileId}
    [HttpGet("customer/{customerProfileId}")]
    public async Task<ActionResult<IEnumerable<ReviewResponseDto>>> GetByCustomer(long customerProfileId)
        => Ok(await service.GetByCustomerAsync(customerProfileId));

    // POST: api/reviews
    [HttpPost]
    public async Task<ActionResult<ReviewResponseDto>> PostReview(ReviewCreateDto dto)
    {
        try
        {
            var created = await service.CreateAsync(dto);
            return Ok(created);
        }
        catch (ArgumentException ex) { return BadRequest(ex.Message); }
    }
}
