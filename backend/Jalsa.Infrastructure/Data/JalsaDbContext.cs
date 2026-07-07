using Jalsa.Domain.Models.Ai;
using Jalsa.Domain.Models.Assessment;
using Jalsa.Domain.Models.Audit;
using Jalsa.Domain.Models.Chat;
using Jalsa.Domain.Models.Clinic;
using Jalsa.Domain.Models.Crisis;
using Jalsa.Domain.Models.Exercise;
using Jalsa.Domain.Models.File;
using Jalsa.Domain.Models.Identity;
using Jalsa.Domain.Models.Notification;
using Jalsa.Domain.Models.Patient;
using Jalsa.Domain.Models.Report;
using Jalsa.Domain.Models.Session;
using Jalsa.Domain.Models.System;
using Microsoft.EntityFrameworkCore;

namespace Jalsa.Infrastructure.Data;

public class JalsaDbContext : DbContext
{
    public JalsaDbContext(DbContextOptions<JalsaDbContext> options) : base(options) { }

    public DbSet<User> Users => Set<User>();
    public DbSet<Role> Roles => Set<Role>();
    public DbSet<UserRole> UserRoles => Set<UserRole>();
    public DbSet<RefreshToken> RefreshTokens => Set<RefreshToken>();
    public DbSet<PasswordResetToken> PasswordResetTokens => Set<PasswordResetToken>();
    public DbSet<Clinic> Clinics => Set<Clinic>();
    public DbSet<Therapist> Therapists => Set<Therapist>();
    public DbSet<TherapistClinic> TherapistClinics => Set<TherapistClinic>();
    public DbSet<Patient> Patients => Set<Patient>();
    public DbSet<PatientInvitation> PatientInvitations => Set<PatientInvitation>();
    public DbSet<IntakeForm> IntakeForms => Set<IntakeForm>();
    public DbSet<IntakeFormOcrExtraction> IntakeFormOcrExtractions => Set<IntakeFormOcrExtraction>();
    public DbSet<Session> Sessions => Set<Session>();
    public DbSet<SessionNote> SessionNotes => Set<SessionNote>();
    public DbSet<SessionEmbedding> SessionEmbeddings => Set<SessionEmbedding>();
    public DbSet<VoiceMemo> VoiceMemos => Set<VoiceMemo>();
    public DbSet<UploadedFile> UploadedFiles => Set<UploadedFile>();
    public DbSet<Notification> Notifications => Set<Notification>();
    public DbSet<AuditLog> AuditLogs => Set<AuditLog>();
    public DbSet<AssessmentTemplate> AssessmentTemplates => Set<AssessmentTemplate>();
    public DbSet<AssessmentQuestion> AssessmentQuestions => Set<AssessmentQuestion>();
    public DbSet<Assessment> Assessments => Set<Assessment>();
    public DbSet<AssessmentResponse> AssessmentResponses => Set<AssessmentResponse>();
    public DbSet<Exercise> Exercises => Set<Exercise>();
    public DbSet<ExerciseLog> ExerciseLogs => Set<ExerciseLog>();
    public DbSet<ChatConversation> ChatConversations => Set<ChatConversation>();
    public DbSet<ChatMessage> ChatMessages => Set<ChatMessage>();
    public DbSet<AiChatLog> AiChatLogs => Set<AiChatLog>();
    public DbSet<CrisisAlert> CrisisAlerts => Set<CrisisAlert>();
    public DbSet<ReferralReport> ReferralReports => Set<ReferralReport>();
    public DbSet<ReportVersion> ReportVersions => Set<ReportVersion>();
    public DbSet<SystemSetting> SystemSettings => Set<SystemSetting>();
    public DbSet<AiArtifact> AiArtifacts => Set<AiArtifact>();
    public DbSet<AiReportGenerationLog> AiReportGenerationLogs => Set<AiReportGenerationLog>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // ── Identity ──────────────────────────────────────────────
        modelBuilder.Entity<User>(e =>
        {
            e.ToTable("Users");
            e.HasKey(u => u.Id);
            e.Property(u => u.Id).HasDefaultValueSql("NEWSEQUENTIALID()");
            e.Property(u => u.Email).IsRequired();
            e.HasIndex(u => u.Email).IsUnique();
            e.Property(u => u.PasswordHash).IsRequired();
            e.Property(u => u.IsActive).HasDefaultValue(true);
            e.Property(u => u.IsDeleted).HasDefaultValue(false);
            e.Property(u => u.FailedLoginAttempts).HasDefaultValue(0);
            e.Property(u => u.LockoutEnd).IsRequired(false);
            e.Property(u => u.CreatedAt).HasDefaultValueSql("GETUTCDATE()");
            e.Property(u => u.UpdatedAt).HasDefaultValueSql("GETUTCDATE()");
        });

        modelBuilder.Entity<Role>(e =>
        {
            e.ToTable("Roles");
            e.HasKey(r => r.Id);
            e.Property(r => r.Id).HasDefaultValueSql("NEWSEQUENTIALID()");
            e.Property(r => r.Name).IsRequired();
            e.HasIndex(r => r.Name).IsUnique();
        });

        modelBuilder.Entity<UserRole>(e =>
        {
            e.ToTable("UserRoles");
            e.HasKey(ur => new { ur.UserId, ur.RoleId });
            e.Property(ur => ur.CreatedAt).HasDefaultValueSql("GETUTCDATE()");
            e.HasOne(ur => ur.User).WithMany(u => u.UserRoles).HasForeignKey(ur => ur.UserId).OnDelete(DeleteBehavior.NoAction);
            e.HasOne(ur => ur.Role).WithMany(r => r.UserRoles).HasForeignKey(ur => ur.RoleId).OnDelete(DeleteBehavior.NoAction);
        });

        modelBuilder.Entity<RefreshToken>(e =>
        {
            e.ToTable("RefreshTokens");
            e.HasKey(rt => rt.Id);
            e.Property(rt => rt.Id).HasDefaultValueSql("NEWSEQUENTIALID()");
            e.Property(rt => rt.Token).IsRequired();
            e.HasIndex(rt => rt.Token).IsUnique();
            e.Property(rt => rt.CreatedAt).HasDefaultValueSql("GETUTCDATE()");
            e.HasOne(rt => rt.User).WithMany(u => u.RefreshTokens).HasForeignKey(rt => rt.UserId).OnDelete(DeleteBehavior.NoAction);
            e.HasOne(rt => rt.ReplacedByToken).WithMany(rt => rt.ReplacedTokens).HasForeignKey(rt => rt.ReplacedByTokenId).OnDelete(DeleteBehavior.NoAction);
        });

        modelBuilder.Entity<PasswordResetToken>(e =>
        {
            e.ToTable("PasswordResetTokens");
            e.HasKey(pr => pr.Id);
            e.Property(pr => pr.Id).HasDefaultValueSql("NEWSEQUENTIALID()");
            e.Property(pr => pr.TokenHash).IsRequired();
            e.HasIndex(pr => pr.TokenHash).IsUnique();
            e.Property(pr => pr.CreatedAt).HasDefaultValueSql("GETUTCDATE()");
            e.HasOne(pr => pr.User).WithMany(u => u.PasswordResetTokens).HasForeignKey(pr => pr.UserId).OnDelete(DeleteBehavior.NoAction);
        });

        // ── Clinic ───────────────────────────────────────────────
        modelBuilder.Entity<Clinic>(e =>
        {
            e.ToTable("Clinics");
            e.HasKey(c => c.Id);
            e.Property(c => c.Id).HasDefaultValueSql("NEWSEQUENTIALID()");
            e.Property(c => c.Name).IsRequired();
            e.Property(c => c.CreatedAt).HasDefaultValueSql("GETUTCDATE()");
        });

        modelBuilder.Entity<Therapist>(e =>
        {
            e.ToTable("Therapists");
            e.HasKey(t => t.Id);
            e.Property(t => t.Id).HasDefaultValueSql("NEWSEQUENTIALID()");
            e.Property(t => t.UserId).IsRequired();
            e.HasIndex(t => t.UserId).IsUnique();
            e.Property(t => t.FullName).IsRequired();
            e.Property(t => t.LicenseNumber).IsRequired();
            e.HasIndex(t => t.LicenseNumber).IsUnique();
            e.Property(t => t.ApprovalStatus).HasMaxLength(20).HasDefaultValue(TherapistApprovalStatus.Pending);
            e.Property(t => t.CreatedAt).HasDefaultValueSql("GETUTCDATE()");
            e.HasOne(t => t.User).WithOne(u => u.Therapist).HasForeignKey<Therapist>(t => t.UserId).OnDelete(DeleteBehavior.NoAction);
        });

        modelBuilder.Entity<TherapistClinic>(e =>
        {
            e.ToTable("TherapistClinics");
            e.HasKey(tc => new { tc.TherapistId, tc.ClinicId });
            e.HasOne(tc => tc.Therapist).WithMany(t => t.TherapistClinics).HasForeignKey(tc => tc.TherapistId).OnDelete(DeleteBehavior.NoAction);
            e.HasOne(tc => tc.Clinic).WithMany(c => c.TherapistClinics).HasForeignKey(tc => tc.ClinicId).OnDelete(DeleteBehavior.NoAction);
        });

        // ── Patient ──────────────────────────────────────────────
        modelBuilder.Entity<Patient>(e =>
        {
            e.ToTable("Patients");
            e.HasKey(p => p.Id);
            e.Property(p => p.Id).HasDefaultValueSql("NEWSEQUENTIALID()");
            e.Property(p => p.FullName).IsRequired();
            e.Property(p => p.Status).HasDefaultValue("Active");
            e.HasIndex(p => p.UserId).IsUnique();
            e.Property(p => p.CreatedAt).HasDefaultValueSql("GETUTCDATE()");
            e.Property(p => p.UpdatedAt).HasDefaultValueSql("GETUTCDATE()");
            e.HasOne(p => p.Therapist).WithMany(t => t.Patients).HasForeignKey(p => p.TherapistId).OnDelete(DeleteBehavior.NoAction);
            e.HasOne(p => p.Clinic).WithMany(c => c.Patients).HasForeignKey(p => p.ClinicId).OnDelete(DeleteBehavior.SetNull);
            e.HasOne(p => p.User).WithOne(u => u.Patient).HasForeignKey<Patient>(p => p.UserId).OnDelete(DeleteBehavior.SetNull);
        });

        modelBuilder.Entity<PatientInvitation>(e =>
        {
            e.ToTable("PatientInvitations");
            e.HasKey(pi => pi.Id);
            e.Property(pi => pi.Id).HasDefaultValueSql("NEWSEQUENTIALID()");
            e.Property(pi => pi.InviteTokenHash).IsRequired();
            e.HasIndex(pi => pi.InviteTokenHash).IsUnique();
            e.Property(pi => pi.CreatedAt).HasDefaultValueSql("GETUTCDATE()");
            e.HasOne(pi => pi.Patient).WithMany(p => p.PatientInvitations).HasForeignKey(pi => pi.PatientId).OnDelete(DeleteBehavior.NoAction);
        });

        // ── Intake Forms ─────────────────────────────────────────
        modelBuilder.Entity<IntakeForm>(e =>
        {
            e.ToTable("IntakeForms");
            e.HasKey(f => f.Id);
            e.Property(f => f.Id).HasDefaultValueSql("NEWSEQUENTIALID()");
            e.Property(f => f.Status).HasDefaultValue("Draft");
            e.Property(f => f.CreatedAt).HasDefaultValueSql("GETUTCDATE()");
            e.HasOne(f => f.Patient).WithMany(p => p.IntakeForms).HasForeignKey(f => f.PatientId).OnDelete(DeleteBehavior.NoAction);
        });

        modelBuilder.Entity<IntakeFormOcrExtraction>(e =>
        {
            e.ToTable("IntakeFormOcrExtractions");
            e.HasKey(o => o.Id);
            e.Property(o => o.Id).HasDefaultValueSql("NEWSEQUENTIALID()");
            e.Property(o => o.ExtractedJson).HasColumnType("nvarchar(max)");
            e.Property(o => o.Confidence).HasPrecision(18, 6);
            e.Property(o => o.CreatedAt).HasDefaultValueSql("GETUTCDATE()");
            e.Property(o => o.UpdatedAt).HasDefaultValueSql("GETUTCDATE()");
            e.HasOne(o => o.IntakeForm).WithMany(f => f.OcrExtractions).HasForeignKey(o => o.IntakeFormId).OnDelete(DeleteBehavior.NoAction);
        });

        // ── Session ──────────────────────────────────────────────
        modelBuilder.Entity<Session>(e =>
        {
            e.ToTable("Sessions");
            e.HasKey(s => s.Id);
            e.Property(s => s.Id).HasDefaultValueSql("NEWSEQUENTIALID()");
            e.Property(s => s.Status).HasDefaultValue("Draft");
            e.Property(s => s.SessionDate).HasColumnType("date");
            e.Property(s => s.CreatedAt).HasDefaultValueSql("GETUTCDATE()");
            e.Property(s => s.UpdatedAt).HasDefaultValueSql("GETUTCDATE()");
            e.HasIndex(s => s.PatientId);
            e.HasOne(s => s.Patient).WithMany(p => p.Sessions).HasForeignKey(s => s.PatientId).OnDelete(DeleteBehavior.NoAction);
            e.HasOne(s => s.IntakeForm).WithMany(f => f.Sessions).HasForeignKey(s => s.IntakeFormId).OnDelete(DeleteBehavior.SetNull);
        });

        modelBuilder.Entity<SessionNote>(e =>
        {
            e.ToTable("SessionNotes");
            e.HasKey(n => n.Id);
            e.Property(n => n.Id).HasDefaultValueSql("NEWSEQUENTIALID()");
            e.Property(n => n.CreatedAt).HasDefaultValueSql("GETUTCDATE()");
            e.Property(n => n.UpdatedAt).HasDefaultValueSql("GETUTCDATE()");
            e.HasIndex(n => n.SessionId).IsUnique();
            e.HasOne(n => n.Session).WithOne(s => s.SessionNote).HasForeignKey<SessionNote>(n => n.SessionId).OnDelete(DeleteBehavior.NoAction);
        });

        modelBuilder.Entity<SessionEmbedding>(e =>
        {
            e.ToTable("SessionEmbeddings");
            e.HasKey(se => se.Id);
            e.Property(se => se.Id).HasDefaultValueSql("NEWSEQUENTIALID()");
            e.Property(se => se.CreatedAt).HasDefaultValueSql("GETUTCDATE()");
            e.HasIndex(se => new { se.SessionId, se.ChunkIndex }).IsUnique();
            e.HasOne(se => se.Session).WithMany(s => s.SessionEmbeddings).HasForeignKey(se => se.SessionId).OnDelete(DeleteBehavior.NoAction);
        });

        modelBuilder.Entity<VoiceMemo>(e =>
        {
            e.ToTable("VoiceMemos");
            e.HasKey(vm => vm.Id);
            e.Property(vm => vm.Id).HasDefaultValueSql("NEWSEQUENTIALID()");
            e.Property(vm => vm.CreatedAt).HasDefaultValueSql("GETUTCDATE()");
            e.HasOne(vm => vm.Session).WithMany(s => s.VoiceMemos).HasForeignKey(vm => vm.SessionId).OnDelete(DeleteBehavior.NoAction);
        });

        // ── Files ────────────────────────────────────────────────
        modelBuilder.Entity<UploadedFile>(e =>
        {
            e.ToTable("UploadedFiles");
            e.HasKey(f => f.Id);
            e.Property(f => f.Id).HasDefaultValueSql("NEWSEQUENTIALID()");
            e.Property(f => f.BlobUrl).IsRequired();
            e.Property(f => f.CreatedAt).HasDefaultValueSql("GETUTCDATE()");
            e.HasOne(f => f.Patient).WithMany().HasForeignKey(f => f.PatientId).OnDelete(DeleteBehavior.SetNull);
            e.HasOne(f => f.Session).WithMany(s => s.UploadedFiles).HasForeignKey(f => f.SessionId).OnDelete(DeleteBehavior.SetNull);
            e.HasOne(f => f.UploadedByUser).WithMany(u => u.UploadedFiles).HasForeignKey(f => f.UploadedByUserId).OnDelete(DeleteBehavior.NoAction);
        });

        // ── Notifications ────────────────────────────────────────
        modelBuilder.Entity<Notification>(e =>
        {
            e.ToTable("Notifications");
            e.HasKey(n => n.Id);
            e.Property(n => n.Id).HasDefaultValueSql("NEWSEQUENTIALID()");
            e.Property(n => n.IsRead).HasDefaultValue(false);
            e.Property(n => n.CreatedAt).HasDefaultValueSql("GETUTCDATE()");
            e.HasIndex(n => n.RecipientUserId);
            e.HasOne(n => n.RecipientUser).WithMany(u => u.Notifications).HasForeignKey(n => n.RecipientUserId).OnDelete(DeleteBehavior.NoAction);
        });

        // ── Audit ────────────────────────────────────────────────
        modelBuilder.Entity<AuditLog>(e =>
        {
            e.ToTable("AuditLogs");
            e.HasKey(al => al.Id);
            e.Property(al => al.Id).HasDefaultValueSql("NEWSEQUENTIALID()");
            e.Property(al => al.OldValues).HasColumnType("nvarchar(max)");
            e.Property(al => al.NewValues).HasColumnType("nvarchar(max)");
            e.Property(al => al.OccurredAt).HasDefaultValueSql("GETUTCDATE()");
            e.HasOne(al => al.User).WithMany(u => u.AuditLogs).HasForeignKey(al => al.UserId).OnDelete(DeleteBehavior.SetNull);
        });

        // ── Assessment ───────────────────────────────────────────
        modelBuilder.Entity<AssessmentTemplate>(e =>
        {
            e.ToTable("AssessmentTemplates");
            e.HasKey(at => at.Id);
            e.Property(at => at.Id).HasDefaultValueSql("NEWSEQUENTIALID()");
            e.Property(at => at.Name).IsRequired();
            e.Property(at => at.CreatedAt).HasDefaultValueSql("GETUTCDATE()");
        });

        modelBuilder.Entity<AssessmentQuestion>(e =>
        {
            e.ToTable("AssessmentQuestions");
            e.HasKey(aq => aq.Id);
            e.Property(aq => aq.Id).HasDefaultValueSql("NEWSEQUENTIALID()");
            e.Property(aq => aq.QuestionText).IsRequired();
            e.Property(aq => aq.CreatedAt).HasDefaultValueSql("GETUTCDATE()");
            e.HasOne(aq => aq.Template).WithMany(t => t.Questions).HasForeignKey(aq => aq.TemplateId).OnDelete(DeleteBehavior.NoAction);
        });

        modelBuilder.Entity<Assessment>(e =>
        {
            e.ToTable("Assessments");
            e.HasKey(a => a.Id);
            e.Property(a => a.Id).HasDefaultValueSql("NEWSEQUENTIALID()");
            e.Property(a => a.AssessmentDate).HasColumnType("date");
            e.Property(a => a.TotalScore).HasPrecision(18, 6);
            e.Property(a => a.Status).HasDefaultValue("Active");
            e.Property(a => a.CreatedAt).HasDefaultValueSql("GETUTCDATE()");
            e.Property(a => a.UpdatedAt).HasDefaultValueSql("GETUTCDATE()");
            e.HasOne(a => a.Patient).WithMany(p => p.Assessments).HasForeignKey(a => a.PatientId).OnDelete(DeleteBehavior.NoAction);
            e.HasOne(a => a.Session).WithMany(s => s.Assessments).HasForeignKey(a => a.SessionId).OnDelete(DeleteBehavior.SetNull);
            e.HasOne(a => a.Template).WithMany(t => t.Assessments).HasForeignKey(a => a.TemplateId).OnDelete(DeleteBehavior.NoAction);
        });

        modelBuilder.Entity<AssessmentResponse>(e =>
        {
            e.ToTable("AssessmentResponses");
            e.HasKey(ar => ar.Id);
            e.Property(ar => ar.Id).HasDefaultValueSql("NEWSEQUENTIALID()");
            e.Property(ar => ar.AnswerNumber).HasPrecision(18, 6);
            e.Property(ar => ar.CreatedAt).HasDefaultValueSql("GETUTCDATE()");
            e.HasOne(ar => ar.Assessment).WithMany(a => a.Responses).HasForeignKey(ar => ar.AssessmentId).OnDelete(DeleteBehavior.NoAction);
            e.HasOne(ar => ar.Question).WithMany(q => q.Responses).HasForeignKey(ar => ar.QuestionId).OnDelete(DeleteBehavior.NoAction);
        });

        // ── Exercise ─────────────────────────────────────────────
        modelBuilder.Entity<Exercise>(e =>
        {
            e.ToTable("Exercises");
            e.HasKey(ex => ex.Id);
            e.Property(ex => ex.Id).HasDefaultValueSql("NEWSEQUENTIALID()");
            e.Property(ex => ex.Status).HasDefaultValue("Active");
            e.Property(ex => ex.CreatedAt).HasDefaultValueSql("GETUTCDATE()");
            e.Property(ex => ex.UpdatedAt).HasDefaultValueSql("GETUTCDATE()");
            e.HasIndex(ex => ex.PatientId);
            e.HasOne(ex => ex.Patient).WithMany(p => p.Exercises).HasForeignKey(ex => ex.PatientId).OnDelete(DeleteBehavior.NoAction);
        });

        modelBuilder.Entity<ExerciseLog>(e =>
        {
            e.ToTable("ExerciseLogs");
            e.HasKey(el => el.Id);
            e.Property(el => el.Id).HasDefaultValueSql("NEWSEQUENTIALID()");
            e.Property(el => el.CreatedAt).HasDefaultValueSql("GETUTCDATE()");
            e.HasOne(el => el.Exercise).WithMany(ex => ex.ExerciseLogs).HasForeignKey(el => el.ExerciseId).OnDelete(DeleteBehavior.NoAction);
            e.HasOne(el => el.Patient).WithMany(p => p.ExerciseLogs).HasForeignKey(el => el.PatientId).OnDelete(DeleteBehavior.NoAction);
        });

        // ── Chat ─────────────────────────────────────────────────
        modelBuilder.Entity<ChatConversation>(e =>
        {
            e.ToTable("ChatConversations");
            e.HasKey(cc => cc.Id);
            e.Property(cc => cc.Id).HasDefaultValueSql("NEWSEQUENTIALID()");
            e.Property(cc => cc.Status).HasDefaultValue("Open");
            e.Property(cc => cc.CreatedAt).HasDefaultValueSql("GETUTCDATE()");
            e.Property(cc => cc.UpdatedAt).HasDefaultValueSql("GETUTCDATE()");
            e.HasOne(cc => cc.Patient).WithMany(p => p.ChatConversations).HasForeignKey(cc => cc.PatientId).OnDelete(DeleteBehavior.NoAction);
        });

        modelBuilder.Entity<ChatMessage>(e =>
        {
            e.ToTable("ChatMessages");
            e.HasKey(cm => cm.Id);
            e.Property(cm => cm.Id).HasDefaultValueSql("NEWSEQUENTIALID()");
            e.Property(cm => cm.CreatedAt).HasDefaultValueSql("GETUTCDATE()");
            e.HasIndex(cm => cm.ConversationId);
            e.HasOne(cm => cm.Conversation).WithMany(cc => cc.ChatMessages).HasForeignKey(cm => cm.ConversationId).OnDelete(DeleteBehavior.NoAction);
        });

        modelBuilder.Entity<AiChatLog>(e =>
        {
            e.ToTable("AiChatLogs");
            e.HasKey(acl => acl.Id);
            e.Property(acl => acl.Id).HasDefaultValueSql("NEWSEQUENTIALID()");
            e.Property(acl => acl.Cost).HasPrecision(18, 6);
            e.Property(acl => acl.CreatedAt).HasDefaultValueSql("GETUTCDATE()");
            e.HasOne(acl => acl.Conversation).WithMany(cc => cc.AiChatLogs).HasForeignKey(acl => acl.ConversationId).OnDelete(DeleteBehavior.NoAction);
            e.HasOne(acl => acl.Patient).WithMany(p => p.AiChatLogs).HasForeignKey(acl => acl.PatientId).OnDelete(DeleteBehavior.NoAction);
        });

        // ── Crisis ───────────────────────────────────────────────
        modelBuilder.Entity<CrisisAlert>(e =>
        {
            e.ToTable("CrisisAlerts");
            e.HasKey(ca => ca.Id);
            e.Property(ca => ca.Id).HasDefaultValueSql("NEWSEQUENTIALID()");
            e.Property(ca => ca.Status).HasDefaultValue("Open");
            e.Property(ca => ca.CreatedAt).HasDefaultValueSql("GETUTCDATE()");
            e.Property(ca => ca.UpdatedAt).HasDefaultValueSql("GETUTCDATE()");
            e.HasOne(ca => ca.Patient).WithMany(p => p.CrisisAlerts).HasForeignKey(ca => ca.PatientId).OnDelete(DeleteBehavior.NoAction);
            e.HasOne(ca => ca.ChatMessage).WithOne(cm => cm.TriggeredCrisisAlert).HasForeignKey<CrisisAlert>(ca => ca.ChatMessageId).OnDelete(DeleteBehavior.SetNull);
        });

        // ── Reports ──────────────────────────────────────────────
        modelBuilder.Entity<ReferralReport>(e =>
        {
            e.ToTable("ReferralReports");
            e.HasKey(rr => rr.Id);
            e.Property(rr => rr.Id).HasDefaultValueSql("NEWSEQUENTIALID()");
            e.Property(rr => rr.Status).HasDefaultValue("Draft");
            e.Property(rr => rr.CreatedAt).HasDefaultValueSql("GETUTCDATE()");
            e.Property(rr => rr.UpdatedAt).HasDefaultValueSql("GETUTCDATE()");
            e.HasOne(rr => rr.Patient).WithMany(p => p.ReferralReports).HasForeignKey(rr => rr.PatientId).OnDelete(DeleteBehavior.NoAction);
            e.HasOne(rr => rr.Therapist).WithMany(t => t.ReferralReports).HasForeignKey(rr => rr.TherapistId).OnDelete(DeleteBehavior.NoAction);
            e.HasOne(rr => rr.GeneratedByTherapist).WithMany().HasForeignKey(rr => rr.GeneratedByTherapistId).OnDelete(DeleteBehavior.NoAction);
            e.HasOne(rr => rr.CurrentVersion).WithMany().HasForeignKey(rr => rr.CurrentVersionId).OnDelete(DeleteBehavior.SetNull);
        });

        modelBuilder.Entity<ReportVersion>(e =>
        {
            e.ToTable("ReportVersions");
            e.HasKey(rv => rv.Id);
            e.Property(rv => rv.Id).HasDefaultValueSql("NEWSEQUENTIALID()");
            e.Property(rv => rv.CreatedAt).HasDefaultValueSql("GETUTCDATE()");
            e.HasOne(rv => rv.Report).WithMany(rr => rr.Versions).HasForeignKey(rv => rv.ReportId).OnDelete(DeleteBehavior.NoAction);
            e.HasOne(rv => rv.CreatedByTherapist).WithMany(t => t.ReportVersions).HasForeignKey(rv => rv.CreatedByTherapistId).OnDelete(DeleteBehavior.NoAction);
        });

        // ── System ───────────────────────────────────────────────
        modelBuilder.Entity<SystemSetting>(e =>
        {
            e.ToTable("SystemSettings");
            e.HasKey(ss => ss.Key);
            e.Property(ss => ss.Key).HasMaxLength(200);
            e.Property(ss => ss.Value).HasColumnType("nvarchar(max)");
            e.Property(ss => ss.UpdatedAt).HasDefaultValueSql("GETUTCDATE()");
        });

        // ── AI ───────────────────────────────────────────────────
        modelBuilder.Entity<AiArtifact>(e =>
        {
            e.ToTable("AiArtifacts");
            e.HasKey(aa => aa.Id);
            e.Property(aa => aa.Id).HasDefaultValueSql("NEWSEQUENTIALID()");
            e.Property(aa => aa.CreatedAt).HasDefaultValueSql("GETUTCDATE()");
            e.HasOne(aa => aa.Conversation).WithMany(cc => cc.AiArtifacts).HasForeignKey(aa => aa.ConversationId).OnDelete(DeleteBehavior.NoAction);
            e.HasOne(aa => aa.Patient).WithMany(p => p.AiArtifacts).HasForeignKey(aa => aa.PatientId).OnDelete(DeleteBehavior.NoAction);
            e.HasOne(aa => aa.TriggerMessage).WithOne(cm => cm.TriggeredArtifact).HasForeignKey<AiArtifact>(aa => aa.TriggerMessageId).OnDelete(DeleteBehavior.NoAction);
        });

        modelBuilder.Entity<AiReportGenerationLog>(e =>
        {
            e.ToTable("AiReportGenerationLogs");
            e.HasKey(argl => argl.Id);
            e.Property(argl => argl.Id).HasDefaultValueSql("NEWSEQUENTIALID()");
            e.Property(argl => argl.CostEstimate).HasPrecision(18, 6);
            e.Property(argl => argl.CreatedAt).HasDefaultValueSql("GETUTCDATE()");
            e.HasOne(argl => argl.Patient).WithMany(p => p.AiReportGenerationLogs).HasForeignKey(argl => argl.PatientId).OnDelete(DeleteBehavior.NoAction);
            e.HasOne(argl => argl.Report).WithMany(rr => rr.AiReportGenerationLogs).HasForeignKey(argl => argl.ReportId).OnDelete(DeleteBehavior.SetNull);
            e.HasOne(argl => argl.GeneratedByTherapist).WithMany(t => t.AiReportGenerationLogs).HasForeignKey(argl => argl.GeneratedByTherapistId).OnDelete(DeleteBehavior.NoAction);
        });
    }
}
