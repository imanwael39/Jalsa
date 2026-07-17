using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Jalsa.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class SplitTherapistAndPatientChat : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Old FK must go before the old tables can be dropped later; the new one
            // targeting PatientSupportMessages is added after that table exists and is
            // populated (see below).
            migrationBuilder.DropForeignKey(
                name: "FK_CrisisAlerts_ChatMessages_ChatMessageId",
                table: "CrisisAlerts");

            migrationBuilder.CreateTable(
                name: "PatientSupportConversations",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWSEQUENTIALID()"),
                    PatientId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: false, defaultValue: "Open"),
                    LastActivityAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PatientSupportConversations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PatientSupportConversations_Patients_PatientId",
                        column: x => x.PatientId,
                        principalTable: "Patients",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "TherapistAiConversations",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWSEQUENTIALID()"),
                    TherapistId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PatientId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: false, defaultValue: "Open"),
                    LastActivityAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TherapistAiConversations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TherapistAiConversations_Patients_PatientId",
                        column: x => x.PatientId,
                        principalTable: "Patients",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_TherapistAiConversations_Therapists_TherapistId",
                        column: x => x.TherapistId,
                        principalTable: "Therapists",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "PatientSupportAiChatLogs",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWSEQUENTIALID()"),
                    ConversationId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PatientId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TokensUsed = table.Column<int>(type: "int", nullable: true),
                    Cost = table.Column<decimal>(type: "decimal(18,6)", precision: 18, scale: 6, nullable: true),
                    ResponseLatencyMs = table.Column<int>(type: "int", nullable: true),
                    ModelUsed = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PatientSupportAiChatLogs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PatientSupportAiChatLogs_PatientSupportConversations_ConversationId",
                        column: x => x.ConversationId,
                        principalTable: "PatientSupportConversations",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_PatientSupportAiChatLogs_Patients_PatientId",
                        column: x => x.PatientId,
                        principalTable: "Patients",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "PatientSupportMessages",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWSEQUENTIALID()"),
                    ConversationId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SenderType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Content = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PatientSupportMessages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PatientSupportMessages_PatientSupportConversations_ConversationId",
                        column: x => x.ConversationId,
                        principalTable: "PatientSupportConversations",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "TherapistAiChatLogs",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWSEQUENTIALID()"),
                    ConversationId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PatientId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TokensUsed = table.Column<int>(type: "int", nullable: true),
                    Cost = table.Column<decimal>(type: "decimal(18,6)", precision: 18, scale: 6, nullable: true),
                    ResponseLatencyMs = table.Column<int>(type: "int", nullable: true),
                    ModelUsed = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TherapistAiChatLogs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TherapistAiChatLogs_Patients_PatientId",
                        column: x => x.PatientId,
                        principalTable: "Patients",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_TherapistAiChatLogs_TherapistAiConversations_ConversationId",
                        column: x => x.ConversationId,
                        principalTable: "TherapistAiConversations",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "TherapistAiMessages",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWSEQUENTIALID()"),
                    ConversationId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SenderType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Content = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TherapistAiMessages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TherapistAiMessages_TherapistAiConversations_ConversationId",
                        column: x => x.ConversationId,
                        principalTable: "TherapistAiConversations",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "PatientSupportMemories",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWSEQUENTIALID()"),
                    ConversationId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PatientId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TriggerMessageId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ContentText = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EmbeddingVector = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PatientSupportMemories", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PatientSupportMemories_PatientSupportConversations_ConversationId",
                        column: x => x.ConversationId,
                        principalTable: "PatientSupportConversations",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_PatientSupportMemories_PatientSupportMessages_TriggerMessageId",
                        column: x => x.TriggerMessageId,
                        principalTable: "PatientSupportMessages",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_PatientSupportMemories_Patients_PatientId",
                        column: x => x.PatientId,
                        principalTable: "Patients",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "TherapistAiMemories",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWSEQUENTIALID()"),
                    ConversationId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PatientId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TriggerMessageId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ContentText = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EmbeddingVector = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TherapistAiMemories", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TherapistAiMemories_Patients_PatientId",
                        column: x => x.PatientId,
                        principalTable: "Patients",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_TherapistAiMemories_TherapistAiConversations_ConversationId",
                        column: x => x.ConversationId,
                        principalTable: "TherapistAiConversations",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_TherapistAiMemories_TherapistAiMessages_TriggerMessageId",
                        column: x => x.TriggerMessageId,
                        principalTable: "TherapistAiMessages",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_PatientSupportAiChatLogs_ConversationId",
                table: "PatientSupportAiChatLogs",
                column: "ConversationId");

            migrationBuilder.CreateIndex(
                name: "IX_PatientSupportAiChatLogs_PatientId",
                table: "PatientSupportAiChatLogs",
                column: "PatientId");

            migrationBuilder.CreateIndex(
                name: "IX_PatientSupportConversations_PatientId",
                table: "PatientSupportConversations",
                column: "PatientId");

            migrationBuilder.CreateIndex(
                name: "IX_PatientSupportMemories_ConversationId",
                table: "PatientSupportMemories",
                column: "ConversationId");

            migrationBuilder.CreateIndex(
                name: "IX_PatientSupportMemories_PatientId",
                table: "PatientSupportMemories",
                column: "PatientId");

            migrationBuilder.CreateIndex(
                name: "IX_PatientSupportMemories_TriggerMessageId",
                table: "PatientSupportMemories",
                column: "TriggerMessageId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_PatientSupportMessages_ConversationId",
                table: "PatientSupportMessages",
                column: "ConversationId");

            migrationBuilder.CreateIndex(
                name: "IX_TherapistAiChatLogs_ConversationId",
                table: "TherapistAiChatLogs",
                column: "ConversationId");

            migrationBuilder.CreateIndex(
                name: "IX_TherapistAiChatLogs_PatientId",
                table: "TherapistAiChatLogs",
                column: "PatientId");

            migrationBuilder.CreateIndex(
                name: "IX_TherapistAiConversations_PatientId",
                table: "TherapistAiConversations",
                column: "PatientId");

            migrationBuilder.CreateIndex(
                name: "IX_TherapistAiConversations_TherapistId_PatientId",
                table: "TherapistAiConversations",
                columns: new[] { "TherapistId", "PatientId" });

            migrationBuilder.CreateIndex(
                name: "IX_TherapistAiMemories_ConversationId",
                table: "TherapistAiMemories",
                column: "ConversationId");

            migrationBuilder.CreateIndex(
                name: "IX_TherapistAiMemories_PatientId",
                table: "TherapistAiMemories",
                column: "PatientId");

            migrationBuilder.CreateIndex(
                name: "IX_TherapistAiMemories_TriggerMessageId",
                table: "TherapistAiMemories",
                column: "TriggerMessageId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_TherapistAiMessages_ConversationId",
                table: "TherapistAiMessages",
                column: "ConversationId");

            // ── Data-preserving backfill ──────────────────────────────────────
            // Splits every existing ChatConversation into a PatientSupportConversation
            // (always, since every conversation has at least Patient/AI turns) and,
            // where the conversation also contains Therapist-authored messages, a
            // separate synthetic TherapistAiConversation. Every AI reply is attributed to
            // whichever side asked the immediately preceding question — safe because both
            // the real send flow and the seed data write messages in strict alternation
            // (human turn, then its AI reply), so a message's own predecessor by CreatedAt
            // is always the human turn it answered. Message ids are preserved across the
            // split so memory (AiArtifacts) and crisis alerts join back unambiguously.
            migrationBuilder.Sql(@"
                DECLARE @ConvMap TABLE (OldConversationId UNIQUEIDENTIFIER, NewConversationId UNIQUEIDENTIFIER, TherapistId UNIQUEIDENTIFIER, PatientId UNIQUEIDENTIFIER);

                INSERT INTO @ConvMap (OldConversationId, NewConversationId, TherapistId, PatientId)
                SELECT DISTINCT cc.Id, NEWID(), p.TherapistId, cc.PatientId
                FROM ChatConversations cc
                JOIN Patients p ON p.Id = cc.PatientId
                WHERE EXISTS (SELECT 1 FROM ChatMessages cm WHERE cm.ConversationId = cc.Id AND cm.SenderType = 'Therapist');

                INSERT INTO TherapistAiConversations (Id, TherapistId, PatientId, Status, LastActivityAt, CreatedAt, UpdatedAt)
                SELECT m.NewConversationId, m.TherapistId, m.PatientId, cc.Status, cc.LastActivityAt,
                       (SELECT MIN(cm.CreatedAt) FROM ChatMessages cm WHERE cm.ConversationId = cc.Id AND cm.SenderType = 'Therapist'),
                       cc.UpdatedAt
                FROM @ConvMap m
                JOIN ChatConversations cc ON cc.Id = m.OldConversationId;

                INSERT INTO PatientSupportConversations (Id, PatientId, Status, LastActivityAt, CreatedAt, UpdatedAt)
                SELECT cc.Id, cc.PatientId, cc.Status, cc.LastActivityAt, cc.CreatedAt, cc.UpdatedAt
                FROM ChatConversations cc;

                ;WITH Attributed AS (
                    SELECT cm.Id, cm.ConversationId, cm.SenderType, cm.Content, cm.CreatedAt,
                           LAG(cm.SenderType) OVER (PARTITION BY cm.ConversationId ORDER BY cm.CreatedAt, cm.Id) AS PrevSender
                    FROM ChatMessages cm
                )
                INSERT INTO TherapistAiMessages (Id, ConversationId, SenderType, Content, CreatedAt)
                SELECT a.Id, m.NewConversationId, a.SenderType, a.Content, a.CreatedAt
                FROM Attributed a
                JOIN @ConvMap m ON m.OldConversationId = a.ConversationId
                WHERE a.SenderType = 'Therapist' OR (a.SenderType = 'AI' AND a.PrevSender = 'Therapist');

                ;WITH Attributed AS (
                    SELECT cm.Id, cm.ConversationId, cm.SenderType, cm.Content, cm.CreatedAt,
                           LAG(cm.SenderType) OVER (PARTITION BY cm.ConversationId ORDER BY cm.CreatedAt, cm.Id) AS PrevSender
                    FROM ChatMessages cm
                )
                INSERT INTO PatientSupportMessages (Id, ConversationId, SenderType, Content, CreatedAt)
                SELECT a.Id, a.ConversationId, a.SenderType, a.Content, a.CreatedAt
                FROM Attributed a
                WHERE a.SenderType = 'Patient' OR (a.SenderType = 'AI' AND (a.PrevSender = 'Patient' OR a.PrevSender IS NULL));

                -- AiChatLogs telemetry has no per-message linkage, only ConversationId; a
                -- conversation that produced any Therapist messages attributes its telemetry
                -- to the therapist side, everything else to patient support.
                INSERT INTO PatientSupportAiChatLogs (Id, ConversationId, PatientId, TokensUsed, Cost, ResponseLatencyMs, ModelUsed, CreatedAt)
                SELECT acl.Id, acl.ConversationId, acl.PatientId, acl.TokensUsed, acl.Cost, acl.ResponseLatencyMs, acl.ModelUsed, acl.CreatedAt
                FROM AiChatLogs acl
                WHERE NOT EXISTS (SELECT 1 FROM @ConvMap m WHERE m.OldConversationId = acl.ConversationId);

                INSERT INTO TherapistAiChatLogs (Id, ConversationId, PatientId, TokensUsed, Cost, ResponseLatencyMs, ModelUsed, CreatedAt)
                SELECT acl.Id, m.NewConversationId, acl.PatientId, acl.TokensUsed, acl.Cost, acl.ResponseLatencyMs, acl.ModelUsed, acl.CreatedAt
                FROM AiChatLogs acl
                JOIN @ConvMap m ON m.OldConversationId = acl.ConversationId;

                -- AiArtifacts (semantic memory) attribute exactly via TriggerMessageId, which
                -- was preserved verbatim into whichever new message table now holds it.
                INSERT INTO PatientSupportMemories (Id, ConversationId, PatientId, TriggerMessageId, ContentText, EmbeddingVector, CreatedAt)
                SELECT aa.Id, aa.ConversationId, aa.PatientId, aa.TriggerMessageId, aa.ContentText, aa.EmbeddingVector, aa.CreatedAt
                FROM AiArtifacts aa
                WHERE EXISTS (SELECT 1 FROM PatientSupportMessages psm WHERE psm.Id = aa.TriggerMessageId);

                INSERT INTO TherapistAiMemories (Id, ConversationId, PatientId, TriggerMessageId, ContentText, EmbeddingVector, CreatedAt)
                SELECT aa.Id, m.NewConversationId, aa.PatientId, aa.TriggerMessageId, aa.ContentText, aa.EmbeddingVector, aa.CreatedAt
                FROM AiArtifacts aa
                JOIN @ConvMap m ON m.OldConversationId = aa.ConversationId
                WHERE EXISTS (SELECT 1 FROM TherapistAiMessages tam WHERE tam.Id = aa.TriggerMessageId);
            ");

            // CrisisAlerts.ChatMessageId values are untouched — crisis detection only ever
            // fires on patient-authored messages, and those message ids were preserved
            // verbatim into PatientSupportMessages above, so the FK just needs repointing.
            migrationBuilder.AddForeignKey(
                name: "FK_CrisisAlerts_PatientSupportMessages_ChatMessageId",
                table: "CrisisAlerts",
                column: "ChatMessageId",
                principalTable: "PatientSupportMessages",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.DropTable(
                name: "AiArtifacts");

            migrationBuilder.DropTable(
                name: "AiChatLogs");

            migrationBuilder.DropTable(
                name: "ChatMessages");

            migrationBuilder.DropTable(
                name: "ChatConversations");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Best-effort: only PatientSupportConversations/Messages are reconstructed
            // (their ids are the original ChatConversations/ChatMessages ids). Any
            // therapist-originated conversations/messages created after this migration
            // ran, and all telemetry/memory rows, are not restorable on rollback — this
            // schema split should never have existed as one shared table, so a full
            // symmetric rollback is intentionally not supported.
            migrationBuilder.DropForeignKey(
                name: "FK_CrisisAlerts_PatientSupportMessages_ChatMessageId",
                table: "CrisisAlerts");

            migrationBuilder.DropTable(
                name: "PatientSupportAiChatLogs");

            migrationBuilder.DropTable(
                name: "PatientSupportMemories");

            migrationBuilder.DropTable(
                name: "TherapistAiChatLogs");

            migrationBuilder.DropTable(
                name: "TherapistAiMemories");

            migrationBuilder.DropTable(
                name: "TherapistAiMessages");

            migrationBuilder.DropTable(
                name: "TherapistAiConversations");

            migrationBuilder.CreateTable(
                name: "ChatConversations",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWSEQUENTIALID()"),
                    PatientId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    LastActivityAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: false, defaultValue: "Open"),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChatConversations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ChatConversations_Patients_PatientId",
                        column: x => x.PatientId,
                        principalTable: "Patients",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "AiChatLogs",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWSEQUENTIALID()"),
                    ConversationId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PatientId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Cost = table.Column<decimal>(type: "decimal(18,6)", precision: 18, scale: 6, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    ModelUsed = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ResponseLatencyMs = table.Column<int>(type: "int", nullable: true),
                    TokensUsed = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AiChatLogs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AiChatLogs_ChatConversations_ConversationId",
                        column: x => x.ConversationId,
                        principalTable: "ChatConversations",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_AiChatLogs_Patients_PatientId",
                        column: x => x.PatientId,
                        principalTable: "Patients",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "ChatMessages",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWSEQUENTIALID()"),
                    ConversationId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Content = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    LatencyMs = table.Column<int>(type: "int", nullable: true),
                    SenderType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TokensUsed = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChatMessages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ChatMessages_ChatConversations_ConversationId",
                        column: x => x.ConversationId,
                        principalTable: "ChatConversations",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "AiArtifacts",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWSEQUENTIALID()"),
                    ConversationId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PatientId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TriggerMessageId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ContentText = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    EmbeddingVector = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SourceId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    SourceType = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AiArtifacts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AiArtifacts_ChatConversations_ConversationId",
                        column: x => x.ConversationId,
                        principalTable: "ChatConversations",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_AiArtifacts_ChatMessages_TriggerMessageId",
                        column: x => x.TriggerMessageId,
                        principalTable: "ChatMessages",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_AiArtifacts_Patients_PatientId",
                        column: x => x.PatientId,
                        principalTable: "Patients",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_AiArtifacts_ConversationId",
                table: "AiArtifacts",
                column: "ConversationId");

            migrationBuilder.CreateIndex(
                name: "IX_AiArtifacts_PatientId",
                table: "AiArtifacts",
                column: "PatientId");

            migrationBuilder.CreateIndex(
                name: "IX_AiArtifacts_TriggerMessageId",
                table: "AiArtifacts",
                column: "TriggerMessageId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AiChatLogs_ConversationId",
                table: "AiChatLogs",
                column: "ConversationId");

            migrationBuilder.CreateIndex(
                name: "IX_AiChatLogs_PatientId",
                table: "AiChatLogs",
                column: "PatientId");

            migrationBuilder.CreateIndex(
                name: "IX_ChatConversations_PatientId",
                table: "ChatConversations",
                column: "PatientId");

            migrationBuilder.CreateIndex(
                name: "IX_ChatMessages_ConversationId",
                table: "ChatMessages",
                column: "ConversationId");

            migrationBuilder.Sql(@"
                INSERT INTO ChatConversations (Id, PatientId, Status, LastActivityAt, CreatedAt, UpdatedAt)
                SELECT Id, PatientId, Status, LastActivityAt, CreatedAt, UpdatedAt FROM PatientSupportConversations;

                INSERT INTO ChatMessages (Id, ConversationId, SenderType, Content, CreatedAt)
                SELECT Id, ConversationId, SenderType, Content, CreatedAt FROM PatientSupportMessages;

                INSERT INTO AiChatLogs (Id, ConversationId, PatientId, TokensUsed, Cost, ResponseLatencyMs, ModelUsed, CreatedAt)
                SELECT Id, ConversationId, PatientId, TokensUsed, Cost, ResponseLatencyMs, ModelUsed, CreatedAt FROM PatientSupportAiChatLogs;

                INSERT INTO AiArtifacts (Id, ConversationId, PatientId, TriggerMessageId, SourceType, ContentText, EmbeddingVector, CreatedAt)
                SELECT Id, ConversationId, PatientId, TriggerMessageId, 'Chat', ContentText, EmbeddingVector, CreatedAt FROM PatientSupportMemories;
            ");

            migrationBuilder.DropTable(
                name: "PatientSupportAiChatLogs");

            migrationBuilder.DropTable(
                name: "PatientSupportMemories");

            migrationBuilder.DropTable(
                name: "PatientSupportMessages");

            migrationBuilder.DropTable(
                name: "PatientSupportConversations");

            migrationBuilder.AddForeignKey(
                name: "FK_CrisisAlerts_ChatMessages_ChatMessageId",
                table: "CrisisAlerts",
                column: "ChatMessageId",
                principalTable: "ChatMessages",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }
    }
}
