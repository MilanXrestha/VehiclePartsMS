using VehiclePartsMS.Application.DTOs;
using VehiclePartsMS.Application.Interfaces.IRepositories;
using VehiclePartsMS.Application.Interfaces.IServices;
using VehiclePartsMS.Domain.Models;

namespace VehiclePartsMS.Infrastructure.Services;

public class SalesInvoiceService(
    ISalesInvoiceRepository repo,
    IVehiclePartRepository partRepo,
    ICustomerRepository customerRepo) : ISalesInvoiceService
{
    private const decimal LoyaltyThreshold = 5000m;
    private const decimal LoyaltyDiscount  = 10m;

    public Task<IEnumerable<SalesInvoiceSummaryDto>> GetAllAsync() => repo.GetAllAsync();

    public Task<SalesInvoiceResponseDto?> GetByIdAsync(long id) => repo.GetWithItemsAsync(id);

    public Task<IEnumerable<SalesInvoiceSummaryDto>> GetByCustomerAsync(long customerProfileId)
        => repo.GetByCustomerAsync(customerProfileId);

    public async Task<SalesInvoiceResponseDto> CreateAsync(SalesInvoiceCreateDto dto)
    {
        var items   = new List<SalesInvoiceItem>();
        decimal sub = 0;

        foreach (var itemDto in dto.Items)
        {
            var part = await partRepo.FindByIdAsync(itemDto.PartId)
                ?? throw new KeyNotFoundException($"Part {itemDto.PartId} not found.");

            if (part.StockQuantity < itemDto.Quantity)
                throw new InvalidOperationException(
                    $"Insufficient stock for '{part.Name}'. Available: {part.StockQuantity}.");

            items.Add(new SalesInvoiceItem
            {
                PartId    = part.Id,
                Quantity  = itemDto.Quantity,
                UnitPrice = itemDto.UnitPrice
            });

            part.StockQuantity -= itemDto.Quantity;
            await partRepo.UpdateAsync(part);

            sub += itemDto.Quantity * itemDto.UnitPrice;
        }

        // Feature 16: 10% loyalty discount when subtotal > 5000
        decimal discountPct = sub > LoyaltyThreshold ? LoyaltyDiscount : 0m;
        decimal total       = sub * (1 - discountPct / 100);

        var invoice = new SalesInvoice
        {
            CustomerProfileId = dto.CustomerProfileId,
            StaffId           = dto.StaffId,
            SaleDate          = DateTime.UtcNow,
            SubTotal          = sub,
            DiscountPercent   = discountPct,
            TotalAmount       = total,
            PaymentStatus     = dto.PaymentStatus,
            Items             = items
        };
        var created = await repo.AddAsync(invoice);

        // Update customer spending totals
        var customer = await customerRepo.FindByIdAsync(dto.CustomerProfileId);
        if (customer is not null)
        {
            customer.TotalSpent += total;
            if (dto.PaymentStatus == PaymentStatus.Credit)
                customer.CreditBalance += total;
            await customerRepo.UpdateAsync(customer);
        }

        return (await repo.GetWithItemsAsync(created.Id))!;
    }
}
