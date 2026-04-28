using Microsoft.EntityFrameworkCore;
using VehiclePartsMS.Application.DTOs;
using VehiclePartsMS.Application.Interfaces.IRepositories;
using VehiclePartsMS.Domain.Models;
using VehiclePartsMS.Infrastructure.Persistance;

namespace VehiclePartsMS.Infrastructure.Repositories;

public class UserRepository(ApplicationDbContext context) : IUserRepository
{
    public async Task<IEnumerable<UserResponseDto>> GetAllAsync()
        => await context.Users
            .Select(u => new UserResponseDto(u.Id, u.FirstName, u.LastName, u.Email, u.Phone, u.Role, u.IsActive, u.CreatedAt))
            .ToListAsync();

    public async Task<IEnumerable<UserResponseDto>> GetByRoleAsync(UserRole role)
        => await context.Users
            .Where(u => u.Role == role)
            .Select(u => new UserResponseDto(u.Id, u.FirstName, u.LastName, u.Email, u.Phone, u.Role, u.IsActive, u.CreatedAt))
            .ToListAsync();

    public async Task<User?> FindByIdAsync(long id)
        => await context.Users.FindAsync(id);

    public async Task<User?> FindByEmailAsync(string email)
        => await context.Users.FirstOrDefaultAsync(u => u.Email == email);

    public async Task<User> AddAsync(User user)
    {
        context.Users.Add(user);
        await context.SaveChangesAsync();
        return user;
    }

    public async Task UpdateAsync(User user)
    {
        context.Users.Update(user);
        await context.SaveChangesAsync();
    }

    public async Task DeleteAsync(User user)
    {
        context.Users.Remove(user);
        await context.SaveChangesAsync();
    }

    public async Task<bool> ExistsAsync(long id)
        => await context.Users.AnyAsync(u => u.Id == id);

    public async Task<bool> EmailExistsAsync(string email)
        => await context.Users.AnyAsync(u => u.Email == email);
}
