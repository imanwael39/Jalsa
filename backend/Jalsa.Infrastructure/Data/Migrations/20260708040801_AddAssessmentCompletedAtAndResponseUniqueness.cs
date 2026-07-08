using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Jalsa.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddAssessmentCompletedAtAndResponseUniqueness : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_AssessmentResponses_AssessmentId",
                table: "AssessmentResponses");

            migrationBuilder.AddColumn<DateTime>(
                name: "CompletedAt",
                table: "Assessments",
                type: "datetime2",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_AssessmentResponses_AssessmentId_QuestionId",
                table: "AssessmentResponses",
                columns: new[] { "AssessmentId", "QuestionId" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_AssessmentResponses_AssessmentId_QuestionId",
                table: "AssessmentResponses");

            migrationBuilder.DropColumn(
                name: "CompletedAt",
                table: "Assessments");

            migrationBuilder.CreateIndex(
                name: "IX_AssessmentResponses_AssessmentId",
                table: "AssessmentResponses",
                column: "AssessmentId");
        }
    }
}
