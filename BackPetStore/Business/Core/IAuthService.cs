using Entity.DTOs;

namespace Business.Interfaces
{
    public interface IAuthService
    {
        Task<string?> AuthenticateAsync(LoginDto loginDto);
    }
}
