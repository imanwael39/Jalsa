using Bogus;

using Jalsa.Domain.Models.Clinic;
using Jalsa.Domain.Models.Identity;
using Jalsa.Infrastructure.Data;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

using PatientEntity = Jalsa.Domain.Models.Patient.Patient;
using IntakeFormEntity = Jalsa.Domain.Models.Patient.IntakeForm;

namespace Jalsa.Infrastructure.Data.Seeding;

/// <summary>
/// Production-quality, idempotent development data seeder for the Jalsa clinic.
///
/// Design notes:
/// - Runtime seeder (not EF <c>HasData</c>) because the volume and Bogus-driven randomization
///   are unsuitable for static migration seed data.
/// - Idempotent: a no-op if the sentinel admin account already exists.
/// - Deterministic: a fixed Bogus seed so a fresh database always produces the same clinic.
/// - Bulk-insert friendly: only scalar foreign-key Guids are set (never navigation properties),
///   which lets the change tracker be cleared between phases to keep memory flat.
/// - All timestamps are set explicitly to believable values spread over the last ~12 months,
///   which overrides the <c>GETUTCDATE()</c> column defaults.
/// </summary>
public sealed partial class DatabaseSeeder
{
    private const int SeedNumber = 20260711;

    // Fixed, testable login accounts.
    private const string AdminEmail = "admin@jalsa.com";
    private const string AdminPassword = "Admin@123";
    private const string DefaultPassword = "Password123!";

    // Volume knobs.
    private const int TherapistCount = 5;
    private const int PatientCount = 40;

    private readonly JalsaDbContext _db;
    private readonly ILogger<DatabaseSeeder> _logger;

    private Faker _faker = new();
    private readonly DateTime _now = DateTime.UtcNow;

    // Cached hashes so we run BCrypt once per distinct password, not once per user.
    private readonly Dictionary<string, string> _hashCache = new();

    // Reference data shared across phases (populated as we go).
    private readonly Dictionary<string, Guid> _roleIds = new();
    private Guid _clinicId;
    private readonly List<Therapist> _therapists = new();
    private readonly List<PatientEntity> _patients = new();

    public DatabaseSeeder(JalsaDbContext db, ILogger<DatabaseSeeder> logger)
    {
        _db = db;
        _logger = logger;
    }

    public async Task SeedAsync(CancellationToken ct = default)
    {
        if (await _db.Users.AnyAsync(u => u.Email == AdminEmail, ct))
        {
            _logger.LogInformation("Database already seeded (found {Email}); skipping.", AdminEmail);
            return;
        }

        _logger.LogInformation("Seeding development data...");

        Randomizer.Seed = new Random(SeedNumber);
        _faker = new Faker();

        var autoDetect = _db.ChangeTracker.AutoDetectChangesEnabled;
        _db.ChangeTracker.AutoDetectChangesEnabled = false;

        await using var tx = await _db.Database.BeginTransactionAsync(ct);
        try
        {
            await SeedIdentityAsync(ct);
            await SeedPatientsAndIntakeAsync(ct);
            await SeedAssessmentsAsync(ct);
            await SeedSessionsAsync(ct);
            await SeedExercisesAsync(ct);
            await SeedChatAndCrisisAsync(ct);
            await SeedTherapistAiChatAsync(ct);
            await SeedNotificationsAsync(ct);
            await SeedReportsAsync(ct);

            await tx.CommitAsync(ct);
            _logger.LogInformation(
                "Seeding complete: {Therapists} therapists, {Patients} patients.",
                _therapists.Count, _patients.Count);
        }
        catch (Exception ex)
        {
            await tx.RollbackAsync(ct);
            _logger.LogError(ex, "Seeding failed; transaction rolled back.");
            throw;
        }
        finally
        {
            _db.ChangeTracker.AutoDetectChangesEnabled = autoDetect;
        }
    }

    // ── Phase 1: roles, admin, clinic, therapists ────────────────────────────
    private async Task SeedIdentityAsync(CancellationToken ct)
    {
        // Roles are seeded by the InitialCreate migration; reuse them.
        var roles = await _db.Roles.AsNoTracking().ToListAsync(ct);
        foreach (var r in roles) _roleIds[r.Name] = r.Id;
        foreach (var name in new[] { "Admin", "Therapist", "Patient" })
        {
            if (_roleIds.ContainsKey(name)) continue;
            var role = new Role { Id = Guid.NewGuid(), Name = name };
            _db.Roles.Add(role);
            _roleIds[name] = role.Id;
        }

        // One clinic that all therapists and patients belong to.
        _clinicId = Guid.NewGuid();
        _db.Clinics.Add(new Clinic
        {
            Id = _clinicId,
            Name = "عيادة جلسة للصحة النفسية",
            Timezone = "Africa/Cairo",
            CreatedAt = _now.AddMonths(-13),
        });

        // Admin.
        var adminUser = NewUser(AdminEmail, AdminPassword, _now.AddMonths(-13));
        AddUserWithRole(adminUser, "Admin");

        // Therapists (therapist1..N@jalsa.com / Password123!).
        for (var i = 0; i < TherapistCount; i++)
        {
            var createdAt = _now.AddMonths(-13).AddDays(i * 3);
            var user = NewUser($"therapist{i + 1}@jalsa.com", DefaultPassword, createdAt);
            AddUserWithRole(user, "Therapist");

            var therapist = new Therapist
            {
                Id = Guid.NewGuid(),
                UserId = user.Id,
                FullName = ArabicSeedData.TherapistFullNames[i],
                LicenseNumber = $"LIC-{2001 + i}",
                Specialization = ArabicSeedData.Specializations[i % ArabicSeedData.Specializations.Length],
                Phone = RandomPhone(),
                Bio = _faker.PickRandom(ArabicSeedData.TherapistBios),
                ApprovalStatus = TherapistApprovalStatus.Approved,
                ApprovalStatusUpdatedAt = createdAt,
                CreatedAt = createdAt,
            };
            _db.Therapists.Add(therapist);
            _db.TherapistClinics.Add(new TherapistClinic
            {
                TherapistId = therapist.Id,
                ClinicId = _clinicId,
                IsPrimary = true,
            });
            _therapists.Add(therapist);
        }

        await _db.SaveChangesAsync(ct);
        _db.ChangeTracker.Clear();
    }

    // ── Phase 2: patients + intake forms ─────────────────────────────────────
    private async Task SeedPatientsAndIntakeAsync(CancellationToken ct)
    {
        for (var i = 0; i < PatientCount; i++)
        {
            // First N patients seed one-per-therapist for guaranteed coverage; the rest random.
            var therapist = i < _therapists.Count
                ? _therapists[i]
                : _faker.PickRandom(_therapists);

            var isFemale = _faker.Random.Bool();
            var firstName = _faker.PickRandom(isFemale ? ArabicSeedData.FemaleFirstNames : ArabicSeedData.MaleFirstNames);
            var fullName = $"{firstName} {_faker.PickRandom(ArabicSeedData.FamilyNames)}";
            var createdAt = RecentCreatedAt(maxMonthsBack: 12);
            var isArchived = _faker.Random.Double() < 0.15;

            var patient = new PatientEntity
            {
                Id = Guid.NewGuid(),
                TherapistId = therapist.Id,
                ClinicId = _clinicId,
                UserId = null, // set below once the login user exists
                FullName = fullName,
                DateOfBirth = DateOnly.FromDateTime(_faker.Date.Between(_now.AddYears(-65), _now.AddYears(-18))),
                Gender = isFemale ? "أنثى" : "ذكر",
                Phone = RandomPhone(),
                Email = $"patient{i + 1}@jalsa.com",
                Address = $"{_faker.PickRandom(ArabicSeedData.Streets)}، {_faker.PickRandom(ArabicSeedData.Cities)}",
                ReferralSource = _faker.PickRandom(ArabicSeedData.ReferralSources),
                ChiefComplaint = _faker.PickRandom(ArabicSeedData.ChiefComplaints),
                EmergencyContactName = $"{_faker.PickRandom(ArabicSeedData.MaleFirstNames)} {_faker.PickRandom(ArabicSeedData.FamilyNames)}",
                EmergencyContactRelationship = _faker.PickRandom(ArabicSeedData.EmergencyRelationships),
                EmergencyContactPhone = RandomPhone(),
                MedicalHistory = _faker.PickRandom(ArabicSeedData.PsychiatricHistories),
                TreatmentStartDate = DateOnly.FromDateTime(createdAt),
                Status = isArchived ? "Archived" : "Active",
                CreatedAt = createdAt,
                UpdatedAt = createdAt,
            };

            // Every patient also gets a login account (patientN@jalsa.com / Password123!).
            var user = NewUser(patient.Email!, DefaultPassword, createdAt);
            AddUserWithRole(user, "Patient");
            patient.UserId = user.Id;

            _db.Patients.Add(patient);
            _patients.Add(patient);

            // One submitted intake form per patient.
            _db.IntakeForms.Add(new IntakeFormEntity
            {
                Id = Guid.NewGuid(),
                PatientId = patient.Id,
                PresentingProblem = _faker.PickRandom(ArabicSeedData.PresentingProblems),
                PsychiatricHistory = _faker.PickRandom(ArabicSeedData.PsychiatricHistories),
                FamilyHistory = _faker.PickRandom(ArabicSeedData.FamilyHistories),
                SocialHistory = _faker.PickRandom(ArabicSeedData.SocialHistories),
                Medications = _faker.PickRandom(ArabicSeedData.Medications),
                Status = "Submitted",
                CreatedAt = createdAt,
                SubmittedAt = createdAt.AddDays(1),
            });
        }

        await _db.SaveChangesAsync(ct);
        _db.ChangeTracker.Clear();
    }

    // ── Shared helpers ───────────────────────────────────────────────────────
    private User NewUser(string email, string password, DateTime createdAt) => new()
    {
        Id = Guid.NewGuid(),
        Email = email,
        PasswordHash = HashPassword(password),
        IsActive = true,
        IsDeleted = false,
        FailedLoginAttempts = 0,
        LastLoginAt = _faker.Random.Bool(0.7f) ? _faker.Date.Between(createdAt, _now) : null,
        CreatedAt = createdAt,
        UpdatedAt = createdAt,
    };

    private void AddUserWithRole(User user, string roleName)
    {
        _db.Users.Add(user);
        _db.UserRoles.Add(new UserRole
        {
            UserId = user.Id,
            RoleId = _roleIds[roleName],
            CreatedAt = user.CreatedAt,
        });
    }

    private string HashPassword(string password)
    {
        if (!_hashCache.TryGetValue(password, out var hash))
        {
            hash = BCrypt.Net.BCrypt.HashPassword(password);
            _hashCache[password] = hash;
        }
        return hash;
    }

    private string RandomPhone() => $"01{_faker.Random.Int(0, 2)}{_faker.Random.Replace("########")}";

    /// <summary>A creation date within the last <paramref name="maxMonthsBack"/> months, skewed recent.</summary>
    private DateTime RecentCreatedAt(int maxMonthsBack)
    {
        // Square the uniform sample to bias toward more recent dates.
        var t = _faker.Random.Double();
        var monthsBack = maxMonthsBack * (t * t);
        return _now.AddDays(-monthsBack * 30).AddHours(-_faker.Random.Int(0, 23));
    }
}
