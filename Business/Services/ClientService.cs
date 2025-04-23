using Data.Entities;
using Data.Interfaces;

namespace Business.Services;

public interface IClientService
{
    Task<IEnumerable<ClientEntity>> GetClientsAsync();
}

public class ClientService(IClientRepository clientRepository) : IClientService
{
    private readonly IClientRepository _clientRepository = clientRepository;

    public async Task<IEnumerable<ClientEntity>> GetClientsAsync()
    {
        var result = await _clientRepository.GetAllAsync();
        return result;
    }
}
