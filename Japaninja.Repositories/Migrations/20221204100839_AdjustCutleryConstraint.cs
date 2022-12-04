using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Japaninja.Repositories.Migrations
{
    public partial class AdjustCutleryConstraint : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Cutlery",
                keyColumn: "Id",
                keyValue: "0");

            migrationBuilder.DeleteData(
                table: "Cutlery",
                keyColumn: "Id",
                keyValue: "1");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Cutlery",
                columns: new[] { "Id", "Name" },
                values: new object[] { "0", "Fork" });

            migrationBuilder.InsertData(
                table: "Cutlery",
                columns: new[] { "Id", "Name" },
                values: new object[] { "1", "Sticks" });
        }
    }
}
