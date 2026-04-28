using VehiclePartsMS.Application.DTOs;
using VehiclePartsMS.Domain.Models;

namespace VehiclePartsMS.Application.Interfaces.IServices;

public interface IUserService
{
    Task<IEnumerable<UserResponseDto>> GetAllAsync();
    Task<IEnumerable<UserResponseDto>> GetStaffAsync();
    Task<UserResponseDto?> GetByIdAsync(long id);
    Task<UserResponseDto> CreateAsync(UserCreateDto dto);
    Task UpdateAsync(long id, UserUpdateDto dto);
    Task DeleteAsync(long id);
}
