using System.Globalization;
using Jalsa.Application.DTOs.Dashboard;
using Jalsa.Application.DTOs.PatientDashboard;
using Jalsa.Application.Interfaces.Repositories;
using Jalsa.Application.Interfaces.Services;
using Jalsa.Domain.Models.Chat;
using Jalsa.Domain.Models.Clinic;
using Jalsa.Domain.Models.System;
using Microsoft.EntityFrameworkCore;
using PatientEntity = Jalsa.Domain.Models.Patient.Patient;

namespace Jalsa.Infrastructure.Services;

public class PatientDashboardService : IPatientDashboardService
{
    private const string CrisisHotlineSettingKey = "CrisisHotline";
    private const string DefaultHotlineNumber = "08008880700";
    private const string DefaultHotlineLabel = "الخط الساخن للصحة النفسية";

    private readonly IPatientRepository _patientRepository;
    private readonly ISessionRepository _sessionRepository;
    private readonly IExerciseRepository _exerciseRepository;
    private readonly IAssessmentRepository _assessmentRepository;
    private readonly IUnitOfWork _unitOfWork;

    public PatientDashboardService(
        IPatientRepository patientRepository,
        ISessionRepository sessionRepository,
        IExerciseRepository exerciseRepository,
        IAssessmentRepository assessmentRepository,
        IUnitOfWork unitOfWork)
    {
        _patientRepository = patientRepository;
        _sessionRepository = sessionRepository;
        _exerciseRepository = exerciseRepository;
        _assessmentRepository = assessmentRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<PatientDashboardDto> GetDashboardAsync(Guid userId)
    {
        var patient = await ResolvePatientAsync(userId);
        var today = DateOnly.FromDateTime(DateTime.UtcNow);
        var sixMonthsAgo = today.AddMonths(-6);

        var upcomingSessions = await GetUpcomingSessionsAsync(patient.Id, today);
        var todaySessions = await GetTodaySessionsAsync(patient.Id, today);
        var todayExercises = await GetExercisesDueTodayAsync(patient.Id, today);

        var todayReminders = new List<TodayReminderDto>();
        todayReminders.AddRange(todaySessions.Select(s => new TodayReminderDto
        {
            Type = "Session",
            Title = $"جلسة اليوم - {s.SessionType ?? "علاجية"}",
            ReferenceId = s.Id
        }));
        todayReminders.AddRange(todayExercises.Select(e => new TodayReminderDto
        {
            Type = "Exercise",
            Title = e.Description ?? "تمرين اليوم",
            ReferenceId = e.Id
        }));

        return new PatientDashboardDto
        {
            PatientFirstName = GetFirstName(patient.FullName),
            UpcomingSessions = upcomingSessions,
            TodayReminders = todayReminders,
            AssignedExercises = await GetAssignedExercisesAsync(patient.Id, today),
            PendingAssessments = new List<PendingAssessmentDto>(),
            RecentConversations = await GetRecentConversationsAsync(patient.Id),
            ProgressOverview = await GetProgressOverviewAsync(patient.Id, sixMonthsAgo, today),
            Therapist = await GetTherapistInfoAsync(patient.TherapistId),
            CrisisSupport = await GetCrisisSupportInfoAsync()
        };
    }

    private async Task<List<UpcomingSessionDto>> GetUpcomingSessionsAsync(Guid patientId, DateOnly today)
    {
        return await _sessionRepository.Query()
            .AsNoTracking()
            .Where(s => s.PatientId == patientId && s.SessionDate >= today)
            .OrderBy(s => s.SessionDate)
            .Take(5)
            .Select(s => new UpcomingSessionDto
            {
                Id = s.Id,
                SessionDate = s.SessionDate,
                SessionType = s.SessionType,
                DurationMinutes = s.DurationMinutes,
                Status = s.Status
            })
            .ToListAsync();
    }

    private async Task<List<UpcomingSessionDto>> GetTodaySessionsAsync(Guid patientId, DateOnly today)
    {
        return await _sessionRepository.Query()
            .AsNoTracking()
            .Where(s => s.PatientId == patientId && s.SessionDate == today)
            .Select(s => new UpcomingSessionDto
            {
                Id = s.Id,
                SessionDate = s.SessionDate,
                SessionType = s.SessionType,
                DurationMinutes = s.DurationMinutes,
                Status = s.Status
            })
            .ToListAsync();
    }

    private async Task<List<AssignedExerciseSummaryDto>> GetExercisesDueTodayAsync(Guid patientId, DateOnly today)
    {
        return await _exerciseRepository.Query()
            .AsNoTracking()
            .Where(e => e.PatientId == patientId && e.Status != "Complete" && e.DueDate == today)
            .Select(e => new AssignedExerciseSummaryDto
            {
                Id = e.Id,
                Description = e.Description,
                Frequency = e.Frequency,
                DueDate = e.DueDate,
                Status = e.Status,
                IsOverdue = false
            })
            .ToListAsync();
    }

    private async Task<List<AssignedExerciseSummaryDto>> GetAssignedExercisesAsync(Guid patientId, DateOnly today)
    {
        var exercises = await _exerciseRepository.Query()
            .AsNoTracking()
            .Where(e => e.PatientId == patientId && e.Status != "Complete")
            .OrderBy(e => e.DueDate == null)
            .ThenBy(e => e.DueDate)
            .Take(10)
            .ToListAsync();

        return exercises.Select(e => new AssignedExerciseSummaryDto
        {
            Id = e.Id,
            Description = e.Description,
            Frequency = e.Frequency,
            DueDate = e.DueDate,
            Status = e.Status,
            IsOverdue = e.DueDate.HasValue && e.DueDate.Value < today
        }).ToList();
    }

    private async Task<List<RecentConversationDto>> GetRecentConversationsAsync(Guid patientId)
    {
        return await _unitOfWork.Repository<ChatConversation>().Query()
            .AsNoTracking()
            .Where(c => c.PatientId == patientId)
            .OrderByDescending(c => c.LastActivityAt ?? c.CreatedAt)
            .Take(3)
            .Select(c => new RecentConversationDto
            {
                ConversationId = c.Id,
                LastActivityAt = c.LastActivityAt,
                Status = c.Status
            })
            .ToListAsync();
    }

    private async Task<ProgressOverviewDto> GetProgressOverviewAsync(Guid patientId, DateOnly fromDate, DateOnly toDate)
    {
        var totalExercises = await _exerciseRepository.CountAsync(e => e.PatientId == patientId);
        var completedExercises = await _exerciseRepository.CountAsync(
            e => e.PatientId == patientId && e.Status == "Complete");
        var exerciseCompletionRate = totalExercises > 0
            ? (double)completedExercises / totalExercises * 100
            : 0;

        var recentScores = await _assessmentRepository.Query()
            .AsNoTracking()
            .Where(a => a.PatientId == patientId && a.TotalScore.HasValue)
            .OrderByDescending(a => a.AssessmentDate)
            .Take(2)
            .Select(a => a.TotalScore!.Value)
            .ToListAsync();

        var completedSessionsCount = await _sessionRepository.CountAsync(
            s => s.PatientId == patientId && s.Status == "Completed");

        return new ProgressOverviewDto
        {
            ExerciseCompletionRate = Math.Round(exerciseCompletionRate, 2),
            LatestAssessmentScore = recentScores.Count > 0 ? recentScores[0] : null,
            PreviousAssessmentScore = recentScores.Count > 1 ? recentScores[1] : null,
            AssessmentTrend = await GetAssessmentTrendAsync(patientId, fromDate, toDate),
            CompletedSessionsCount = completedSessionsCount
        };
    }

    private async Task<List<TrendDto>> GetAssessmentTrendAsync(Guid patientId, DateOnly fromDate, DateOnly toDate)
    {
        var assessments = await _assessmentRepository.Query()
            .AsNoTracking()
            .Where(a => a.PatientId == patientId
                && a.AssessmentDate.HasValue
                && a.AssessmentDate.Value >= fromDate
                && a.AssessmentDate.Value <= toDate
                && a.TotalScore.HasValue)
            .Select(a => new { Date = a.AssessmentDate!.Value, Score = a.TotalScore!.Value })
            .ToListAsync();

        var grouped = assessments
            .GroupBy(a => new { a.Date.Year, a.Date.Month })
            .Select(g => new TrendDto
            {
                Date = new DateTime(g.Key.Year, g.Key.Month, 1),
                Value = Math.Round((double)g.Average(x => x.Score), 2),
                Label = new DateTime(g.Key.Year, g.Key.Month, 1).ToString("MMM yyyy", new CultureInfo("ar-EG"))
            })
            .OrderBy(t => t.Date)
            .ToList();

        var result = new List<TrendDto>();
        var current = new DateTime(fromDate.Year, fromDate.Month, 1);
        var end = new DateTime(toDate.Year, toDate.Month, 1);

        while (current <= end)
        {
            var existing = grouped.FirstOrDefault(t => t.Date.Year == current.Year && t.Date.Month == current.Month);
            result.Add(existing ?? new TrendDto
            {
                Date = current,
                Value = 0,
                Label = current.ToString("MMM yyyy", new CultureInfo("ar-EG"))
            });
            current = current.AddMonths(1);
        }

        return result;
    }

    private async Task<TherapistInfoDto?> GetTherapistInfoAsync(Guid therapistId)
    {
        var therapist = await _unitOfWork.Repository<Therapist>()
            .FindSingleAsync(t => t.Id == therapistId);

        if (therapist is null)
            return null;

        return new TherapistInfoDto
        {
            Id = therapist.Id,
            FullName = therapist.FullName,
            Specialization = therapist.Specialization,
            Phone = therapist.Phone,
            ProfileImageUrl = therapist.ProfileImageUrl
        };
    }

    private async Task<CrisisSupportInfoDto> GetCrisisSupportInfoAsync()
    {
        var setting = await _unitOfWork.Repository<SystemSetting>()
            .FindSingleAsync(s => s.Key == CrisisHotlineSettingKey);

        return new CrisisSupportInfoDto
        {
            HotlineNumber = !string.IsNullOrWhiteSpace(setting?.Value) ? setting.Value : DefaultHotlineNumber,
            HotlineLabel = DefaultHotlineLabel
        };
    }

    private async Task<PatientEntity> ResolvePatientAsync(Guid userId)
    {
        return await _patientRepository.FindSingleAsync(p => p.UserId == userId)
            ?? throw new UnauthorizedAccessException("Patient profile not found.");
    }

    private static string GetFirstName(string? fullName)
    {
        if (string.IsNullOrWhiteSpace(fullName))
            return "";

        return fullName.Trim().Split(' ', 2)[0];
    }
}
