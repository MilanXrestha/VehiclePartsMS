namespace VehiclePartsMS.Application.DTOs;

public record ReviewCreateDto(
    long CustomerProfileId,
    int Rating,
    string? Comment);

public record ReviewResponseDto(
    long Id,
    long CustomerProfileId,
    string CustomerName,
    int Rating,
    string? Comment,
    DateTime ReviewDate);
