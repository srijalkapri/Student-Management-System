using CRUD.Application.DTOs;
using CRUD.Application.Interfaces;
using CRUD.Application.Responses;
using CRUD.Domain.Models;
using Microsoft.Extensions.Logging;

namespace CRUD.Application.Services
{
    public class ExamResultService : IExamResultService
    {
        private readonly IExamResultRepository _examResultRepository;
        private readonly ITeacherRepository _teacherRepository;
        private readonly IStudentRepository _studentRepository;
        private readonly IExamRepository _examRepository;
        private readonly ILogger<ExamResultService> _logger;

        public ExamResultService(
            IExamResultRepository examResultRepository,
            ITeacherRepository teacherRepository,
            IStudentRepository studentRepository,
            IExamRepository examRepository,
            ILogger<ExamResultService> logger)
        {
            _examResultRepository = examResultRepository;
            _teacherRepository = teacherRepository;
            _studentRepository = studentRepository;
            _examRepository = examRepository;
            _logger = logger;
        }

        public async Task<ServiceResponse<List<TeacherExamSessionDto>>> GetTeacherExamSessions(int userId)
        {
            var response = new ServiceResponse<List<TeacherExamSessionDto>>();
            var teacher = await _teacherRepository.GetTeacherEntityByUserId(userId);
            if (teacher == null)
            {
                response.Success = false;
                response.Message = "No teacher profile is linked to this user account.";
                return response;
            }

            response.Data = await _examResultRepository.GetTeacherExamSessions(teacher.Id);
            response.Message = "Teacher exam sessions retrieved successfully.";
            return response;
        }

        public async Task<ServiceResponse<TeacherExamResultBatchDto>> GetTeacherExamResults(int userId, int examSessionId)
        {
            var response = new ServiceResponse<TeacherExamResultBatchDto>();
            var teacher = await _teacherRepository.GetTeacherEntityByUserId(userId);
            if (teacher == null)
            {
                response.Success = false;
                response.Message = "No teacher profile is linked to this user account.";
                return response;
            }

            var existingBatch = await _examResultRepository.GetTeacherBatchBySession(teacher.Id, examSessionId);
            if (existingBatch != null)
            {
                response.Data = existingBatch;
                response.Message = "Teacher exam result batch retrieved successfully.";
                return response;
            }

            var students = await _examResultRepository.GetStudentsByExamSession(examSessionId);
            if (students.Count == 0)
            {
                response.Success = false;
                response.Message = "Exam session not found or no students found for this schedule.";
                return response;
            }

            response.Data = new TeacherExamResultBatchDto
            {
                BatchId = 0,
                ExamSessionId = examSessionId,
                Status = ExamResultStatus.Draft.ToString(),
                Items = students.Select(s => new TeacherExamResultItemDto
                {
                    StudentId = s.Id,
                    StudentName = s.Name,
                    MarksObtained = null,
                    TotalMarks = 100,
                    IsAbsent = false,
                    Remarks = null
                }).ToList()
            };
            response.Message = "No saved batch found. Draft template returned.";
            return response;
        }

        public Task<ServiceResponse<string>> SaveDraft(int userId, TeacherSaveExamResultsRequestDto request)
        {
            return SaveInternal(userId, request, false);
        }

        public Task<ServiceResponse<string>> SubmitForApproval(int userId, TeacherSaveExamResultsRequestDto request)
        {
            return SaveInternal(userId, request, true);
        }

        public async Task<ServiceResponse<List<AdminPendingExamResultDto>>> GetPendingApprovals()
        {
            var response = new ServiceResponse<List<AdminPendingExamResultDto>>
            {
                Data = await _examResultRepository.GetPendingBatches(),
                Message = "Pending result approvals retrieved successfully."
            };

            return response;
        }

        public async Task<ServiceResponse<AdminExamResultReviewDto>> GetBatchReview(int batchId)
        {
            var response = new ServiceResponse<AdminExamResultReviewDto>();
            var review = await _examResultRepository.GetBatchReviewById(batchId);
            if (review == null)
            {
                response.Success = false;
                response.Message = "Result batch not found.";
                return response;
            }

            response.Data = review;
            response.Message = "Result batch review data retrieved successfully.";
            return response;
        }

        public async Task<ServiceResponse<string>> ApproveBatch(int batchId, int adminUserId, string? comment)
        {
            var response = new ServiceResponse<string>();
            var batch = await _examResultRepository.GetBatchEntityById(batchId);
            if (batch == null)
            {
                response.Success = false;
                response.Message = "Result batch not found.";
                return response;
            }

            if (batch.Status != ExamResultStatus.PendingApproval)
            {
                response.Success = false;
                response.Message = "Only pending batches can be approved.";
                return response;
            }

            batch.Status = ExamResultStatus.Approved;
            batch.ReviewedByUserId = adminUserId;
            batch.ReviewedAtUtc = DateTime.UtcNow;
            batch.ReviewComment = comment;
            await _examResultRepository.UpdateBatch(batch);

            _logger.LogInformation(
                "Exam result batch approved. BatchId={BatchId}, ExamSessionId={ExamSessionId}, TeacherId={TeacherId}, AdminUserId={AdminUserId}",
                batch.Id, batch.ExamSessionId, batch.TeacherId, adminUserId);

            response.Data = "Result batch approved successfully.";
            response.Message = "Success";
            return response;
        }

        public async Task<ServiceResponse<string>> RejectBatch(int batchId, int adminUserId, string? comment)
        {
            var response = new ServiceResponse<string>();
            if (string.IsNullOrWhiteSpace(comment))
            {
                response.Success = false;
                response.Message = "Reject comment is required.";
                return response;
            }

            var batch = await _examResultRepository.GetBatchEntityById(batchId);
            if (batch == null)
            {
                response.Success = false;
                response.Message = "Result batch not found.";
                return response;
            }

            if (batch.Status != ExamResultStatus.PendingApproval)
            {
                response.Success = false;
                response.Message = "Only pending batches can be rejected.";
                return response;
            }

            batch.Status = ExamResultStatus.Rejected;
            batch.ReviewedByUserId = adminUserId;
            batch.ReviewedAtUtc = DateTime.UtcNow;
            batch.ReviewComment = comment?.Trim();
            await _examResultRepository.UpdateBatch(batch);

            _logger.LogWarning(
                "Exam result batch rejected. BatchId={BatchId}, ExamSessionId={ExamSessionId}, TeacherId={TeacherId}, AdminUserId={AdminUserId}",
                batch.Id, batch.ExamSessionId, batch.TeacherId, adminUserId);

            response.Data = "Result batch rejected successfully.";
            response.Message = "Success";
            return response;
        }

        public async Task<ServiceResponse<List<StudentExamResultScheduleDto>>> GetStudentResults(int userId, int? examScheduleId)
        {
            var response = new ServiceResponse<List<StudentExamResultScheduleDto>>();
            var student = await _studentRepository.GetStudentEntityByUserId(userId);
            if (student == null)
            {
                response.Success = false;
                response.Message = "No student profile is linked to this user account.";
                return response;
            }

            response.Data = await _examResultRepository.GetStudentApprovedResults(student.Id, examScheduleId);
            response.Message = "Student exam results retrieved successfully.";
            return response;
        }

        private async Task<ServiceResponse<string>> SaveInternal(
            int userId,
            TeacherSaveExamResultsRequestDto request,
            bool submit)
        {
            var response = new ServiceResponse<string>();
            var teacher = await _teacherRepository.GetTeacherEntityByUserId(userId);
            if (teacher == null)
            {
                response.Success = false;
                response.Message = "No teacher profile is linked to this user account.";
                return response;
            }

            var allowedSessionIds = (await _examResultRepository.GetTeacherExamSessions(teacher.Id))
                .Select(s => s.ExamSessionId)
                .ToHashSet();

            if (!allowedSessionIds.Contains(request.ExamSessionId))
            {
                response.Success = false;
                response.Message = "You are not assigned to this exam session.";
                return response;
            }

            var expectedStudentIds = (await _examResultRepository.GetStudentsByExamSession(request.ExamSessionId))
                .Select(s => s.Id)
                .ToHashSet();

            if (expectedStudentIds.Count == 0)
            {
                response.Success = false;
                response.Message = "No students found for this exam session.";
                return response;
            }

            var requestStudentIds = request.Items.Select(i => i.StudentId).ToList();
            if (requestStudentIds.Count != requestStudentIds.Distinct().Count())
            {
                response.Success = false;
                response.Message = "Duplicate student entries are not allowed.";
                return response;
            }

            if (requestStudentIds.Any(id => !expectedStudentIds.Contains(id)))
            {
                response.Success = false;
                response.Message = "One or more students are not part of this exam session grade.";
                return response;
            }

            var batch = await _examResultRepository.GetBatchEntityBySessionTeacher(request.ExamSessionId, teacher.Id);
            if (batch == null)
            {
                batch = new ExamResultBatch
                {
                    ExamSessionId = request.ExamSessionId,
                    TeacherId = teacher.Id,
                    Status = submit ? ExamResultStatus.PendingApproval : ExamResultStatus.Draft,
                    SubmittedAtUtc = submit ? DateTime.UtcNow : null,
                    CreatedAtUtc = DateTime.UtcNow,
                    UpdatedAtUtc = DateTime.UtcNow
                };
                batch.Id = await _examResultRepository.CreateBatch(batch);
            }
            else
            {
                if (batch.Status == ExamResultStatus.PendingApproval || batch.Status == ExamResultStatus.Approved)
                {
                    response.Success = false;
                    response.Message = "This result batch is locked. Ask admin to reject it before editing.";
                    return response;
                }

                batch.Status = submit ? ExamResultStatus.PendingApproval : ExamResultStatus.Draft;
                batch.SubmittedAtUtc = submit ? DateTime.UtcNow : null;
                batch.ReviewedAtUtc = null;
                batch.ReviewedByUserId = null;
                batch.ReviewComment = null;
                await _examResultRepository.UpdateBatch(batch);
            }

            var items = request.Items.Select(i => new ExamResultItem
            {
                ExamResultBatchId = batch.Id,
                StudentId = i.StudentId,
                MarksObtained = i.IsAbsent ? null : i.MarksObtained,
                TotalMarks = i.TotalMarks,
                IsAbsent = i.IsAbsent,
                Remarks = i.Remarks
            }).ToList();

            await _examResultRepository.ReplaceBatchItems(batch.Id, items);

            _logger.LogInformation(
                "Exam results saved by teacher. BatchId={BatchId}, ExamSessionId={ExamSessionId}, TeacherId={TeacherId}, Submitted={Submitted}, ItemCount={ItemCount}",
                batch.Id, request.ExamSessionId, teacher.Id, submit, items.Count);

            response.Data = submit
                ? "Results submitted for admin approval."
                : "Draft results saved successfully.";
            response.Message = "Success";
            return response;
        }
    }
}
