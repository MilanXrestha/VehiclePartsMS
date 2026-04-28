using VehiclePartsMS.Application.DTOs;

namespace VehiclePartsMS.Application.Interfaces.IServices;

public interface ISalesInvoiceService
{
    Task<IEnumerable<SalesInvoiceSummaryDto>> GetAllAsync();
    Task<SalesInvoiceResponseDto?> GetByIdAsync(long id);
    Task<IEnumerable<SalesInvoiceSummaryDto>> GetByCustomerAsync(long customerProfileId);
    Task<SalesInvoiceResponseDto> CreateAsync(SalesInvoiceCreateDto dto);
}
