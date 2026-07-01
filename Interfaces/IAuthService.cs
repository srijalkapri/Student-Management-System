using CRUD.DTOs;
using CRUD.Responses;

namespace CRUD.Interfaces
{
    public interface IAuthService
    {
        Task<ServiceResponse<LoginResponseDto>> Login(LoginRequestDto loginRequest);
        Task<ServiceResponse<UserDto>> GetCurrentUser(int userId);
    }
}
