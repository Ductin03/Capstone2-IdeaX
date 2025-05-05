using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace IdeaX.Migrations
{
    /// <inheritdoc />
    public partial class AddInvestorPreferences : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "FundingRangeMax",
                table: "Users",
                type: "numeric",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "FundingRangeMin",
                table: "Users",
                type: "numeric",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PreferredIndustries",
                table: "Users",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PreferredRegions",
                table: "Users",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PreferredStages",
                table: "Users",
                type: "text",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FundingRangeMax",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "FundingRangeMin",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "PreferredIndustries",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "PreferredRegions",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "PreferredStages",
                table: "Users");
        }
    }
}
