using Microsoft.EntityFrameworkCore;
using VehiclePartsMS.Application.DTOs;
using VehiclePartsMS.Application.Interfaces.IRepositories;
using VehiclePartsMS.Domain.Models;
using VehiclePartsMS.Infrastructure.Persistance;

namespace VehiclePartsMS.Infrastructure.Repositories;

public class SalesInvoiceRepository(ApplicationDbContext context) : ISalesInvoiceRepository
{
    public async Task<IEnumerable<SalesInvoiceSummaryDto>> GetAllAsync()
        => await context.SalesInvoices
            .Select(si => new SalesInvoiceSummaryDto(si.Id, si.SaleDate, si.TotalAmount, si.DiscountPercent, si.PaymentStatus))
            .ToListAsync();

    public async Task<SalesInvoice?> FindByIdAsync(long id)
        => await context.SalesInvoices
            .Include(si => si.Items).ThenInclude(item => item.Part)
            .Include(si => si.CustomerProfile).ThenInclude(cp => cp.User)
            .Include(si => si.Staff)
            .FirstOrDefaultAsync(si => si.Id == id);

    public async Task<SalesInvoiceResponseDto?> GetWithItemsAsync(long id)
        => await context.SalesInvoices
            .Include(si => si.CustomerProfile).ThenInclude(cp => cp.User)
            .Include(si => si.Staff)
            .Include(si => si.Items).ThenInclude(item => item.Part)
            .Where(si => si.Id == id)
            .Select(si => new SalesInvoiceResponseDto(
                si.Id, si.CustomerProfileId,
                si.CustomerProfile.User.FirstName + " " + si.CustomerProfile.User.LastName,
                si.StaffId, si.Staff.FirstName + " " + si.Staff.LastName,
                si.SaleDate, si.SubTotal, si.DiscountPercent, si.TotalAmount, si.PaymentStatus,
                si.Items.Select(item => new SalesInvoiceItemResponseDto(
                    item.Id, item.PartId, item.Part.Name, item.Quantity, item.UnitPrice, item.Quantity * item.UnitPrice))))
            .FirstOrDefaultAsync();

    public async Task<IEnumerable<SalesInvoiceSummaryDto>> GetByCustomerAsync(long customerProfileId)
        => await context.SalesInvoices
            .Where(si => si.CustomerProfileId == customerProfileId)
            .Select(si => new SalesInvoiceSummaryDto(si.Id, si.SaleDate, si.TotalAmount, si.DiscountPercent, si.PaymentStatus))
            .ToListAsync();

    public async Task<IEnumerable<SalesInvoiceSummaryDto>> GetByDateRangeAsync(DateTime from, DateTime to)
        => await context.SalesInvoices
            .Where(si => si.SaleDate >= from && si.SaleDate <= to)
            .Select(si => new SalesInvoiceSummaryDto(si.Id, si.SaleDate, si.TotalAmount, si.DiscountPercent, si.PaymentStatus))
            .ToListAsync();

    public async Task<SalesInvoice> AddAsync(SalesInvoice invoice)
    {
        context.SalesInvoices.Add(invoice);
        await context.SaveChangesAsync();
        return invoice;
    }

    public async Task<bool> ExistsAsync(long id)
        => await context.SalesInvoices.AnyAsync(si => si.Id == id);
}
