using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Jalsa.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddExerciseMoodAndAssessmentSeverityFields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Difficulty",
                table: "Exercises",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "DurationMinutes",
                table: "Exercises",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "MoodAfter",
                table: "ExerciseLogs",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "MoodBefore",
                table: "ExerciseLogs",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Notes",
                table: "Assessments",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Severity",
                table: "Assessments",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Difficulty",
                table: "Exercises");

            migrationBuilder.DropColumn(
                name: "DurationMinutes",
                table: "Exercises");

            migrationBuilder.DropColumn(
                name: "MoodAfter",
                table: "ExerciseLogs");

            migrationBuilder.DropColumn(
                name: "MoodBefore",
                table: "ExerciseLogs");

            migrationBuilder.DropColumn(
                name: "Notes",
                table: "Assessments");

            migrationBuilder.DropColumn(
                name: "Severity",
                table: "Assessments");
        }
    }
}
