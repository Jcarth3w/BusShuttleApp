using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BusShuttleManager.Migrations
{
    /// <inheritdoc />
    public partial class UpdateEntry : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "BusId",
                table: "Entry",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "DriverId",
                table: "Entry",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "LoopId",
                table: "Entry",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "StopId",
                table: "Entry",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BusId",
                table: "Entry");

            migrationBuilder.DropColumn(
                name: "DriverId",
                table: "Entry");

            migrationBuilder.DropColumn(
                name: "LoopId",
                table: "Entry");

            migrationBuilder.DropColumn(
                name: "StopId",
                table: "Entry");
        }
    }
}
