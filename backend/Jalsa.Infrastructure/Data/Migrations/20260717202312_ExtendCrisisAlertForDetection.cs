using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Jalsa.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class ExtendCrisisAlertForDetection : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Status",
                table: "CrisisAlerts",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "New",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldDefaultValue: "Open");

            migrationBuilder.AddColumn<double>(
                name: "Confidence",
                table: "CrisisAlerts",
                type: "float(5)",
                precision: 5,
                scale: 4,
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "ConversationId",
                table: "CrisisAlerts",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Reason",
                table: "CrisisAlerts",
                type: "nvarchar(max)",
                nullable: true);

            // The Status vocabulary changed from Open/Acknowledged/Resolved to
            // New/Acknowledged/Resolved; only the column default changes automatically,
            // so existing rows need an explicit backfill.
            migrationBuilder.Sql("UPDATE CrisisAlerts SET Status = 'New' WHERE Status = 'Open';");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("UPDATE CrisisAlerts SET Status = 'Open' WHERE Status = 'New';");

            migrationBuilder.DropColumn(
                name: "Confidence",
                table: "CrisisAlerts");

            migrationBuilder.DropColumn(
                name: "ConversationId",
                table: "CrisisAlerts");

            migrationBuilder.DropColumn(
                name: "Reason",
                table: "CrisisAlerts");

            migrationBuilder.AlterColumn<string>(
                name: "Status",
                table: "CrisisAlerts",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "Open",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldDefaultValue: "New");
        }
    }
}
