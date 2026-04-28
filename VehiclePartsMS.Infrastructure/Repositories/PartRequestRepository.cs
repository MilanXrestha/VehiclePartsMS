using Microsoft.EntityFrameworkCore;
using VehiclePartsMS.Application.DTOs;
using VehiclePartsMS.Application.Interfaces.IRepositories;
using VehiclePartsMS.Domain.Models;
using VehiclePartsMS.Infrastructure.Persistance;

namespace VehiclePartsMS.Infrastructure.Repositories;

public class PartRequestRepository(ApplicationDbContext context) : IPartRequestRepository
{
    private static PartRequestResponseDto MapToDto(PartRequest pr) =>
        new(pr.Id, pr.CustomerProfileId,
            pr.CustomerProfile.User.FirstName + " " + pr.CustomerProfile.User.LastName,
            pr.PartName, pr.Description, pr.RequestDate, pr.Status);

    private IQueryable<PartRequest> WithIncludes()
        => context.PartRequests
            .Include(pr => pr.CustomerProfile).ThenInclude(cp => cp.User);

    public async Task<IEnumerable<PartRequestResponseDto>> GetAllAsync()
        => (await WithIncludes().ToListAsync()).Select(MapToDto);

    public async Task<IEnumerable<PartRequestResponseDto>> GetByCustomerAsync(long customerProfileId)
        => (await WithIncludes().Where(pr => pr.CustomerProfileId == customerProfileId).ToListAsync()).Select(MapToDto);

    public async Task<PartRequestResponseDto?> GetByIdAsync(long id)
    {
        var pr = await WithIncludes().FirstOrDefaultAsync(pr => pr.Id == id);
        return pr is null ? null : MapToDto(pr);
    }

    public async Task<PartRequest?> FindByIdAsync(long id)
        => await context.PartRequests.FindAsync(id);

    public async Task<PartRequest> AddAsync(PartRequest request)
    {
        context.PartRequests.Add(request);
        await context.SaveChangesAsync();
        return request;
    }

    public async Task UpdateAsync(PartRequest request)
    {
        context.PartRequests.Update(request);
        await context.SaveChangesAsync();
    }

    public async Task<bool> ExistsAsync(long id)
        => await context.PartRequests.AnyAsync(pr => pr.Id == id);
}
