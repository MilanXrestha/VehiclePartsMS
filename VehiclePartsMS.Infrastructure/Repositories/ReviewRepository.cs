using Microsoft.EntityFrameworkCore;
using VehiclePartsMS.Application.DTOs;
using VehiclePartsMS.Application.Interfaces.IRepositories;
using VehiclePartsMS.Domain.Models;
using VehiclePartsMS.Infrastructure.Persistance;

namespace VehiclePartsMS.Infrastructure.Repositories;

public class ReviewRepository(ApplicationDbContext context) : IReviewRepository
{
    private static ReviewResponseDto MapToDto(Review r) =>
        new(r.Id, r.CustomerProfileId,
            r.CustomerProfile.User.FirstName + " " + r.CustomerProfile.User.LastName,
            r.Rating, r.Comment, r.ReviewDate);

    private IQueryable<Review> WithIncludes()
        => context.Reviews
            .Include(r => r.CustomerProfile).ThenInclude(cp => cp.User);

    public async Task<IEnumerable<ReviewResponseDto>> GetAllAsync()
        => (await WithIncludes().ToListAsync()).Select(MapToDto);

    public async Task<IEnumerable<ReviewResponseDto>> GetByCustomerAsync(long customerProfileId)
        => (await WithIncludes().Where(r => r.CustomerProfileId == customerProfileId).ToListAsync()).Select(MapToDto);

    public async Task<ReviewResponseDto?> GetByIdAsync(long id)
    {
        var r = await WithIncludes().FirstOrDefaultAsync(r => r.Id == id);
        return r is null ? null : MapToDto(r);
    }

    public async Task<Review?> FindByIdAsync(long id)
        => await context.Reviews.FindAsync(id);

    public async Task<Review> AddAsync(Review review)
    {
        context.Reviews.Add(review);
        await context.SaveChangesAsync();
        return review;
    }

    public async Task<bool> ExistsAsync(long id)
        => await context.Reviews.AnyAsync(r => r.Id == id);
}
