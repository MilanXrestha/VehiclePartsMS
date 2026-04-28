using Microsoft.AspNetCore.Mvc;
using VehiclePartsMS.Application.DTOs;
using VehiclePartsMS.Application.Interfaces.IServices;

namespace VehiclePartsMS.Controllers;

[Route("api/[controller]")]
[ApiController]
public class SalesInvoicesController(ISalesInvoiceService service) : ControllerBase
{
    // GET: api/salesinvoices
    [HttpGet]
    public async Task<ActionResult<IEnumerable<SalesInvoiceSummaryDto>>> GetSalesInvoices()
        => Ok(await service.GetAllAsync());

    // GET: api/salesinvoices/{id}
    [HttpGet("{id}")]
    public async Task<ActionResult<SalesInvoiceResponseDto>> GetSalesInvoice(long id)
    {
        var invoice = await service.GetByIdAsync(id);
        return invoice is null ? NotFound() : Ok(invoice);
    }

    // GET: api/salesinvoices/customer/{customerProfileId}
    [HttpGet("customer/{customerProfileId}")]
    public async Task<ActionResult<IEnumerable<SalesInvoiceSummaryDto>>> GetByCustomer(long customerProfileId)
        => Ok(await service.GetByCustomerAsync(customerProfileId));

    // POST: api/salesinvoices
    [HttpPost]
    public async Task<ActionResult<SalesInvoiceResponseDto>> PostSalesInvoice(SalesInvoiceCreateDto dto)
    {
        try
        {
            var created = await service.CreateAsync(dto);
            return CreatedAtAction(nameof(GetSalesInvoice), new { id = created.Id }, created);
        }
        catch (KeyNotFoundException ex)      { return NotFound(ex.Message); }
        catch (InvalidOperationException ex) { return BadRequest(ex.Message); }
    }
}
