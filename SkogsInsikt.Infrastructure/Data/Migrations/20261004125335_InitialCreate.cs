using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SkogsInsikt.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ForestAnalyses",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ForestAreaId = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    RiskLevel = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Recommendation = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Temperature = table.Column<double>(type: "float", nullable: false),
                    Precipitation = table.Column<double>(type: "float", nullable: false),
                    WindSpeed = table.Column<double>(type: "float", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ForestAnalyses", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ForestAreas",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AreaHectares = table.Column<double>(type: "float", nullable: false),
                    TreeSpecies = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PlantingYear = table.Column<int>(type: "int", nullable: false),
                    Latitude = table.Column<double>(type: "float", nullable: false),
                    Longitude = table.Column<double>(type: "float", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ForestAreas", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ForestAnalyses");

            migrationBuilder.DropTable(
                name: "ForestAreas");
        }
    }
}
