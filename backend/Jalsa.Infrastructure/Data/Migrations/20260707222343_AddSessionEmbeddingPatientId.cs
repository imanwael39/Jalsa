using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Jalsa.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddSessionEmbeddingPatientId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_SessionEmbeddings_SessionId_ChunkIndex",
                table: "SessionEmbeddings");

            migrationBuilder.AddColumn<Guid>(
                name: "PatientId",
                table: "SessionEmbeddings",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<string>(
                name: "Source",
                table: "SessionEmbeddings",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "SessionNote");

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedAt",
                table: "SessionEmbeddings",
                type: "datetime2",
                nullable: false,
                defaultValueSql: "GETUTCDATE()");

            migrationBuilder.CreateIndex(
                name: "IX_SessionEmbeddings_PatientId",
                table: "SessionEmbeddings",
                column: "PatientId");

            migrationBuilder.CreateIndex(
                name: "IX_SessionEmbeddings_SessionId_ChunkIndex_Source",
                table: "SessionEmbeddings",
                columns: new[] { "SessionId", "ChunkIndex", "Source" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_SessionEmbeddings_PatientId",
                table: "SessionEmbeddings");

            migrationBuilder.DropIndex(
                name: "IX_SessionEmbeddings_SessionId_ChunkIndex_Source",
                table: "SessionEmbeddings");

            migrationBuilder.DropColumn(
                name: "PatientId",
                table: "SessionEmbeddings");

            migrationBuilder.DropColumn(
                name: "Source",
                table: "SessionEmbeddings");

            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                table: "SessionEmbeddings");

            migrationBuilder.CreateIndex(
                name: "IX_SessionEmbeddings_SessionId_ChunkIndex",
                table: "SessionEmbeddings",
                columns: new[] { "SessionId", "ChunkIndex" },
                unique: true);
        }
    }
}
