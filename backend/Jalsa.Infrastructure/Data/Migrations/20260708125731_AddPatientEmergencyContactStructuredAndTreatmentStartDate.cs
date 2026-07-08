using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Jalsa.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddPatientEmergencyContactStructuredAndTreatmentStartDate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "EmergencyContact",
                table: "Patients",
                newName: "EmergencyContactRelationship");

            migrationBuilder.AddColumn<string>(
                name: "EmergencyContactName",
                table: "Patients",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "EmergencyContactPhone",
                table: "Patients",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateOnly>(
                name: "TreatmentStartDate",
                table: "Patients",
                type: "date",
                nullable: true);

            // Existing free-text values follow the seed data's "Relationship - Name - Phone"
            // convention (e.g. "الأخت - سارة عبدالله - +201190980339"). Best-effort split rows
            // matching that shape into the three new columns; all three SET expressions below
            // read the pre-update value of EmergencyContactRelationship, so this is safe as a
            // single statement despite reassigning that same column.
            migrationBuilder.Sql(@"
                UPDATE Patients
                SET
                    EmergencyContactName = LTRIM(RTRIM(SUBSTRING(
                        EmergencyContactRelationship,
                        CHARINDEX(' - ', EmergencyContactRelationship) + 3,
                        CHARINDEX(' - ', EmergencyContactRelationship, CHARINDEX(' - ', EmergencyContactRelationship) + 3) - CHARINDEX(' - ', EmergencyContactRelationship) - 3
                    ))),
                    EmergencyContactPhone = LTRIM(RTRIM(SUBSTRING(
                        EmergencyContactRelationship,
                        CHARINDEX(' - ', EmergencyContactRelationship, CHARINDEX(' - ', EmergencyContactRelationship) + 3) + 3,
                        4000
                    ))),
                    EmergencyContactRelationship = LTRIM(RTRIM(SUBSTRING(EmergencyContactRelationship, 1, CHARINDEX(' - ', EmergencyContactRelationship) - 1)))
                WHERE EmergencyContactRelationship IS NOT NULL
                    AND (LEN(EmergencyContactRelationship) - LEN(REPLACE(EmergencyContactRelationship, ' - ', ''))) >= 6;
            ");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                UPDATE Patients
                SET EmergencyContactRelationship =
                    COALESCE(EmergencyContactRelationship, '') +
                    CASE WHEN EmergencyContactName IS NOT NULL THEN ' - ' + EmergencyContactName ELSE '' END +
                    CASE WHEN EmergencyContactPhone IS NOT NULL THEN ' - ' + EmergencyContactPhone ELSE '' END
                WHERE EmergencyContactName IS NOT NULL OR EmergencyContactPhone IS NOT NULL;
            ");

            migrationBuilder.DropColumn(
                name: "EmergencyContactName",
                table: "Patients");

            migrationBuilder.DropColumn(
                name: "EmergencyContactPhone",
                table: "Patients");

            migrationBuilder.DropColumn(
                name: "TreatmentStartDate",
                table: "Patients");

            migrationBuilder.RenameColumn(
                name: "EmergencyContactRelationship",
                table: "Patients",
                newName: "EmergencyContact");
        }
    }
}
