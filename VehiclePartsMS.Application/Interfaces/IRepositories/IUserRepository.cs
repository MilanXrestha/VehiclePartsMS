using VehiclePartsMS.Application.DTOs;
using VehiclePartsMS.Domain.Models;

namespace VehiclePartsMS.Application.Interfaces.IRepositories;

public interface IUserRepository
{
    Task<IEnumerable<UserResponseDto>> GetAllAsync();
    Task<IEnumerable<UserResponseDto>> GetByRoleAsync(UserRole role);
    Task<User?> FindByIdAsync(long id);
    Task<User?> FindByEmailAsync(string email);
    Task<User> AddAsync(User user);
    Task UpdateAsync(User user);
    Task DeleteAsync(User user);
    Task<bool> ExistsAsync(long id);
    Task<bool> EmailExistsAsync(string email);
}
