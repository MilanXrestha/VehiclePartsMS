using VehiclePartsMS.Application.DTOs;
using VehiclePartsMS.Application.Interfaces.IRepositories;
using VehiclePartsMS.Application.Interfaces.IServices;
using VehiclePartsMS.Domain.Models;

namespace VehiclePartsMS.Infrastructure.Services;

public class ReviewService(IReviewRepository repo) : IReviewService
{
    public Task<IEnumerable<ReviewResponseDto>> GetAllAsync() => repo.GetAllAsync();

    public Task<IEnumerable<ReviewResponseDto>> GetByCustomerAsync(long customerProfileId)
        => repo.GetByCustomerAsync(customerProfileId);

    public async Task<ReviewResponseDto> CreateAsync(ReviewCreateDto dto)
    {
        if (dto.Rating < 1 || dto.Rating > 5)
            throw new ArgumentException("Rating must be between 1 and 5.");

        var review = new Review
        {
            CustomerProfileId = dto.CustomerProfileId,
            Rating            = dto.Rating,
            Comment           = dto.Comment,
            ReviewDate        = DateTime.UtcNow
        };
        var created = await repo.AddAsync(review);
        return (await repo.GetByIdAsync(created.Id))!;
    }
}
