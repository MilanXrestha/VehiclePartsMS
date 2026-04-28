using VehiclePartsMS.Application.DTOs;
using VehiclePartsMS.Application.Interfaces.IRepositories;
using VehiclePartsMS.Application.Interfaces.IServices;
using VehiclePartsMS.Domain.Models;

namespace VehiclePartsMS.Infrastructure.Services;

public class PartRequestService(IPartRequestRepository repo) : IPartRequestService
{
    public Task<IEnumerable<PartRequestResponseDto>> GetAllAsync() => repo.GetAllAsync();

    public Task<IEnumerable<PartRequestResponseDto>> GetByCustomerAsync(long customerProfileId)
        => repo.GetByCustomerAsync(customerProfileId);

    public Task<PartRequestResponseDto?> GetByIdAsync(long id) => repo.GetByIdAsync(id);

    public async Task<PartRequestResponseDto> CreateAsync(PartRequestCreateDto dto)
    {
        var request = new PartRequest
        {
            CustomerProfileId = dto.CustomerProfileId,
            PartName          = dto.PartName,
            Description       = dto.Description,
            RequestDate       = DateTime.UtcNow,
            Status            = PartRequestStatus.Pending
        };
        var created = await repo.AddAsync(request);
        return (await repo.GetByIdAsync(created.Id))!;
    }

    public async Task UpdateStatusAsync(long id, PartRequestUpdateDto dto)
    {
        var existing = await repo.FindByIdAsync(id)
            ?? throw new KeyNotFoundException($"Part request {id} not found.");

        existing.Status = dto.Status;
        await repo.UpdateAsync(existing);
    }
}
