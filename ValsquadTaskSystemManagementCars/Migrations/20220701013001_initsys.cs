using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ValsquadTaskSystemManagementCars.Migrations
{
    public partial class initsys : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Cars",
                columns: table => new
                {
                    PlateNumber = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    BrandName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ModelName = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cars", x => x.PlateNumber);
                });

            migrationBuilder.CreateTable(
                name: "Employees",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Age = table.Column<int>(type: "int", nullable: false),
                    Position = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Employees", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "EmployeeCard",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EmployeesId = table.Column<int>(type: "int", nullable: false),
                    CarsPlateNumber = table.Column<string>(type: "nvarchar(10)", nullable: true),
                    CountAccessSameMinute = table.Column<int>(type: "int", nullable: false),
                    CountAccess = table.Column<int>(type: "int", nullable: false),
                    Credit = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Debit = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    DateTimeNow = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmployeeCard", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EmployeeCard_Cars_CarsPlateNumber",
                        column: x => x.CarsPlateNumber,
                        principalTable: "Cars",
                        principalColumn: "PlateNumber");
                    table.ForeignKey(
                        name: "FK_EmployeeCard_Employees_EmployeesId",
                        column: x => x.EmployeesId,
                        principalTable: "Employees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeCard_CarsPlateNumber",
                table: "EmployeeCard",
                column: "CarsPlateNumber");

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeCard_EmployeesId",
                table: "EmployeeCard",
                column: "EmployeesId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EmployeeCard");

            migrationBuilder.DropTable(
                name: "Cars");

            migrationBuilder.DropTable(
                name: "Employees");
        }
    }
}
