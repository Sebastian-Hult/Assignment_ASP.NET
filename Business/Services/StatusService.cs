using Data.Entities;
using Data.Interfaces;

namespace Business.Services;

public interface IStatusService
{
    Task<StatusEntity?> GetStatusByIdAsync(int id);
    Task<StatusEntity?> GetStatusByNameAsync(string statusName);
    Task<IEnumerable<StatusEntity>> GetStatusesAsync();
}

public class StatusService(IStatusRepository statusRepository) : IStatusService
{
    private readonly IStatusRepository _statusRepository = statusRepository;

    public async Task<IEnumerable<StatusEntity>> GetStatusesAsync()
    {
        var result = await _statusRepository.GetAllAsync();
        return result;
    }

    public async Task<StatusEntity?> GetStatusByNameAsync(string statusName)
    {
        var result = await _statusRepository.GetAsync(x => x.StatusName == statusName);
        return result;
    }

    public async Task<StatusEntity?> GetStatusByIdAsync(int id)
    {
        var result = await _statusRepository.GetAsync(x => x.Id == id);
        return result;
    }
}
