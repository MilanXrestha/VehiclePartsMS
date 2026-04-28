using VehiclePartsMS.Domain.Models;

namespace VehiclePartsMS.Application.DTOs;

public record AppointmentCreateDto(
    long CustomerProfileId,
    long? VehicleId,
    DateTime AppointmentDate,
    string? Notes);

public record AppointmentUpdateDto(
    AppointmentStatus Status,
    string? Notes);

public record AppointmentResponseDto(
    long Id,
    long CustomerProfileId,
    string CustomerName,
    long? VehicleId,
    string? VehicleNumber,
    DateTime AppointmentDate,
    AppointmentStatus Status,
    string? Notes,
    DateTime CreatedAt);
