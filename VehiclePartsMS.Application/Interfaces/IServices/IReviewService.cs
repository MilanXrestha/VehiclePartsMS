using VehiclePartsMS.Application.DTOs;

namespace VehiclePartsMS.Application.Interfaces.IServices;

public interface IReviewService
{
    Task<IEnumerable<ReviewResponseDto>> GetAllAsync();
    Task<IEnumerable<ReviewResponseDto>> GetByCustomerAsync(long customerProfileId);
    Task<ReviewResponseDto> CreateAsync(ReviewCreateDto dto);
}
