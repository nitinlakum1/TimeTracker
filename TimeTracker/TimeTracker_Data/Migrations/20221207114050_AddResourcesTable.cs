using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TimeTracker_Data.Migrations
{
    public partial class AddResourcesTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Resources",
                columns: table => new
                {
                    id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    preferenceId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    gender = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    mobile = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    email = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    workYears = table.Column<int>(type: "int", nullable: true),
                    designation = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    degree = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    birthDate = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    workStartDate = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    companyExperiences = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    city = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Resources", x => x.id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Resources");
        }
    }
}
