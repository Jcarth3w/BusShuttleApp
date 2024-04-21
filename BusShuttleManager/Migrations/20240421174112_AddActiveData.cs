using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BusShuttleManager.Migrations
{
    /// <inheritdoc />
    public partial class AddActiveData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "Stop",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "Driver",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "Bus",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "Stop");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "Driver");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "Bus");
        }
    }
}
