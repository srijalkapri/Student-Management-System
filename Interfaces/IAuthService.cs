using CRUD.DTOs;
using CRUD.Responses;

namespace CRUD.Interfaces
{
    public interface IAuthService
    {
        Task<ServiceResponse<LoginResponseDto>> LoginAsync(LoginRequestDto loginRequest);
        Task<ServiceResponse<UserDto>> GetCurrentUserAsync(int userId);
    }
}
