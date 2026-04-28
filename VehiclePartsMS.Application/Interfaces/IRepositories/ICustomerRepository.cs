using VehiclePartsMS.Application.DTOs;
using VehiclePartsMS.Domain.Models;

namespace VehiclePartsMS.Application.Interfaces.IRepositories;

public interface ICustomerRepository
{
    Task<IEnumerable<CustomerResponseDto>> GetAllAsync();
    Task<CustomerProfile?> FindByIdAsync(long id);
    Task<CustomerWithDetailsDto?> GetWithDetailsAsync(long id);
    Task<CustomerHistoryDto?> GetHistoryAsync(long id);
    Task<IEnumerable<CustomerResponseDto>> SearchByNameAsync(string name);
    Task<IEnumerable<CustomerResponseDto>> SearchByPhoneAsync(string phone);
    Task<IEnumerable<CustomerResponseDto>> SearchByVehicleNumberAsync(string vehicleNumber);
    Task<IEnumerable<CustomerResponseDto>> SearchByIdAsync(long userId);
    Task<CustomerProfile> AddAsync(CustomerProfile customer);
    Task UpdateAsync(CustomerProfile customer);
    Task<bool> ExistsAsync(long id);
}
