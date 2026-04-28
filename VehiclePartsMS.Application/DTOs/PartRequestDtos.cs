using VehiclePartsMS.Domain.Models;

namespace VehiclePartsMS.Application.DTOs;

public record PartRequestCreateDto(
    long CustomerProfileId,
    string PartName,
    string? Description);

public record PartRequestUpdateDto(
    PartRequestStatus Status);

public record PartRequestResponseDto(
    long Id,
    long CustomerProfileId,
    string CustomerName,
    string PartName,
    string? Description,
    DateTime RequestDate,
    PartRequestStatus Status);
