using System.Globalization;
using Jalsa.Application.DTOs.Dashboard;
using Jalsa.Application.Interfaces.Repositories;
using Jalsa.Application.Interfaces.Repositores;
using Jalsa.Application.Interfaces.Services;
using Jalsa.Domain.Models.Crisis;
using Microsoft.EntityFrameworkCore;

namespace Jalsa.Infrastructure.Services;

public class ProgressService : IProgressService
{
    private readonly IPatientRepository _patientRepository;
    private readonly ISessionRepository _sessionRepository;
    private readonly IExerciseRepository _exerciseRepository;
    private readonly IAssessmentRepository _assessmentRepository;
    private readonly IUnitOfWork _unitOfWork;

    public ProgressService(
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

    public async Task<DashboardSummaryDto> GetDashboardAsync()
    {
        var today = DateOnly.FromDateTime(DateTime.UtcNow);
        var thirtyDaysAgo = today.AddDays(-30);
        var sixMonthsAgo = today.AddMonths(-6);
        var eightWeeksAgo = today.AddDays(-56);

        var totalPatients = await _patientRepository.CountAsync(_ => true);
        var activePatients = await _patientRepository.CountAsync(p => p.Status != "Archived");
        var archivedPatients = await _patientRepository.CountAsync(p => p.Status == "Archived");

        var totalSessions = await _sessionRepository.CountAsync(_ => true);
        var sessionsThisMonth = await _sessionRepository.CountAsync(s => s.SessionDate >= thirtyDaysAgo);

        var totalExercises = await _exerciseRepository.CountAsync(_ => true);
        var completedExercises = await _exerciseRepository.CountAsync(e => e.Status == "Complete");
        var exerciseCompletionRate = totalExercises > 0
            ? (double)completedExercises / totalExercises * 100
            : 0;

        var assessmentScores = await _assessmentRepository.Query()
            .AsNoTracking()
            .Where(a => a.TotalScore.HasValue)
            .Select(a => a.TotalScore!.Value)
            .ToListAsync();
        var averageAssessmentScore = assessmentScores.Count > 0
            ? (double)assessmentScores.Average()
            : 0;

        var recentAlerts = await _unitOfWork.Repository<CrisisAlert>().Query()
            .AsNoTracking()
            .OrderByDescending(a => a.CreatedAt)
            .Take(5)
            .Select(a => $"[{a.Severity}] Alert for patient {a.PatientId} — {a.Status}")
            .ToListAsync();

        var assessmentTrend = await GetAssessmentTrendAsync(sixMonthsAgo, today);
        var exerciseCompletion = await GetExerciseCompletionBreakdownAsync();
        var sessionFrequency = await GetSessionFrequencyAsync(eightWeeksAgo, today);

        return new DashboardSummaryDto
        {
            TotalPatients = totalPatients,
            ActivePatients = activePatients,
            ArchivedPatients = archivedPatients,
            TotalSessions = totalSessions,
            SessionsThisMonth = sessionsThisMonth,
            ExerciseCompletionRate = Math.Round(exerciseCompletionRate, 2),
            AverageAssessmentScore = Math.Round(averageAssessmentScore, 2),
            RecentAlerts = recentAlerts,
            Analytics = new AnalyticsDto
            {
                AssessmentTrend = assessmentTrend,
                ExerciseCompletion = exerciseCompletion,
                SessionFrequency = sessionFrequency
            }
        };
    }

    private async Task<List<TrendDto>> GetAssessmentTrendAsync(DateOnly fromDate, DateOnly toDate)
    {
        var assessments = await _assessmentRepository.Query()
            .AsNoTracking()
            .Where(a => a.AssessmentDate.HasValue
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

    private async Task<ExerciseCompletionBreakdownDto> GetExerciseCompletionBreakdownAsync()
    {
        var exercises = await _exerciseRepository.Query()
            .AsNoTracking()
            .Select(e => e.Status)
            .ToListAsync();

        return new ExerciseCompletionBreakdownDto
        {
            Complete = exercises.Count(s => s == "Complete"),
            Partial = exercises.Count(s => s == "Partial"),
            Skipped = exercises.Count(s => s == "Skipped")
        };
    }

    private async Task<List<TrendDto>> GetSessionFrequencyAsync(DateOnly fromDate, DateOnly toDate)
    {
        var sessions = await _sessionRepository.Query()
            .AsNoTracking()
            .Where(s => s.SessionDate >= fromDate && s.SessionDate <= toDate)
            .Select(s => s.SessionDate)
            .ToListAsync();

        var grouped = sessions
            .GroupBy(d => GetWeekStart(d))
            .ToDictionary(g => g.Key, g => g.Count());

        var result = new List<TrendDto>();
        var currentWeekStart = GetWeekStart(fromDate);
        var endWeekStart = GetWeekStart(toDate);

        while (currentWeekStart <= endWeekStart)
        {
            var count = grouped.TryGetValue(currentWeekStart, out var c) ? c : 0;
            result.Add(new TrendDto
            {
                Date = currentWeekStart.ToDateTime(TimeOnly.MinValue),
                Value = count,
                Label = $"أسبوع {currentWeekStart.ToString("dd MMM", new CultureInfo("ar-EG"))}"
            });
            currentWeekStart = currentWeekStart.AddDays(7);
        }

        return result;
    }

    private static DateOnly GetWeekStart(DateOnly date)
    {
        var diff = (int)date.DayOfWeek - (int)DayOfWeek.Monday;
        if (diff < 0) diff += 7;
        return date.AddDays(-diff);
    }
}
