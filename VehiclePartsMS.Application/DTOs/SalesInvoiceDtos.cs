using VehiclePartsMS.Domain.Models;

namespace VehiclePartsMS.Application.DTOs;

public record SalesInvoiceItemCreateDto(
    long PartId,
    int Quantity,
    decimal UnitPrice);

public record SalesInvoiceCreateDto(
    long CustomerProfileId,
    long StaffId,
    PaymentStatus PaymentStatus,
    List<SalesInvoiceItemCreateDto> Items);

public record SalesInvoiceItemResponseDto(
    long Id,
    long PartId,
    string PartName,
    int Quantity,
    decimal UnitPrice,
    decimal LineTotal);

public record SalesInvoiceResponseDto(
    long Id,
    long CustomerProfileId,
    string CustomerName,
    long StaffId,
    string StaffName,
    DateTime SaleDate,
    decimal SubTotal,
    decimal DiscountPercent,
    decimal TotalAmount,
    PaymentStatus PaymentStatus,
    IEnumerable<SalesInvoiceItemResponseDto> Items);

public record SalesInvoiceSummaryDto(
    long Id,
    DateTime SaleDate,
    decimal TotalAmount,
    decimal DiscountPercent,
    PaymentStatus PaymentStatus);
