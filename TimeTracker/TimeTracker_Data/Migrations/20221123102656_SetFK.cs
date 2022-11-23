using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TimeTracker_Data.Migrations
{
    public partial class SetFK : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_SystemLogs_UserId",
                table: "SystemLogs",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_SystemLogs_Users_UserId",
                table: "SystemLogs",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SystemLogs_Users_UserId",
                table: "SystemLogs");

            migrationBuilder.DropIndex(
                name: "IX_SystemLogs_UserId",
                table: "SystemLogs");
        }
    }
}
