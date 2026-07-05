using CRUD.Application.DTOs;
using CRUD.Application.Responses;

namespace CRUD.Application.Interfaces
{
    public interface IAuthService
    {
        Task<ServiceResponse<LoginResponseDto>> Login(LoginRequestDto loginRequest);
        Task<ServiceResponse<UserDto>> GetCurrentUser(int userId);
    }
}
