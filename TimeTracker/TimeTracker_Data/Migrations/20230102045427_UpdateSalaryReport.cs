using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TimeTracker_Data.Migrations
{
    public partial class UpdateSalaryReport : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Amount",
                table: "SalaryReports",
                newName: "PayableAmount");

            migrationBuilder.AddColumn<decimal>(
                name: "BasicSalary",
                table: "SalaryReports",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<int>(
                name: "WorkingDays",
                table: "SalaryReports",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BasicSalary",
                table: "SalaryReports");

            migrationBuilder.DropColumn(
                name: "WorkingDays",
                table: "SalaryReports");

            migrationBuilder.RenameColumn(
                name: "PayableAmount",
                table: "SalaryReports",
                newName: "Amount");
        }
    }
}
