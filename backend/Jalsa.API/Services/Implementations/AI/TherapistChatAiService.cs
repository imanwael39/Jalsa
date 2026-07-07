using Jalsa.API.Configurations;
using Jalsa.API.Services.Interfaces.AI;
using Jalsa.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace Jalsa.API.Services.Implementations.AI;

public class TherapistChatAiService : ITherapistChatAiService
{
    private readonly IGeminiClient _client;
    private readonly JalsaDbContext _context;
    private readonly string _model;

    private readonly ILlmObservabilityService _observability;
    private readonly Services.Interfaces.IPromptService _prompts;

    public TherapistChatAiService(
        IOptions<GeminiSettings> settings,
        IGeminiClient client,
        JalsaDbContext context,
        ILlmObservabilityService observability,
        Services.Interfaces.IPromptService prompts)
    {
        _client = client;
        _model = settings.Value.ChatModelId;
        _context = context;
        _observability = observability;
        _prompts = prompts;
    }

    public async Task<string> AnswerQuestionAsync(
        Guid patientId,
        string question,
        string language = "ar")
    {
        var patient = await _context.Patients
            .FirstOrDefaultAsync(p => p.Id == patientId);

        if (patient == null)
            return language == "ar" ? "المريض غير موجود." : "Patient not found.";

        var recentSessions = await _context.Sessions
            .Include(s => s.SessionNote)
            .Where(s => s.PatientId == patientId)
            .OrderByDescending(s => s.SessionDate)
            .Take(5)
            .ToListAsync();

        var recentAssessments = await _context.Assessments
            .Include(a => a.Template)
            .Where(a => a.PatientId == patientId)
            .OrderByDescending(a => a.AssessmentDate)
            .Take(5)
            .ToListAsync();

        var sessionsText = string.Join("\n\n", recentSessions.Select(s =>
        {
            var n = s.SessionNote;
            var fields = new[]
            {
                n?.Observations is not null ? $"الملاحظات: {n.Observations}" : null,
                n?.Interventions is not null ? $"التدخلات: {n.Interventions}" : null,
                n?.PatientResponse is not null ? $"استجابة المريض: {n.PatientResponse}" : null,
                n?.HomeworkAssigned is not null ? $"الواجبات المنزلية: {n.HomeworkAssigned}" : null,
                n?.NextGoals is not null ? $"الأهداف التالية: {n.NextGoals}" : null
            }.Where(x => x is not null);

            return $"جلسة {s.SessionDate:yyyy-MM-dd}:\n" + string.Join("\n", fields);
        }));

        var assessmentsText = string.Join("\n\n", recentAssessments.Select(a =>
            $"{a.Template?.Name ?? a.Title} ({a.AssessmentDate:yyyy-MM-dd}): {a.TotalScore}"));

        var contextText = string.Join(
            "\n\n",
            new[] { sessionsText, assessmentsText }.Where(t => !string.IsNullOrWhiteSpace(t)));

        var userPrompt = _prompts.Get(
            "therapist-patient-qa",
            language,
            new Dictionary<string, string>
            {
                ["patientName"] = patient.FullName,
                ["contextText"] = contextText,
                ["question"] = question
            });

        var systemPrompt = _prompts.Get("therapist-patient-qa", language);

        var startTime = DateTime.UtcNow;

        var output = await _client.ChatAsync(systemPrompt, userPrompt);

        await _observability.LogGenerationAsync(
            new LlmGenerationLog
            {
                Name = "therapist-patient-qa",
                Model = _model,
                Input = userPrompt,
                Output = output,
                StartTime = startTime,
                EndTime = DateTime.UtcNow,

                Metadata = new Dictionary<string, object>
                {
                    ["patientId"] = patientId.ToString()
                }
            });

        return output;
    }
}
