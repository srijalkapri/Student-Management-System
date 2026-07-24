using CRUD.Application.DTOs;
using CRUD.Application.Interfaces;
using CRUD.Application.Responses;
using CRUD.Domain.Models;
using Microsoft.Extensions.Logging;

namespace CRUD.Application.Services
{
    public class ReExamService : IReExamService
    {
        public const decimal PassPercentage = 40m;
        public const int MaxAttempts = 1;

        private readonly IReExamRepository _reExamRepository;
        private readonly IStudentRepository _studentRepository;
        private readonly ITeacherRepository _teacherRepository;
        private readonly ILogger<ReExamService> _logger;

        public ReExamService(
            IReExamRepository reExamRepository,
            IStudentRepository studentRepository,
            ITeacherRepository teacherRepository,
            ILogger<ReExamService> logger)
        {
            _reExamRepository = reExamRepository;
            _studentRepository = studentRepository;
            _teacherRepository = teacherRepository;
            _logger = logger;
        }

        public async Task<ServiceResponse<string>> Apply(int userId, ApplyReExamRequestDto request)
        {
            var response = new ServiceResponse<string>();
            var student = await _studentRepository.GetStudentEntityByUserId(userId);
            if (student == null)
            {
                response.Success = false;
                response.Message = "No student profile is linked to this user account.";
                return response;
            }

            var item = await _reExamRepository.GetApprovedResultItem(student.Id, request.ExamSessionId);
            if (item == null)
            {
                response.Success = false;
                response.Message = "No approved result found for this exam session.";
                return response;
            }

            if (!IsEligibleForReExam(item))
            {
                response.Success = false;
                response.Message = "You are not eligible for re-exam. Only absent or failed (below 40%) results can apply.";
                return response;
            }

            if (await _reExamRepository.HasOpenRequest(student.Id, request.ExamSessionId))
            {
                response.Success = false;
                response.Message = "You already have an open re-exam request for this subject.";
                return response;
            }

            var completed = await _reExamRepository.CountCompletedAttempts(student.Id, request.ExamSessionId);
            if (completed >= MaxAttempts)
            {
                response.Success = false;
                response.Message = "Maximum re-exam attempts reached for this subject.";
                return response;
            }

            var entity = new ReExamRequest
            {
                StudentId = student.Id,
                ExamSessionId = request.ExamSessionId,
                OriginalResultItemId = item.Id,
                AttemptNumber = completed + 1,
                StudentReason = string.IsNullOrWhiteSpace(request.Reason) ? null : request.Reason.Trim(),
                Status = ReExamRequestStatus.Requested,
                CreatedAtUtc = DateTime.UtcNow,
                UpdatedAtUtc = DateTime.UtcNow
            };

            var id = await _reExamRepository.Create(entity);
            _logger.LogInformation(
                "Re-exam requested. ReExamId={ReExamId}, StudentId={StudentId}, ExamSessionId={ExamSessionId}",
                id, student.Id, request.ExamSessionId);

            response.Data = "Re-exam request submitted successfully.";
            response.Message = "Success";
            return response;
        }

        public async Task<ServiceResponse<List<ReExamRequestDto>>> GetMyRequests(int userId)
        {
            var response = new ServiceResponse<List<ReExamRequestDto>>();
            var student = await _studentRepository.GetStudentEntityByUserId(userId);
            if (student == null)
            {
                response.Success = false;
                response.Message = "No student profile is linked to this user account.";
                return response;
            }

            response.Data = await _reExamRepository.GetByStudent(student.Id);
            response.Message = "Re-exam requests retrieved successfully.";
            return response;
        }

        public async Task<ServiceResponse<List<ReExamRequestDto>>> GetPendingRequestApprovals()
        {
            return new ServiceResponse<List<ReExamRequestDto>>
            {
                Data = await _reExamRepository.GetPendingRequestApprovals(),
                Message = "Pending re-exam request approvals retrieved successfully."
            };
        }

        public async Task<ServiceResponse<List<ReExamRequestDto>>> GetPendingMarksApprovals()
        {
            return new ServiceResponse<List<ReExamRequestDto>>
            {
                Data = await _reExamRepository.GetPendingMarksApprovals(),
                Message = "Pending re-exam marks approvals retrieved successfully."
            };
        }

        public async Task<ServiceResponse<ReExamRequestDto>> GetById(int id)
        {
            var response = new ServiceResponse<ReExamRequestDto>();
            var dto = await _reExamRepository.GetDtoById(id);
            if (dto == null)
            {
                response.Success = false;
                response.Message = "Re-exam request not found.";
                return response;
            }

            response.Data = dto;
            response.Message = "Re-exam request retrieved successfully.";
            return response;
        }

        public async Task<ServiceResponse<string>> ApproveRequest(int id, int adminUserId, string? comment)
        {
            return await ReviewRequest(id, adminUserId, comment, approve: true);
        }

        public async Task<ServiceResponse<string>> RejectRequest(int id, int adminUserId, string? comment)
        {
            if (string.IsNullOrWhiteSpace(comment))
            {
                return new ServiceResponse<string>
                {
                    Success = false,
                    Message = "Reject comment is required."
                };
            }

            return await ReviewRequest(id, adminUserId, comment, approve: false);
        }

        public async Task<ServiceResponse<List<ReExamRequestDto>>> GetTeacherReExams(int userId)
        {
            var response = new ServiceResponse<List<ReExamRequestDto>>();
            var teacher = await _teacherRepository.GetTeacherEntityByUserId(userId);
            if (teacher == null)
            {
                response.Success = false;
                response.Message = "No teacher profile is linked to this user account.";
                return response;
            }

            response.Data = await _reExamRepository.GetForTeacher(teacher.Id);
            response.Message = "Teacher re-exam requests retrieved successfully.";
            return response;
        }

        public async Task<ServiceResponse<string>> SubmitMarks(int userId, int reExamId, TeacherSubmitReExamMarksDto request)
        {
            var response = new ServiceResponse<string>();
            var teacher = await _teacherRepository.GetTeacherEntityByUserId(userId);
            if (teacher == null)
            {
                response.Success = false;
                response.Message = "No teacher profile is linked to this user account.";
                return response;
            }

            var entity = await _reExamRepository.GetById(reExamId);
            if (entity == null)
            {
                response.Success = false;
                response.Message = "Re-exam request not found.";
                return response;
            }

            if (entity.Status != ReExamRequestStatus.Approved && entity.Status != ReExamRequestStatus.MarksRejected)
            {
                response.Success = false;
                response.Message = "Marks can only be submitted for approved or marks-rejected re-exam requests.";
                return response;
            }

            if (!await _reExamRepository.IsTeacherAssignedToSession(teacher.Id, entity.ExamSessionId))
            {
                response.Success = false;
                response.Message = "You are not assigned to this exam session.";
                return response;
            }

            if (request.TotalMarks <= 0)
            {
                response.Success = false;
                response.Message = "Total marks must be greater than zero.";
                return response;
            }

            if (!request.IsAbsent)
            {
                if (request.MarksObtained == null)
                {
                    response.Success = false;
                    response.Message = "Marks obtained is required when student is not absent.";
                    return response;
                }

                if (request.MarksObtained < 0 || request.MarksObtained > request.TotalMarks)
                {
                    response.Success = false;
                    response.Message = "Marks obtained must be between 0 and total marks.";
                    return response;
                }
            }

            entity.TeacherId = teacher.Id;
            entity.IsAbsent = request.IsAbsent;
            entity.MarksObtained = request.IsAbsent ? null : request.MarksObtained;
            entity.TotalMarks = request.TotalMarks;
            entity.MarksRemarks = string.IsNullOrWhiteSpace(request.Remarks) ? null : request.Remarks.Trim();
            entity.MarksSubmittedAtUtc = DateTime.UtcNow;
            entity.MarksReviewedAtUtc = null;
            entity.MarksReviewedByUserId = null;
            entity.MarksReviewComment = null;
            entity.Status = ReExamRequestStatus.MarksSubmitted;

            await _reExamRepository.Update(entity);

            _logger.LogInformation(
                "Re-exam marks submitted. ReExamId={ReExamId}, TeacherId={TeacherId}",
                entity.Id, teacher.Id);

            response.Data = "Re-exam marks submitted for admin approval.";
            response.Message = "Success";
            return response;
        }

        public async Task<ServiceResponse<string>> ApproveMarks(int id, int adminUserId, string? comment)
        {
            return await ReviewMarks(id, adminUserId, comment, approve: true);
        }

        public async Task<ServiceResponse<string>> RejectMarks(int id, int adminUserId, string? comment)
        {
            if (string.IsNullOrWhiteSpace(comment))
            {
                return new ServiceResponse<string>
                {
                    Success = false,
                    Message = "Reject comment is required."
                };
            }

            return await ReviewMarks(id, adminUserId, comment, approve: false);
        }

        public static bool IsEligibleForReExam(ExamResultItem item)
        {
            if (item.IsAbsent)
            {
                return true;
            }

            if (item.TotalMarks <= 0 || item.MarksObtained == null)
            {
                return false;
            }

            var percentage = (item.MarksObtained.Value / item.TotalMarks) * 100m;
            return percentage < PassPercentage;
        }

        private async Task<ServiceResponse<string>> ReviewRequest(int id, int adminUserId, string? comment, bool approve)
        {
            var response = new ServiceResponse<string>();
            var entity = await _reExamRepository.GetById(id);
            if (entity == null)
            {
                response.Success = false;
                response.Message = "Re-exam request not found.";
                return response;
            }

            if (entity.Status != ReExamRequestStatus.Requested)
            {
                response.Success = false;
                response.Message = "Only requested re-exams can be reviewed.";
                return response;
            }

            entity.Status = approve ? ReExamRequestStatus.Approved : ReExamRequestStatus.Rejected;
            entity.ReviewedByUserId = adminUserId;
            entity.ReviewedAtUtc = DateTime.UtcNow;
            entity.AdminComment = string.IsNullOrWhiteSpace(comment) ? null : comment.Trim();
            await _reExamRepository.Update(entity);

            response.Data = approve
                ? "Re-exam request approved successfully."
                : "Re-exam request rejected successfully.";
            response.Message = "Success";
            return response;
        }

        private async Task<ServiceResponse<string>> ReviewMarks(int id, int adminUserId, string? comment, bool approve)
        {
            var response = new ServiceResponse<string>();
            var entity = await _reExamRepository.GetById(id);
            if (entity == null)
            {
                response.Success = false;
                response.Message = "Re-exam request not found.";
                return response;
            }

            if (entity.Status != ReExamRequestStatus.MarksSubmitted)
            {
                response.Success = false;
                response.Message = "Only submitted re-exam marks can be reviewed.";
                return response;
            }

            entity.Status = approve ? ReExamRequestStatus.MarksApproved : ReExamRequestStatus.MarksRejected;
            entity.MarksReviewedByUserId = adminUserId;
            entity.MarksReviewedAtUtc = DateTime.UtcNow;
            entity.MarksReviewComment = string.IsNullOrWhiteSpace(comment) ? null : comment.Trim();
            await _reExamRepository.Update(entity);

            response.Data = approve
                ? "Re-exam marks approved successfully."
                : "Re-exam marks rejected successfully.";
            response.Message = "Success";
            return response;
        }
    }
}
