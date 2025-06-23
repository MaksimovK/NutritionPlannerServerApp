using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NutritionPlanner.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class AddBlockReasonUsers : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "BlockReason",
                table: "Users",
                type: "text",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BlockReason",
                table: "Users");
        }
    }
}
