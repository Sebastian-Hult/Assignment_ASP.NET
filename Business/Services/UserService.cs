using Data.Entities;
using Data.Repositories;

namespace Business.Services;

public interface IUserService
{
    Task<IEnumerable<UserEntity>> GetUsersAsync();
}

public class UserService(IUserRepository userRepository) : IUserService
{
    private readonly IUserRepository _userRepository = userRepository;

    public async Task<IEnumerable<UserEntity>> GetUsersAsync()
    {
        var result = await _userRepository.GetAllAsync();
        return result;
    }
}
