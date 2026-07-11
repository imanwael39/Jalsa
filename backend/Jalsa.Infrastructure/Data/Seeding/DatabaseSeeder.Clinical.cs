using Jalsa.Domain.Models.Assessment;
using Jalsa.Domain.Models.Session;

using Microsoft.EntityFrameworkCore;

using AssessmentEntity = Jalsa.Domain.Models.Assessment.Assessment;
using SessionEntity = Jalsa.Domain.Models.Session.Session;
using PatientEntity = Jalsa.Domain.Models.Patient.Patient;

namespace Jalsa.Infrastructure.Data.Seeding;

public sealed partial class DatabaseSeeder
{
    private enum Trajectory { Improving, Worsening, Fluctuating }

    // templateName -> (templateId, ordered question ids)
    private readonly Dictionary<string, (Guid TemplateId, List<Guid> QuestionIds)> _templates = new();

    // ── Assessment templates (reuse PHQ-9 from migration; create GAD-7 / BDI) ─
    private async Task SeedAssessmentTemplatesAsync(CancellationToken ct)
    {
        foreach (var def in AssessmentDefinition.All)
        {
            var existing = await _db.AssessmentTemplates
                .AsNoTracking()
                .FirstOrDefaultAsync(t => t.Name == def.Name, ct);

            if (existing is not null)
            {
                var questionIds = await _db.AssessmentQuestions
                    .AsNoTracking()
                    .Where(q => q.TemplateId == existing.Id)
                    .OrderBy(q => q.SortOrder)
                    .Select(q => q.Id)
                    .ToListAsync(ct);

                _templates[def.Name] = (existing.Id, questionIds);
                continue;
            }

            var template = new AssessmentTemplate
            {
                Id = Guid.NewGuid(),
                Name = def.Name,
                Version = def.Version,
                Description = def.Description,
                IsActive = true,
                CreatedAt = _now.AddMonths(-13),
            };
            _db.AssessmentTemplates.Add(template);

            var ids = new List<Guid>();
            for (var i = 0; i < def.Questions.Length; i++)
            {
                var q = new AssessmentQuestion
                {
                    Id = Guid.NewGuid(),
                    TemplateId = template.Id,
                    QuestionText = def.Questions[i],
                    QuestionType = "Likert0to3",
                    SortOrder = i + 1,
                    CreatedAt = template.CreatedAt,
                };
                _db.AssessmentQuestions.Add(q);
                ids.Add(q.Id);
            }

            _templates[def.Name] = (template.Id, ids);
        }

        await _db.SaveChangesAsync(ct);
        _db.ChangeTracker.Clear();
    }

    // ── Assessments: per-patient time series across instruments ──────────────
    private async Task SeedAssessmentsAsync(CancellationToken ct)
    {
        await SeedAssessmentTemplatesAsync(ct);

        foreach (var patient in _patients)
        {
            var trajectory = _faker.Random.Double() switch
            {
                < 0.65 => Trajectory.Improving,
                < 0.85 => Trajectory.Fluctuating,
                _ => Trajectory.Worsening,
            };

            // How many administrations of each instrument this patient received.
            AddAssessmentSeries(patient, AssessmentDefinition.Phq9, count: 3, trajectory);
            AddAssessmentSeries(patient, AssessmentDefinition.Gad7, count: _faker.Random.Int(2, 3), trajectory);
            AddAssessmentSeries(patient, AssessmentDefinition.Bdi, count: _faker.Random.Int(0, 2), trajectory);
        }

        await _db.SaveChangesAsync(ct);
        _db.ChangeTracker.Clear();
    }

    private void AddAssessmentSeries(PatientEntity patient, AssessmentDefinition def, int count, Trajectory trajectory)
    {
        if (count <= 0) return;

        var (templateId, questionIds) = _templates[def.Name];
        if (questionIds.Count == 0) return;

        // Score band as a fraction of the instrument max.
        var startPct = _faker.Random.Double(0.50, 0.85);
        var endPct = trajectory switch
        {
            Trajectory.Improving => _faker.Random.Double(0.10, 0.35),
            Trajectory.Worsening => _faker.Random.Double(0.70, 0.95),
            _ => Math.Clamp(startPct + _faker.Random.Double(-0.15, 0.15), 0.15, 0.9),
        };

        var firstDate = patient.CreatedAt.AddDays(_faker.Random.Int(2, 7));
        var lastDate = _now.AddDays(-_faker.Random.Int(1, 20));
        if (lastDate <= firstDate) lastDate = firstDate.AddDays(count * 21);

        for (var k = 0; k < count; k++)
        {
            var f = count == 1 ? 1.0 : (double)k / (count - 1);
            var pct = Lerp(startPct, endPct, f) + _faker.Random.Double(-0.06, 0.06);
            var total = (int)Math.Round(Math.Clamp(pct, 0, 1) * def.MaxTotal);

            var date = count == 1
                ? lastDate
                : firstDate.AddDays((lastDate - firstDate).TotalDays * f);

            var assessment = new AssessmentEntity
            {
                Id = Guid.NewGuid(),
                PatientId = patient.Id,
                TemplateId = templateId,
                Title = def.Name.ToUpperInvariant(),
                AssessmentDate = DateOnly.FromDateTime(date),
                TotalScore = total,
                Severity = def.Severity(total),
                Status = "Completed",
                CompletedAt = date,
                CreatedAt = date,
                UpdatedAt = date,
            };
            _db.Assessments.Add(assessment);

            var answers = def.DistributeScore(total, _faker);
            for (var q = 0; q < questionIds.Count; q++)
            {
                _db.AssessmentResponses.Add(new AssessmentResponse
                {
                    Id = Guid.NewGuid(),
                    AssessmentId = assessment.Id,
                    QuestionId = questionIds[q],
                    AnswerNumber = answers[q],
                    CreatedAt = date,
                });
            }
        }
    }

    // ── Sessions + notes ─────────────────────────────────────────────────────
    private async Task SeedSessionsAsync(CancellationToken ct)
    {
        foreach (var patient in _patients)
        {
            var tenureDays = Math.Max(14, (_now - patient.CreatedAt).TotalDays);
            var count = _faker.Random.Int(8, 20);
            var spacingDays = tenureDays / count;

            for (var n = 0; n < count; n++)
            {
                var date = patient.CreatedAt.AddDays(spacingDays * (n + 1)).AddHours(_faker.Random.Int(9, 17));
                if (date > _now) date = _now.AddDays(-1);

                // ~10% cancelled, the rest completed.
                var cancelled = _faker.Random.Double() < 0.10;
                var status = cancelled ? "Cancelled" : "Completed";

                var session = new SessionEntity
                {
                    Id = Guid.NewGuid(),
                    PatientId = patient.Id,
                    SessionNumber = n + 1,
                    SessionDate = DateOnly.FromDateTime(date),
                    DurationMinutes = _faker.PickRandom(45, 50, 60),
                    SessionType = _faker.PickRandom(ArabicSeedData.SessionTypes),
                    Status = status,
                    CreatedAt = date,
                    UpdatedAt = date,
                };
                _db.Sessions.Add(session);

                if (!cancelled)
                {
                    _db.SessionNotes.Add(new SessionNote
                    {
                        Id = Guid.NewGuid(),
                        SessionId = session.Id,
                        Observations = _faker.PickRandom(ArabicSeedData.SessionObservations),
                        Interventions = _faker.PickRandom(ArabicSeedData.Interventions),
                        PatientResponse = _faker.PickRandom(ArabicSeedData.PatientResponses),
                        HomeworkAssigned = _faker.PickRandom(ArabicSeedData.Homeworks),
                        NextGoals = _faker.PickRandom(ArabicSeedData.NextGoals),
                        CreatedAt = date,
                        UpdatedAt = date,
                    });
                }
            }

            // Half of the active patients have one upcoming scheduled session.
            if (patient.Status == "Active" && _faker.Random.Bool())
            {
                var upcoming = _now.AddDays(_faker.Random.Int(3, 14)).AddHours(_faker.Random.Int(9, 17));
                _db.Sessions.Add(new SessionEntity
                {
                    Id = Guid.NewGuid(),
                    PatientId = patient.Id,
                    SessionNumber = count + 1,
                    SessionDate = DateOnly.FromDateTime(upcoming),
                    DurationMinutes = 60,
                    SessionType = _faker.PickRandom(ArabicSeedData.SessionTypes),
                    Status = "Scheduled",
                    CreatedAt = _now,
                    UpdatedAt = _now,
                });
            }
        }

        await _db.SaveChangesAsync(ct);
        _db.ChangeTracker.Clear();
    }

    private static double Lerp(double a, double b, double t) => a + (b - a) * t;
}
