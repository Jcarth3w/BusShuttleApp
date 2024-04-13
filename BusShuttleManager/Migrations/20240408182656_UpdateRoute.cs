using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BusShuttleManager.Migrations
{
    /// <inheritdoc />
    public partial class UpdateRoute : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "LoopId",
                table: "Routes",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "StopId",
                table: "Routes",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Routes_LoopId",
                table: "Routes",
                column: "LoopId");

            migrationBuilder.CreateIndex(
                name: "IX_Routes_StopId",
                table: "Routes",
                column: "StopId");

            migrationBuilder.AddForeignKey(
                name: "FK_Routes_Loop_LoopId",
                table: "Routes",
                column: "LoopId",
                principalTable: "Loop",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Routes_Stop_StopId",
                table: "Routes",
                column: "StopId",
                principalTable: "Stop",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Routes_Loop_LoopId",
                table: "Routes");

            migrationBuilder.DropForeignKey(
                name: "FK_Routes_Stop_StopId",
                table: "Routes");

            migrationBuilder.DropIndex(
                name: "IX_Routes_LoopId",
                table: "Routes");

            migrationBuilder.DropIndex(
                name: "IX_Routes_StopId",
                table: "Routes");

            migrationBuilder.DropColumn(
                name: "LoopId",
                table: "Routes");

            migrationBuilder.DropColumn(
                name: "StopId",
                table: "Routes");
        }
    }
}
