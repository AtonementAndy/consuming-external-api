using CallWebApi.Models;

namespace CallWebApi.Services
{
    public interface IUserService
    {
        Task<IEnumerable<User>> GetUsersAsync(CancellationToken cancellationToken = default);
    }
}