using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SkogsInsikt.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddForestAreaOwnership : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "ForestAreas",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.Sql("""
                UPDATE ForestAreas
                SET UserId = (
                    SELECT TOP 1 Id
                    FROM AspNetUsers
                    WHERE Email = 'test@skogsinsikt.se'
                )
                WHERE UserId IS NULL;
                """);

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "ForestAreas",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ForestAreas_UserId",
                table: "ForestAreas",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_ForestAreas_AspNetUsers_UserId",
                table: "ForestAreas",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ForestAreas_AspNetUsers_UserId",
                table: "ForestAreas");

            migrationBuilder.DropIndex(
                name: "IX_ForestAreas_UserId",
                table: "ForestAreas");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "ForestAreas");
        }
    }
}
