namespace VehiclePartsMS.Application.DTOs;

// ── Vehicle DTOs ──────────────────────────────────────────────
public record VehicleCreateDto(
    string VehicleNumber,
    string? Make,
    string? Model,
    int? Year);

public record VehicleResponseDto(
    long Id,
    string VehicleNumber,
    string? Make,
    string? Model,
    int? Year);

// ── Customer DTOs ─────────────────────────────────────────────
public record CustomerCreateDto(
    string FirstName,
    string LastName,
    string Email,
    string? Phone,
    string Password,
    string? Address,
    List<VehicleCreateDto>? Vehicles);

public record CustomerUpdateDto(
    string? Address);

public record CustomerResponseDto(
    long Id,
    long UserId,
    string FirstName,
    string LastName,
    string Email,
    string? Phone,
    string? Address,
    decimal TotalSpent,
    decimal CreditBalance);

public record CustomerWithDetailsDto(
    long Id,
    long UserId,
    string FirstName,
    string LastName,
    string Email,
    string? Phone,
    string? Address,
    decimal TotalSpent,
    decimal CreditBalance,
    IEnumerable<VehicleResponseDto> Vehicles);

public record CustomerHistoryDto(
    long Id,
    string FirstName,
    string LastName,
    IEnumerable<SalesInvoiceSummaryDto> Purchases,
    IEnumerable<AppointmentResponseDto> Appointments);
