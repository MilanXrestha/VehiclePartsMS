using VehiclePartsMS.Application.DTOs;
using VehiclePartsMS.Domain.Models;

namespace VehiclePartsMS.Application.Interfaces.IRepositories;

public interface IReviewRepository
{
    Task<IEnumerable<ReviewResponseDto>> GetAllAsync();
    Task<IEnumerable<ReviewResponseDto>> GetByCustomerAsync(long customerProfileId);
    Task<ReviewResponseDto?> GetByIdAsync(long id);
    Task<Review?> FindByIdAsync(long id);
    Task<Review> AddAsync(Review review);
    Task<bool> ExistsAsync(long id);
}
