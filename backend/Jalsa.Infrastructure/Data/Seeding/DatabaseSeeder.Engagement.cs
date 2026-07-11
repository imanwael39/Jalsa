using Jalsa.Domain.Models.Chat;
using Jalsa.Domain.Models.Crisis;
using Jalsa.Domain.Models.Exercise;
using Jalsa.Domain.Models.Report;

using ExerciseEntity = Jalsa.Domain.Models.Exercise.Exercise;
using NotificationEntity = Jalsa.Domain.Models.Notification.Notification;

namespace Jalsa.Infrastructure.Data.Seeding;

public sealed partial class DatabaseSeeder
{
    // ── Exercises + logs ─────────────────────────────────────────────────────
    private async Task SeedExercisesAsync(CancellationToken ct)
    {
        foreach (var patient in _patients)
        {
            var count = _faker.Random.Int(3, 8);
            var picks = _faker.Random.Shuffle(ArabicSeedData.ExerciseCatalog).Take(count);

            foreach (var (name, description) in picks)
            {
                var start = _faker.Date.Between(patient.CreatedAt, _now);
                var due = start.AddDays(_faker.PickRandom(7, 14, 21, 30));

                // Pending exercises are due in the future and carry no logs yet.
                var isPending = due > _now && _faker.Random.Bool();
                var status = isPending ? "Active"
                    : _faker.Random.Double() < 0.75 ? "Completed"
                    : "Active";

                var exercise = new ExerciseEntity
                {
                    Id = Guid.NewGuid(),
                    PatientId = patient.Id,
                    Description = $"{name}: {description}",
                    Frequency = _faker.PickRandom(ArabicSeedData.ExerciseFrequencies),
                    StartDate = DateOnly.FromDateTime(start),
                    DueDate = DateOnly.FromDateTime(due),
                    DurationMinutes = _faker.PickRandom(5, 10, 15, 20),
                    Difficulty = _faker.PickRandom(ArabicSeedData.ExerciseDifficulties),
                    Status = status,
                    CreatedAt = start,
                    UpdatedAt = start,
                };
                _db.Exercises.Add(exercise);

                if (isPending) continue;

                // A few log entries between start and now.
                var logCount = _faker.Random.Int(1, 5);
                for (var l = 0; l < logCount; l++)
                {
                    var loggedAt = _faker.Date.Between(start, _now);
                    var moodBefore = _faker.Random.Int(3, 6);
                    var moodAfter = Math.Min(10, moodBefore + _faker.Random.Int(0, 3));

                    _db.ExerciseLogs.Add(new ExerciseLog
                    {
                        Id = Guid.NewGuid(),
                        ExerciseId = exercise.Id,
                        PatientId = patient.Id,
                        CompletionStatus = _faker.Random.WeightedRandom(
                            new[] { "Completed", "PartiallyCompleted", "Skipped" },
                            new[] { 0.6f, 0.25f, 0.15f }),
                        ReflectionNote = _faker.PickRandom(ArabicSeedData.ExerciseReflections),
                        MoodBefore = moodBefore,
                        MoodAfter = moodAfter,
                        LoggedAt = loggedAt,
                        CreatedAt = loggedAt,
                    });
                }
            }
        }

        await _db.SaveChangesAsync(ct);
        _db.ChangeTracker.Clear();
    }

    // ── Chat conversations, messages, AI logs and crisis alerts ──────────────
    private async Task SeedChatAndCrisisAsync(CancellationToken ct)
    {
        // Roughly 10 patients experience a crisis moment during a conversation.
        var crisisPatients = _faker.Random.Shuffle(_patients).Take(10).Select(p => p.Id).ToHashSet();

        foreach (var patient in _patients)
        {
            var conversationStart = _faker.Date.Between(patient.CreatedAt, _now);
            var conversation = new ChatConversation
            {
                Id = Guid.NewGuid(),
                PatientId = patient.Id,
                Status = _faker.Random.Bool(0.7f) ? "Open" : "Closed",
                CreatedAt = conversationStart,
                UpdatedAt = conversationStart,
            };
            _db.ChatConversations.Add(conversation);

            var messageCount = _faker.Random.Int(20, 60);
            var cursor = conversationStart;
            var injectCrisisAt = crisisPatients.Contains(patient.Id)
                ? _faker.Random.Int(4, messageCount - 2)
                : -1;

            for (var m = 0; m < messageCount; m++)
            {
                cursor = cursor.AddMinutes(_faker.Random.Int(1, 90));
                if (cursor > _now) cursor = _now;

                var isPatientTurn = m % 2 == 0;
                var isCrisisTurn = m == injectCrisisAt;

                string senderType;
                string content;
                int? tokens = null;
                int? latency = null;

                if (isCrisisTurn)
                {
                    senderType = "Patient";
                    content = _faker.PickRandom(ArabicSeedData.PatientCrisisMessages);
                }
                else if (isPatientTurn)
                {
                    senderType = "Patient";
                    content = m == 0
                        ? _faker.PickRandom(ArabicSeedData.PatientChatOpeners)
                        : _faker.PickRandom(ArabicSeedData.PatientChatFollowups);
                }
                else
                {
                    senderType = "AI";
                    content = _faker.PickRandom(ArabicSeedData.AiChatReplies);
                    tokens = _faker.Random.Int(60, 320);
                    latency = _faker.Random.Int(600, 2500);
                }

                var message = new ChatMessage
                {
                    Id = Guid.NewGuid(),
                    ConversationId = conversation.Id,
                    SenderType = senderType,
                    Content = content,
                    TokensUsed = tokens,
                    LatencyMs = latency,
                    CreatedAt = cursor,
                };
                _db.ChatMessages.Add(message);

                if (isCrisisTurn)
                {
                    // Reassuring AI reply immediately after the crisis message.
                    cursor = cursor.AddMinutes(1);
                    _db.ChatMessages.Add(new ChatMessage
                    {
                        Id = Guid.NewGuid(),
                        ConversationId = conversation.Id,
                        SenderType = "AI",
                        Content = _faker.PickRandom(ArabicSeedData.AiCrisisReplies),
                        TokensUsed = _faker.Random.Int(80, 200),
                        LatencyMs = _faker.Random.Int(600, 2000),
                        CreatedAt = cursor,
                    });

                    _db.CrisisAlerts.Add(new CrisisAlert
                    {
                        Id = Guid.NewGuid(),
                        PatientId = patient.Id,
                        TherapistId = patient.TherapistId,
                        ChatMessageId = message.Id,
                        Severity = _faker.PickRandom(ArabicSeedData.CrisisSeverities),
                        Status = _faker.PickRandom(ArabicSeedData.CrisisStatuses),
                        CreatedAt = cursor,
                        UpdatedAt = cursor,
                    });
                }
            }

            conversation.LastActivityAt = cursor;
            conversation.UpdatedAt = cursor;

            // A couple of AI telemetry rows per conversation.
            for (var a = 0; a < _faker.Random.Int(1, 3); a++)
            {
                _db.AiChatLogs.Add(new AiChatLog
                {
                    Id = Guid.NewGuid(),
                    ConversationId = conversation.Id,
                    PatientId = patient.Id,
                    TokensUsed = _faker.Random.Int(200, 1200),
                    Cost = Math.Round(_faker.Random.Decimal(0.001m, 0.02m), 6),
                    ResponseLatencyMs = _faker.Random.Int(600, 2500),
                    ModelUsed = "gemini-2.5-flash",
                    CreatedAt = _faker.Date.Between(conversationStart, cursor),
                });
            }
        }

        await _db.SaveChangesAsync(ct);
        _db.ChangeTracker.Clear();
    }

    // ── Notifications for therapists ─────────────────────────────────────────
    private async Task SeedNotificationsAsync(CancellationToken ct)
    {
        var patientsByTherapist = _patients
            .GroupBy(p => p.TherapistId)
            .ToDictionary(g => g.Key, g => g.ToList());

        foreach (var therapist in _therapists)
        {
            if (!patientsByTherapist.TryGetValue(therapist.Id, out var patients) || patients.Count == 0)
                continue;

            const int perTherapist = 30; // ~150 total across 5 therapists
            for (var i = 0; i < perTherapist; i++)
            {
                var patient = _faker.PickRandom(patients);
                var (type, title, bodyTemplate) = _faker.PickRandom(ArabicSeedData.NotificationTemplates);
                var createdAt = _faker.Date.Between(_now.AddDays(-60), _now);
                var isRead = _faker.Random.Bool(0.5f);

                _db.Notifications.Add(new NotificationEntity
                {
                    Id = Guid.NewGuid(),
                    RecipientUserId = therapist.UserId,
                    Type = type,
                    Title = title,
                    Body = string.Format(bodyTemplate, patient.FullName),
                    IsRead = isRead,
                    ReadAt = isRead ? _faker.Date.Between(createdAt, _now) : null,
                    CreatedAt = createdAt,
                });
            }
        }

        await _db.SaveChangesAsync(ct);
        _db.ChangeTracker.Clear();
    }

    // ── Referral reports + versions ──────────────────────────────────────────
    private async Task SeedReportsAsync(CancellationToken ct)
    {
        // ReferralReport and ReportVersion reference each other (CurrentVersionId / ReportId),
        // which EF cannot untangle in a single INSERT batch. So we insert both with a null
        // CurrentVersionId first, then set the pointer in a second pass.
        var pending = new List<(ReferralReport Report, Guid VersionId)>();

        foreach (var patient in _patients)
        {
            var reportCount = _faker.Random.Int(2, 3); // ~100 total
            for (var r = 0; r < reportCount; r++)
            {
                var status = _faker.Random.WeightedRandom(
                    new[] { "Approved", "Draft", "Rejected" },
                    new[] { 0.5f, 0.35f, 0.15f });
                var createdAt = _faker.Date.Between(patient.CreatedAt.AddDays(14), _now);

                var report = new ReferralReport
                {
                    Id = Guid.NewGuid(),
                    PatientId = patient.Id,
                    TherapistId = patient.TherapistId,
                    GeneratedByTherapistId = patient.TherapistId,
                    Status = status,
                    CreatedAt = createdAt,
                    UpdatedAt = createdAt,
                };
                _db.ReferralReports.Add(report);

                var versionCount = _faker.Random.Int(1, 2);
                ReportVersion? lastVersion = null;
                for (var v = 0; v < versionCount; v++)
                {
                    var versionDate = createdAt.AddDays(v * 2);
                    var content = ArabicSeedData.BuildReportContent(
                        patient.FullName,
                        _faker.PickRandom(ArabicSeedData.ReportDiagnosisImpressions),
                        _faker.PickRandom(ArabicSeedData.ProgressSummaries),
                        _faker.PickRandom(ArabicSeedData.ReportRecommendations));

                    lastVersion = new ReportVersion
                    {
                        Id = Guid.NewGuid(),
                        ReportId = report.Id,
                        VersionNumber = v + 1,
                        Content = content,
                        CreatedByTherapistId = patient.TherapistId,
                        ApprovedAt = status == "Approved" && v == versionCount - 1 ? versionDate : null,
                        ChangeNote = v == 0 ? "النسخة الأولى المولّدة آليًا." : "تعديل يدوي على التوصيات.",
                        CreatedAt = versionDate,
                    };
                    _db.ReportVersions.Add(lastVersion);
                }

                pending.Add((report, lastVersion!.Id));
            }
        }

        // Pass 1: insert reports (CurrentVersionId = null) and their versions.
        await _db.SaveChangesAsync(ct);

        // Pass 2: point each report at its latest version. Change detection is disabled,
        // so mark the property modified explicitly.
        foreach (var (report, versionId) in pending)
        {
            report.CurrentVersionId = versionId;
            _db.Entry(report).Property(x => x.CurrentVersionId).IsModified = true;
        }

        await _db.SaveChangesAsync(ct);
        _db.ChangeTracker.Clear();
    }
}
