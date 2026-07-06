using CRUD.Application.DTOs;
using CRUD.Application.Responses;

namespace CRUD.Application.Interfaces
{
    public interface IAuthService
    {
        Task<ServiceResponse<LoginResponseDto>> Login(LoginRequestDto loginRequest);
        Task<ServiceResponse<UserDto>> GetCurrentUser(int userId);
        Task<ServiceResponse<string>> Register(RegisterRequestDto registerRequest);
        Task<ServiceResponse<List<PendingUserResponseDto>>> GetPendingUsers();
        Task<ServiceResponse<string>> ApproveUser(int userId, ApproveUserRequestDto approveRequest, int approvedByUserId);
        Task<ServiceResponse<string>> RejectUser(int userId);
    }
}
