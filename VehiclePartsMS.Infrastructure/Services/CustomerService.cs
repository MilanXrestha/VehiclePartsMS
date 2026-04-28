using System.Security.Cryptography;
using System.Text;
using VehiclePartsMS.Application.DTOs;
using VehiclePartsMS.Application.Interfaces.IRepositories;
using VehiclePartsMS.Application.Interfaces.IServices;
using VehiclePartsMS.Domain.Models;

namespace VehiclePartsMS.Infrastructure.Services;

public class CustomerService(ICustomerRepository repo, IUserRepository userRepo) : ICustomerService
{
    public Task<IEnumerable<CustomerResponseDto>> GetAllAsync() => repo.GetAllAsync();

    public Task<CustomerWithDetailsDto?> GetWithDetailsAsync(long id) => repo.GetWithDetailsAsync(id);

    public Task<CustomerHistoryDto?> GetHistoryAsync(long id) => repo.GetHistoryAsync(id);

    public async Task<IEnumerable<CustomerResponseDto>> SearchAsync(string? name, string? phone, string? vehicleNumber, long? userId)
    {
        if (!string.IsNullOrWhiteSpace(name))         return await repo.SearchByNameAsync(name);
        if (!string.IsNullOrWhiteSpace(phone))        return await repo.SearchByPhoneAsync(phone);
        if (!string.IsNullOrWhiteSpace(vehicleNumber)) return await repo.SearchByVehicleNumberAsync(vehicleNumber);
        if (userId.HasValue)                           return await repo.SearchByIdAsync(userId.Value);

        return await repo.GetAllAsync();
    }

    public async Task<CustomerResponseDto> CreateAsync(CustomerCreateDto dto)
    {
        if (await userRepo.EmailExistsAsync(dto.Email))
            throw new InvalidOperationException($"Email '{dto.Email}' is already in use.");

        var user = new User
        {
            FirstName    = dto.FirstName,
            LastName     = dto.LastName,
            Email        = dto.Email,
            Phone        = dto.Phone,
            PasswordHash = Hash(dto.Password),
            Role         = UserRole.Customer,
            IsActive     = true,
            CreatedAt    = DateTime.UtcNow
        };
        var createdUser = await userRepo.AddAsync(user);

        var profile = new CustomerProfile
        {
            UserId  = createdUser.Id,
            Address = dto.Address,
            Vehicles = dto.Vehicles?.Select(v => new Vehicle
            {
                VehicleNumber = v.VehicleNumber,
                Make          = v.Make,
                Model         = v.Model,
                Year          = v.Year
            }).ToList() ?? []
        };
        var created = await repo.AddAsync(profile);

        return new CustomerResponseDto(created.Id, createdUser.Id, createdUser.FirstName, createdUser.LastName,
                                       createdUser.Email, createdUser.Phone, created.Address, 0, 0);
    }

    public async Task UpdateAsync(long id, CustomerUpdateDto dto)
    {
        var existing = await repo.FindByIdAsync(id)
            ?? throw new KeyNotFoundException($"Customer profile {id} not found.");

        existing.Address = dto.Address;
        await repo.UpdateAsync(existing);
    }

    public async Task AddVehicleAsync(long customerProfileId, VehicleCreateDto dto)
    {
        var profile = await repo.FindByIdAsync(customerProfileId)
            ?? throw new KeyNotFoundException($"Customer profile {customerProfileId} not found.");

        profile.Vehicles.Add(new Vehicle
        {
            CustomerProfileId = customerProfileId,
            VehicleNumber     = dto.VehicleNumber,
            Make              = dto.Make,
            Model             = dto.Model,
            Year              = dto.Year
        });
        await repo.UpdateAsync(profile);
    }

    private static string Hash(string password)
    {
        var bytes = SHA256.HashData(Encoding.UTF8.GetBytes(password));
        return Convert.ToHexString(bytes);
    }
}
