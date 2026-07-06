using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Jalsa.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class ClearStaleEmbeddings : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(
                "UPDATE [AiArtifacts] SET [EmbeddingVector] = NULL WHERE [EmbeddingVector] IS NOT NULL;");

            migrationBuilder.Sql(
                "DELETE FROM [SessionEmbeddings];");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
        }
    }
}
