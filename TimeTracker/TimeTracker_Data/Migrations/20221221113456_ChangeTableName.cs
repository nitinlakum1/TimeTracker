using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TimeTracker_Data.Migrations
{
    public partial class ChangeTableName : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_ResourcesRemarks",
                table: "ResourcesRemarks");

            migrationBuilder.RenameTable(
                name: "ResourcesRemarks",
                newName: "ResourcesFollowup");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ResourcesFollowup",
                table: "ResourcesFollowup",
                column: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_ResourcesFollowup",
                table: "ResourcesFollowup");

            migrationBuilder.RenameTable(
                name: "ResourcesFollowup",
                newName: "ResourcesRemarks");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ResourcesRemarks",
                table: "ResourcesRemarks",
                column: "Id");
        }
    }
}
