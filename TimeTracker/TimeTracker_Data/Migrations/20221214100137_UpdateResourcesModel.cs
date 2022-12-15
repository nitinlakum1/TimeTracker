using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TimeTracker_Data.Migrations
{
    public partial class UpdateResourcesModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Remarks_Resources_preferenceId",
                table: "Remarks");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Remarks",
                table: "Remarks");

            migrationBuilder.DropIndex(
                name: "IX_Remarks_preferenceId",
                table: "Remarks");

            migrationBuilder.RenameTable(
                name: "Remarks",
                newName: "ResourcesRemarks");

            migrationBuilder.RenameColumn(
                name: "preferenceId",
                table: "ResourcesRemarks",
                newName: "PreferenceId");

            migrationBuilder.AddColumn<int>(
                name: "ResourceStatus",
                table: "Resources",
                type: "int",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "PreferenceId",
                table: "ResourcesRemarks",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ResourcesRemarks",
                table: "ResourcesRemarks",
                column: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_ResourcesRemarks",
                table: "ResourcesRemarks");

            migrationBuilder.DropColumn(
                name: "ResourceStatus",
                table: "Resources");

            migrationBuilder.RenameTable(
                name: "ResourcesRemarks",
                newName: "Remarks");

            migrationBuilder.RenameColumn(
                name: "PreferenceId",
                table: "Remarks",
                newName: "preferenceId");

            migrationBuilder.AlterColumn<string>(
                name: "preferenceId",
                table: "Remarks",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Remarks",
                table: "Remarks",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_Remarks_preferenceId",
                table: "Remarks",
                column: "preferenceId");

            migrationBuilder.AddForeignKey(
                name: "FK_Remarks_Resources_preferenceId",
                table: "Remarks",
                column: "preferenceId",
                principalTable: "Resources",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
