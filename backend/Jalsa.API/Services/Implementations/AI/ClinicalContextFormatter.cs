using Jalsa.API.Services.Interfaces.AI;

namespace Jalsa.API.Services.Implementations.AI;

/// <summary>
/// Renders <see cref="PatientContextBundle"/> pieces into bilingual plain-text
/// sections for LLM prompts. Shared by patient summary, report generation, and
/// chat so every AI feature presents clinical context the same way.
/// </summary>
public static class ClinicalContextFormatter
{
    public static string BuildIntakeText(IntakeContext? intake, string language)
    {
        var isArabic = language == "ar";

        if (intake is null)
            return isArabic ? "لا يوجد نموذج قبول مسجل." : "No intake form on record.";

        var lines = new List<string>();

        void Add(string labelAr, string labelEn, string? value)
        {
            if (string.IsNullOrWhiteSpace(value)) return;
            lines.Add($"{(isArabic ? labelAr : labelEn)}: {value}");
        }

        Add("المشكلة الحالية", "Presenting problem", intake.PresentingProblem);
        Add("التاريخ النفسي", "Psychiatric history", intake.PsychiatricHistory);
        Add("التاريخ العائلي", "Family history", intake.FamilyHistory);
        Add("الأدوية", "Medications", intake.Medications);
        Add("التاريخ الاجتماعي", "Social history", intake.SocialHistory);

        if (lines.Count == 0)
            return isArabic ? "نموذج القبول فارغ." : "Intake form is empty.";

        return string.Join("\n", lines);
    }

    public static string BuildAssessmentsText(IReadOnlyList<AssessmentContext> assessments, string language)
    {
        var isArabic = language == "ar";

        if (assessments.Count == 0)
            return isArabic ? "لا توجد تقييمات مسجلة." : "No assessments on record.";

        return string.Join("\n", assessments.Select(a =>
        {
            var date = a.AssessmentDate?.ToString("d") ?? (isArabic ? "بدون تاريخ" : "no date");
            var score = a.TotalScore?.ToString("0.##") ?? "-";
            var severity = string.IsNullOrWhiteSpace(a.Severity) ? "-" : a.Severity;
            return isArabic
                ? $"{a.TemplateName} ({date}): الدرجة {score}، الشدة {severity}" + (string.IsNullOrWhiteSpace(a.Notes) ? "" : $" — {a.Notes}")
                : $"{a.TemplateName} ({date}): score {score}, severity {severity}" + (string.IsNullOrWhiteSpace(a.Notes) ? "" : $" — {a.Notes}");
        }));
    }

    public static string BuildSessionNotesText(IReadOnlyList<SessionNoteContext> notes, string language)
    {
        var isArabic = language == "ar";

        if (notes.Count == 0)
            return isArabic ? "لا توجد ملاحظات جلسات مباشرة." : "No direct session notes available.";

        return string.Join("\n\n", notes.Select(n =>
        {
            var parts = new List<string>();
            var header = isArabic
                ? $"الجلسة رقم {n.SessionNumber} ({n.SessionDate:d})"
                : $"Session #{n.SessionNumber} ({n.SessionDate:d})";
            parts.Add(header);

            void Add(string labelAr, string labelEn, string? value)
            {
                if (string.IsNullOrWhiteSpace(value)) return;
                parts.Add($"{(isArabic ? labelAr : labelEn)}: {value}");
            }

            Add("الملاحظات", "Observations", n.Observations);
            Add("التدخلات", "Interventions", n.Interventions);
            Add("استجابة المريض", "Patient response", n.PatientResponse);
            Add("الواجبات", "Homework", n.HomeworkAssigned);
            Add("الأهداف التالية", "Next goals", n.NextGoals);

            return string.Join("\n", parts);
        }));
    }

    public static string BuildVoiceTranscriptsText(IReadOnlyList<VoiceTranscriptContext> transcripts, string language)
    {
        var isArabic = language == "ar";

        if (transcripts.Count == 0)
            return isArabic ? "لا توجد نصوص تسجيلات صوتية." : "No voice transcripts available.";

        return string.Join("\n\n", transcripts.Select(t =>
        {
            var header = isArabic
                ? $"تسجيل الجلسة رقم {t.SessionNumber}:"
                : $"Session #{t.SessionNumber} recording:";
            return $"{header}\n{t.Transcript}";
        }));
    }
}
