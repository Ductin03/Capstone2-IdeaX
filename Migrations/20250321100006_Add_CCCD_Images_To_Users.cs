using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace IdeaX.Migrations
{
    /// <inheritdoc />
    public partial class Add_CCCD_Images_To_Users : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CCCD",
                table: "Users",
                type: "character varying(250)",
                maxLength: 250,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "CCCDBack",
                table: "Users",
                type: "character varying(250)",
                maxLength: 250,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "CCCDFront",
                table: "Users",
                type: "character varying(250)",
                maxLength: 250,
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CCCD",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "CCCDBack",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "CCCDFront",
                table: "Users");
        }
    }
}
