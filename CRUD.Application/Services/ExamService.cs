using CRUD.Application.DTOs;
using CRUD.Application.Interfaces;
using CRUD.Domain.Models;
using CRUD.Application.Responses;

namespace CRUD.Application.Services
{
    public class ExamService : IExamService
    {
        private readonly IExamRepository _examRepository;
        private readonly IGradeRepository _gradeRepository;
        private readonly IGradeSubjectRepository _gradeSubjectRepository;
        private readonly ITeacherRepository _teacherRepository;

        public ExamService(
            IExamRepository examRepository,
            IGradeRepository gradeRepository,
            IGradeSubjectRepository gradeSubjectRepository,
            ITeacherRepository teacherRepository)
        {
            _examRepository = examRepository;
            _gradeRepository = gradeRepository;
            _gradeSubjectRepository = gradeSubjectRepository;
            _teacherRepository = teacherRepository;
        }

        public async Task<ServiceResponse<List<ExamScheduleResponseDto>>> GetAllSchedules()
        {
            var response = new ServiceResponse<List<ExamScheduleResponseDto>>();
            var schedules = await _examRepository.GetAllSchedules();
            response.Data = schedules;
            response.Message = "All exam schedules retrieved successfully.";
            return response;
        }

        public async Task<ServiceResponse<ExamScheduleResponseDto?>> GetScheduleById(int id)
        {
            var response = new ServiceResponse<ExamScheduleResponseDto?>();
            var schedule = await _examRepository.GetScheduleById(id);
            if (schedule == null)
            {
                response.Success = false;
                response.Message = $"Exam schedule with ID {id} not found.";
                return response;
            }

            response.Data = schedule;
            response.Message = "Exam schedule retrieved successfully.";
            return response;
        }

        public async Task<ServiceResponse<List<ExamScheduleResponseDto>>> GetSchedulesByGrade(int gradeId)
        {
            var response = new ServiceResponse<List<ExamScheduleResponseDto>>();
            var grade = await _gradeRepository.GetGradeById(gradeId);
            if (grade == null)
            {
                response.Success = false;
                response.Message = $"Grade with ID {gradeId} not found.";
                return response;
            }

            var schedules = await _examRepository.GetSchedulesByGrade(gradeId);
            response.Data = schedules;
            response.Message = $"Exam schedules for grade {gradeId} retrieved successfully.";
            return response;
        }

        public async Task<ServiceResponse<int>> CreateSchedule(ExamScheduleRequestDto request)
        {
            var response = new ServiceResponse<int>();

            var grade = await _gradeRepository.GetGradeById(request.GradeId);
            if (grade == null)
            {
                response.Success = false;
                response.Message = $"Grade with ID {request.GradeId} not found.";
                return response;
            }

            var examSchedule = new ExamSchedule
            {
                GradeId = request.GradeId,
                Title = request.Title,
                AcademicYear = request.AcademicYear,
                Status = ExamScheduleStatus.Draft,
                CreatedAt = DateTime.UtcNow
            };

            List<ExamSession>? sessions = null;
            if (request.AutoGenerateSessions)
            {
                var gradeSubjects = await _gradeSubjectRepository.GetGradeSubjectsByGradeIdEntities(request.GradeId);

                sessions = gradeSubjects.Select(gs => new ExamSession
                {
                    GradeSubjectId = gs.Id
                }).ToList();
            }

            var id = await _examRepository.CreateSchedule(examSchedule, sessions);
            response.Data = id;
            response.Message = "Exam schedule created successfully.";
            return response;
        }

        public async Task<ServiceResponse<int>> UpdateSchedule(int id, UpdateExamScheduleRequestDto request)
        {
            var response = new ServiceResponse<int>();

            var existingSchedule = await _examRepository.GetScheduleEntityById(id);
            if (existingSchedule == null)
            {
                response.Success = false;
                response.Message = $"Exam schedule with ID {id} not found.";
                return response;
            }

            if (existingSchedule.Status == ExamScheduleStatus.Published)
            {
                // Allow unpublish only
                if (request.Status == ExamScheduleStatus.Draft)
                {
                    existingSchedule.Status = ExamScheduleStatus.Draft;
                    var result = await _examRepository.UpdateSchedule(existingSchedule);
                    response.Data = result;
                    response.Message = "Exam schedule unpublished successfully.";
                    return response;
                }

                response.Success = false;
                response.Message = "Cannot edit a published exam schedule. Unpublish it first.";
                return response;
            }

            // Draft schedules: allow full update
            if (request.Status == ExamScheduleStatus.Published)
            {
                var sessions = await _examRepository.GetSessionsByScheduleId(id);
                if (sessions.Any(es => !es.ExamDate.HasValue))
                {
                    response.Success = false;
                    response.Message = "Cannot publish schedule: All sessions must have an exam date.";
                    return response;
                }
            }

            existingSchedule.Title = request.Title;
            existingSchedule.AcademicYear = request.AcademicYear;
            existingSchedule.Status = request.Status;

            var updateResult = await _examRepository.UpdateSchedule(existingSchedule);
            response.Data = updateResult;
            response.Message = "Exam schedule updated successfully.";
            return response;
        }

        public async Task<ServiceResponse<int>> DeleteSchedule(int id)
        {
            var response = new ServiceResponse<int>();

            var existingSchedule = await _examRepository.GetScheduleEntityById(id);
            if (existingSchedule == null)
            {
                response.Success = false;
                response.Message = $"Exam schedule with ID {id} not found.";
                return response;
            }

            if (existingSchedule.Status == ExamScheduleStatus.Published)
            {
                response.Success = false;
                response.Message = "Cannot delete a published exam schedule. Unpublish it first.";
                return response;
            }

            var result = await _examRepository.DeleteSchedule(id);
            response.Data = result;
            response.Message = "Exam schedule deleted successfully.";
            return response;
        }

        public async Task<ServiceResponse<int>> UpdateSession(int id, ExamSessionRequestDto request)
        {
            var response = new ServiceResponse<int>();

            var existingSession = await _examRepository.GetSessionEntityById(id);
            if (existingSession == null)
            {
                response.Success = false;
                response.Message = $"Exam session with ID {id} not found.";
                return response;
            }

            var schedule = await _examRepository.GetScheduleEntityById(existingSession.ExamScheduleId);
            if (schedule == null)
            {
                response.Success = false;
                response.Message = "Exam schedule not found.";
                return response;
            }

            if (schedule.Status == ExamScheduleStatus.Published)
            {
                response.Success = false;
                response.Message = "Cannot edit a session in a published schedule.";
                return response;
            }

            if (request.InvigilatorTeacherId.HasValue)
            {
                var teacherExists = await _teacherRepository.TeacherExists(request.InvigilatorTeacherId.Value);
                if (!teacherExists)
                {
                    response.Success = false;
                    response.Message = $"Invigilator teacher with ID {request.InvigilatorTeacherId} not found.";
                    return response;
                }
            }

            var updatedSession = new ExamSession
            {
                Id = id,
                ExamDate = request.ExamDate,
                StartTime = request.StartTime,
                EndTime = request.EndTime,
                InvigilatorTeacherId = request.InvigilatorTeacherId,
                Notes = request.Notes
            };

            var result = await _examRepository.UpdateSession(updatedSession);
            response.Data = result;
            response.Message = "Exam session updated successfully.";
            return response;
        }

        public async Task<ServiceResponse<int>> AddSession(AddExamSessionRequestDto request)
        {
            var response = new ServiceResponse<int>();

            var schedule = await _examRepository.GetScheduleEntityById(request.ExamScheduleId);
            if (schedule == null)
            {
                response.Success = false;
                response.Message = $"Exam schedule with ID {request.ExamScheduleId} not found.";
                return response;
            }

            if (schedule.Status == ExamScheduleStatus.Published)
            {
                response.Success = false;
                response.Message = "Cannot add a session to a published schedule.";
                return response;
            }

            var gradeSubjectExists = await _gradeSubjectRepository.GradeSubjectExistsForGrade(request.GradeSubjectId, schedule.GradeId);
            if (!gradeSubjectExists)
            {
                response.Success = false;
                response.Message = $"GradeSubject with ID {request.GradeSubjectId} not found for this grade.";
                return response;
            }

            if (request.InvigilatorTeacherId.HasValue)
            {
                var teacherExists = await _teacherRepository.TeacherExists(request.InvigilatorTeacherId.Value);
                if (!teacherExists)
                {
                    response.Success = false;
                    response.Message = $"Invigilator teacher with ID {request.InvigilatorTeacherId} not found.";
                    return response;
                }
            }

            var session = new ExamSession
            {
                ExamScheduleId = request.ExamScheduleId,
                GradeSubjectId = request.GradeSubjectId,
                ExamDate = request.ExamDate,
                StartTime = request.StartTime,
                EndTime = request.EndTime,
                InvigilatorTeacherId = request.InvigilatorTeacherId,
                Notes = request.Notes
            };

            var id = await _examRepository.AddSession(session);
            response.Data = id;
            response.Message = "Exam session added successfully.";
            return response;
        }

        public async Task<ServiceResponse<int>> DeleteSession(int id)
        {
            var response = new ServiceResponse<int>();

            var session = await _examRepository.GetSessionEntityById(id);
            if (session == null)
            {
                response.Success = false;
                response.Message = $"Exam session with ID {id} not found.";
                return response;
            }

            var schedule = await _examRepository.GetScheduleEntityById(session.ExamScheduleId);
            if (schedule == null)
            {
                response.Success = false;
                response.Message = "Exam schedule not found.";
                return response;
            }

            if (schedule.Status == ExamScheduleStatus.Published)
            {
                response.Success = false;
                response.Message = "Cannot delete a session in a published schedule.";
                return response;
            }

            var result = await _examRepository.DeleteSession(id);
            response.Data = result;
            response.Message = "Exam session deleted successfully.";
            return response;
        }

        public async Task<ServiceResponse<bool>> BulkUpdateSessions(BulkUpdateSessionsRequestDto request)
        {
            var response = new ServiceResponse<bool>();

            var schedule = await _examRepository.GetScheduleEntityById(request.ExamScheduleId);
            if (schedule == null)
            {
                response.Success = false;
                response.Message = $"Exam schedule with ID {request.ExamScheduleId} not found.";
                return response;
            }

            if (schedule.Status == ExamScheduleStatus.Published)
            {
                response.Success = false;
                response.Message = "Cannot edit sessions in a published schedule.";
                return response;
            }

            var existingSessions = (await _examRepository.GetSessionsByScheduleId(request.ExamScheduleId))
                .ToDictionary(es => es.Id);

            var sessionsToUpdate = new List<ExamSession>();
            foreach (var dto in request.Sessions)
            {
                if (!existingSessions.TryGetValue(dto.Id, out var session))
                {
                    response.Success = false;
                    response.Message = $"Exam session with ID {dto.Id} not found in this schedule.";
                    return response;
                }

                if (dto.InvigilatorTeacherId.HasValue)
                {
                    var teacherExists = await _teacherRepository.TeacherExists(dto.InvigilatorTeacherId.Value);
                    if (!teacherExists)
                    {
                        response.Success = false;
                        response.Message = $"Invigilator teacher with ID {dto.InvigilatorTeacherId} not found.";
                        return response;
                    }
                }

                session.ExamDate = dto.ExamDate;
                session.StartTime = dto.StartTime;
                session.EndTime = dto.EndTime;
                session.InvigilatorTeacherId = dto.InvigilatorTeacherId;
                session.Notes = dto.Notes;

                sessionsToUpdate.Add(session);
            }

            await _examRepository.BulkUpdateSessions(sessionsToUpdate);
            response.Data = true;
            response.Message = "Bulk update of exam sessions completed successfully.";
            return response;
        }

        public async Task<ServiceResponse<PagedResult<ExamScheduleResponseDto>>> GetSchedulesPaged(PaginationParameters parameters)
        {
            var response = new ServiceResponse<PagedResult<ExamScheduleResponseDto>>();

            var pagedResult = await _examRepository.GetSchedulesPaged(parameters);

            response.Data = pagedResult;
            response.Message = "Exam schedules retrieved successfully.";
            return response;
        }

        public async Task<ServiceResponse<PagedResult<ExamScheduleResponseDto>>> GetSchedulesByGradePaged(int gradeId, PaginationParameters parameters)
        {
            var response = new ServiceResponse<PagedResult<ExamScheduleResponseDto>>();

            // Check if grade exists
            var grade = await _gradeRepository.GetGradeById(gradeId);
            if (grade == null)
            {
                response.Success = false;
                response.Message = $"Grade with ID {gradeId} not found.";
                return response;
            }

            var pagedResult = await _examRepository.GetSchedulesByGradePaged(gradeId, parameters);

            response.Data = pagedResult;
            response.Message = $"Exam schedules for grade {gradeId} retrieved successfully.";
            return response;
        }
    }
}
