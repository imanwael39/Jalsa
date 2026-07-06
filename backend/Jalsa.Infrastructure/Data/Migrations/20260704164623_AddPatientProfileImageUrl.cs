using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Jalsa.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddPatientProfileImageUrl : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ProfileImageUrl",
                table: "Patients",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ProfileImageUrl",
                table: "Patients");
        }
    }
}
