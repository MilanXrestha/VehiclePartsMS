using VehiclePartsMS.Domain.Models;

namespace VehiclePartsMS.Application.DTOs;

public record UserCreateDto(
    string FirstName,
    string LastName,
    string Email,
    string? Phone,
    string Password,
    UserRole Role);

public record UserUpdateDto(
    string FirstName,
    string LastName,
    string? Phone,
    bool IsActive);

public record UserResponseDto(
    long Id,
    string FirstName,
    string LastName,
    string Email,
    string? Phone,
    UserRole Role,
    bool IsActive,
    DateTime CreatedAt);

public record StaffSummaryDto(
    long Id,
    string FirstName,
    string LastName,
    string Email);
