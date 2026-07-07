using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Jalsa.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddTherapistApprovalAndUserSoftDelete : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "Users",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "ApprovalStatus",
                table: "Therapists",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: false,
                defaultValue: "Pending");

            migrationBuilder.AddColumn<DateTime>(
                name: "ApprovalStatusUpdatedAt",
                table: "Therapists",
                type: "datetime2",
                nullable: true);

            // Seed the 3 fixed roles only if missing by name — some environments (this
            // dev DB included) already have them manually seeded with organic GUIDs, and
            // the app only ever looks roles up by Name, never by Id, so no fixed Id is needed.
            migrationBuilder.Sql(@"
                IF NOT EXISTS (SELECT 1 FROM [Roles] WHERE [Name] = N'Admin')
                    INSERT INTO [Roles] ([Id], [Name]) VALUES (NEWID(), N'Admin');
                IF NOT EXISTS (SELECT 1 FROM [Roles] WHERE [Name] = N'Therapist')
                    INSERT INTO [Roles] ([Id], [Name]) VALUES (NEWID(), N'Therapist');
                IF NOT EXISTS (SELECT 1 FROM [Roles] WHERE [Name] = N'Patient')
                    INSERT INTO [Roles] ([Id], [Name]) VALUES (NEWID(), N'Patient');
            ");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // No-op: roles may pre-date this migration or be referenced by existing
            // UserRoles rows, so Down() must not delete data it didn't necessarily create.

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "ApprovalStatus",
                table: "Therapists");

            migrationBuilder.DropColumn(
                name: "ApprovalStatusUpdatedAt",
                table: "Therapists");
        }
    }
}
