using VehiclePartsMS.Application.DTOs;
using VehiclePartsMS.Domain.Models;

namespace VehiclePartsMS.Application.Interfaces.IRepositories;

public interface ISalesInvoiceRepository
{
    Task<IEnumerable<SalesInvoiceSummaryDto>> GetAllAsync();
    Task<SalesInvoice?> FindByIdAsync(long id);
    Task<SalesInvoiceResponseDto?> GetWithItemsAsync(long id);
    Task<IEnumerable<SalesInvoiceSummaryDto>> GetByCustomerAsync(long customerProfileId);
    Task<IEnumerable<SalesInvoiceSummaryDto>> GetByDateRangeAsync(DateTime from, DateTime to);
    Task<SalesInvoice> AddAsync(SalesInvoice invoice);
    Task<bool> ExistsAsync(long id);
}
