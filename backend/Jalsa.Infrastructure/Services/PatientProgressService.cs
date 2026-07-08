using System.Globalization;
using Jalsa.Application.DTOs.Dashboard;
using Jalsa.Application.DTOs.PatientProgress;
using Jalsa.Application.Interfaces.Repositories;
using Jalsa.Application.Interfaces.Services;
using Microsoft.EntityFrameworkCore;

namespace Jalsa.Infrastructure.Services;

public class PatientProgressService : IPatientProgressService
{
    private readonly IPatientRepository _patientRepository;
    private readonly ISessionRepository _sessionRepository;
    private readonly IExerciseRepository _exerciseRepository;
    private readonly IExerciseLogRepository _exerciseLogRepository;
    private readonly IAssessmentRepository _assessmentRepository;

    public PatientProgressService(
        IPatientRepository patientRepository,
        ISessionRepository sessionRepository,
        IExerciseRepository exerciseRepository,
        IExerciseLogRepository exerciseLogRepository,
        IAssessmentRepository assessmentRepository)
    {
        _patientRepository = patientRepository;
        _sessionRepository = sessionRepository;
        _exerciseRepository = exerciseRepository;
        _exerciseLogRepository = exerciseLogRepository;
        _assessmentRepository = assessmentRepository;
    }

    public async Task<PatientProgressDto> GetProgressAsync(Guid userId, DateOnly? fromDate, DateOnly? toDate)
    {
        var patientId = await ResolvePatientIdAsync(userId);

        var today = DateOnly.FromDateTime(DateTime.UtcNow);
        var to = toDate ?? today;
        var from = fromDate ?? to.AddMonths(-6);

        return new PatientProgressDto
        {
            AssessmentScoreTrend = await GetAssessmentScoreTrendAsync(patientId, from, to),
            MoodTrend = await GetMoodTrendAsync(patientId, from, to),
            ExerciseCompletionTrend = await GetExerciseCompletionTrendAsync(patientId, from, to),
            AttendanceBreakdown = await GetAttendanceBreakdownAsync(patientId, from, to),
            Statistics = await GetStatisticsAsync(patientId, from, to)
        };
    }

    private async Task<List<TrendDto>> GetAssessmentScoreTrendAsync(Guid patientId, DateOnly from, DateOnly to)
    {
        var assessments = await _assessmentRepository.Query()
            .AsNoTracking()
            .Where(a => a.PatientId == patientId
                && a.AssessmentDate.HasValue
                && a.AssessmentDate.Value >= from
                && a.AssessmentDate.Value <= to
                && a.TotalScore.HasValue)
            .Select(a => new { Date = a.AssessmentDate!.Value, Score = a.TotalScore!.Value })
            .ToListAsync();

        return BuildMonthlyTrend(from, to, assessments.Select(a => (a.Date, (double)a.Score)));
    }

    private async Task<List<TrendDto>> GetMoodTrendAsync(Guid patientId, DateOnly from, DateOnly to)
    {
        var logs = await _exerciseLogRepository.Query()
            .AsNoTracking()
            .Where(l => l.PatientId == patientId
                && l.LoggedAt.HasValue
                && l.MoodAfter.HasValue
                && DateOnly.FromDateTime(l.LoggedAt.Value) >= from
                && DateOnly.FromDateTime(l.LoggedAt.Value) <= to)
            .Select(l => new { Date = DateOnly.FromDateTime(l.LoggedAt!.Value), Mood = l.MoodAfter!.Value })
            .ToListAsync();

        return BuildMonthlyTrend(from, to, logs.Select(l => (l.Date, (double)l.Mood)));
    }

    private async Task<List<TrendDto>> GetExerciseCompletionTrendAsync(Guid patientId, DateOnly from, DateOnly to)
    {
        var logs = await _exerciseLogRepository.Query()
            .AsNoTracking()
            .Where(l => l.PatientId == patientId
                && l.CompletionStatus == "Completed"
                && l.LoggedAt.HasValue
                && DateOnly.FromDateTime(l.LoggedAt.Value) >= from
                && DateOnly.FromDateTime(l.LoggedAt.Value) <= to)
            .Select(l => DateOnly.FromDateTime(l.LoggedAt!.Value))
            .ToListAsync();

        var grouped = logs
            .GroupBy(d => new { d.Year, d.Month })
            .ToDictionary(g => (g.Key.Year, g.Key.Month), g => g.Count());

        var result = new List<TrendDto>();
        var current = new DateTime(from.Year, from.Month, 1);
        var end = new DateTime(to.Year, to.Month, 1);

        while (current <= end)
        {
            var count = grouped.TryGetValue((current.Year, current.Month), out var c) ? c : 0;
            result.Add(new TrendDto
            {
                Date = current,
                Value = count,
                Label = current.ToString("MMM yyyy", new CultureInfo("ar-EG"))
            });
            current = current.AddMonths(1);
        }

        return result;
    }

    private async Task<List<AttendanceBreakdownDto>> GetAttendanceBreakdownAsync(Guid patientId, DateOnly from, DateOnly to)
    {
        var statuses = await _sessionRepository.Query()
            .AsNoTracking()
            .Where(s => s.PatientId == patientId && s.SessionDate >= from && s.SessionDate <= to)
            .Select(s => s.Status)
            .ToListAsync();

        return statuses
            .GroupBy(s => s)
            .Select(g => new AttendanceBreakdownDto { Status = g.Key, Count = g.Count() })
            .OrderByDescending(d => d.Count)
            .ToList();
    }

    private async Task<ProgressStatisticsDto> GetStatisticsAsync(Guid patientId, DateOnly from, DateOnly to)
    {
        var totalSessions = await _sessionRepository.CountAsync(
            s => s.PatientId == patientId && s.SessionDate >= from && s.SessionDate <= to);
        var completedSessions = await _sessionRepository.CountAsync(
            s => s.PatientId == patientId && s.SessionDate >= from && s.SessionDate <= to && s.Status == "Completed");
        var cancelledSessions = await _sessionRepository.CountAsync(
            s => s.PatientId == patientId && s.SessionDate >= from && s.SessionDate <= to && s.Status == "Cancelled");
        var attendanceEligible = completedSessions + cancelledSessions;
        var attendanceRate = attendanceEligible > 0
            ? (double)completedSessions / attendanceEligible * 100
            : 0;

        var totalExercises = await _exerciseRepository.CountAsync(e => e.PatientId == patientId);
        var completedExercises = await _exerciseRepository.CountAsync(
            e => e.PatientId == patientId && e.Status == "Complete");
        var exerciseCompletionRate = totalExercises > 0
            ? (double)completedExercises / totalExercises * 100
            : 0;

        var scores = await _assessmentRepository.Query()
            .AsNoTracking()
            .Where(a => a.PatientId == patientId && a.TotalScore.HasValue && a.AssessmentDate.HasValue)
            .OrderBy(a => a.AssessmentDate)
            .Select(a => a.TotalScore!.Value)
            .ToListAsync();

        var totalAssessments = scores.Count;
        var firstScore = scores.Count > 0 ? scores[0] : (decimal?)null;
        var latestScore = scores.Count > 0 ? scores[^1] : (decimal?)null;
        var improvement = firstScore.HasValue && latestScore.HasValue && firstScore.Value != 0
            ? (double)((firstScore.Value - latestScore.Value) / firstScore.Value) * 100
            : (double?)null;

        return new ProgressStatisticsDto
        {
            TotalSessions = totalSessions,
            CompletedSessions = completedSessions,
            AttendanceRate = Math.Round(attendanceRate, 2),
            TotalExercises = totalExercises,
            CompletedExercises = completedExercises,
            ExerciseCompletionRate = Math.Round(exerciseCompletionRate, 2),
            TotalAssessments = totalAssessments,
            FirstAssessmentScore = firstScore,
            LatestAssessmentScore = latestScore,
            ImprovementPercentage = improvement.HasValue ? Math.Round(improvement.Value, 2) : null
        };
    }

    private static List<TrendDto> BuildMonthlyTrend(DateOnly from, DateOnly to, IEnumerable<(DateOnly Date, double Value)> points)
    {
        var grouped = points
            .GroupBy(p => new { p.Date.Year, p.Date.Month })
            .Select(g => new TrendDto
            {
                Date = new DateTime(g.Key.Year, g.Key.Month, 1),
                Value = Math.Round(g.Average(x => x.Value), 2),
                Label = new DateTime(g.Key.Year, g.Key.Month, 1).ToString("MMM yyyy", new CultureInfo("ar-EG"))
            })
            .ToList();

        var result = new List<TrendDto>();
        var current = new DateTime(from.Year, from.Month, 1);
        var end = new DateTime(to.Year, to.Month, 1);

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

    private async Task<Guid> ResolvePatientIdAsync(Guid userId)
    {
        var patient = await _patientRepository.FindSingleAsync(p => p.UserId == userId)
            ?? throw new UnauthorizedAccessException("Patient profile not found.");

        return patient.Id;
    }
}
