using System.Security.Cryptography;
using System.Text;
using VehiclePartsMS.Application.DTOs;
using VehiclePartsMS.Application.Interfaces.IRepositories;
using VehiclePartsMS.Application.Interfaces.IServices;
using VehiclePartsMS.Domain.Models;

namespace VehiclePartsMS.Infrastructure.Services;

public class UserService(IUserRepository repo) : IUserService
{
    public Task<IEnumerable<UserResponseDto>> GetAllAsync() => repo.GetAllAsync();

    public Task<IEnumerable<UserResponseDto>> GetStaffAsync() => repo.GetByRoleAsync(UserRole.Staff);

    public async Task<UserResponseDto?> GetByIdAsync(long id)
    {
        var user = await repo.FindByIdAsync(id);
        return user is null ? null
            : new UserResponseDto(user.Id, user.FirstName, user.LastName, user.Email, user.Phone, user.Role, user.IsActive, user.CreatedAt);
    }

    public async Task<UserResponseDto> CreateAsync(UserCreateDto dto)
    {
        if (await repo.EmailExistsAsync(dto.Email))
            throw new InvalidOperationException($"Email '{dto.Email}' is already in use.");

        var user = new User
        {
            FirstName    = dto.FirstName,
            LastName     = dto.LastName,
            Email        = dto.Email,
            Phone        = dto.Phone,
            PasswordHash = HashPassword(dto.Password),
            Role         = dto.Role,
            IsActive     = true,
            CreatedAt    = DateTime.UtcNow
        };

        var created = await repo.AddAsync(user);
        return new UserResponseDto(created.Id, created.FirstName, created.LastName,
                                   created.Email, created.Phone, created.Role, created.IsActive, created.CreatedAt);
    }

    public async Task UpdateAsync(long id, UserUpdateDto dto)
    {
        var existing = await repo.FindByIdAsync(id)
            ?? throw new KeyNotFoundException($"User {id} not found.");

        existing.FirstName = dto.FirstName;
        existing.LastName  = dto.LastName;
        existing.Phone     = dto.Phone;
        existing.IsActive  = dto.IsActive;

        await repo.UpdateAsync(existing);
    }

    public async Task DeleteAsync(long id)
    {
        var existing = await repo.FindByIdAsync(id)
            ?? throw new KeyNotFoundException($"User {id} not found.");
        await repo.DeleteAsync(existing);
    }

    private static string HashPassword(string password)
    {
        var bytes = SHA256.HashData(Encoding.UTF8.GetBytes(password));
        return Convert.ToHexString(bytes);
    }
}
