using Shared.Authentication.Entities;

namespace Shared.Authentication.Interfaces
{
    public interface ITokenRepository
    {
        Task SaveTokenAsync(UserToken userToken);
    }
}