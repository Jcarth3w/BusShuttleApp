using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BusShuttleManager.Migrations
{
    /// <inheritdoc />
    public partial class AddActiveLoops : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "Loop",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "Loop");
        }
    }
}
