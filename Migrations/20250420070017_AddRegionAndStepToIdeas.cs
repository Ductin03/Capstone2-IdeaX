using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace IdeaX.Migrations
{
    /// <inheritdoc />
    public partial class AddRegionAndStepToIdeas : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Region",
                table: "Ideas",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Stage",
                table: "Ideas",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Region",
                table: "Ideas");

            migrationBuilder.DropColumn(
                name: "Stage",
                table: "Ideas");
        }
    }
}
