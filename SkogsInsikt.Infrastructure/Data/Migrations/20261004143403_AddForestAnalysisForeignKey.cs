using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SkogsInsikt.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddForestAnalysisForeignKey : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "TreeSpecies",
                table: "ForestAreas",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "ForestAreas",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.CreateIndex(
                name: "IX_ForestAnalyses_ForestAreaId",
                table: "ForestAnalyses",
                column: "ForestAreaId");

            migrationBuilder.AddForeignKey(
                name: "FK_ForestAnalyses_ForestAreas_ForestAreaId",
                table: "ForestAnalyses",
                column: "ForestAreaId",
                principalTable: "ForestAreas",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ForestAnalyses_ForestAreas_ForestAreaId",
                table: "ForestAnalyses");

            migrationBuilder.DropIndex(
                name: "IX_ForestAnalyses_ForestAreaId",
                table: "ForestAnalyses");

            migrationBuilder.AlterColumn<string>(
                name: "TreeSpecies",
                table: "ForestAreas",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "ForestAreas",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100);
        }
    }
}
