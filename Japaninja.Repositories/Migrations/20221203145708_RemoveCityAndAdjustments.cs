using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Japaninja.Repositories.Migrations
{
    public partial class RemoveCityAndAdjustments : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Restaurants_Cities_CityId",
                table: "Restaurants");

            migrationBuilder.DropTable(
                name: "Cities");

            migrationBuilder.DropIndex(
                name: "IX_Restaurants_CityId",
                table: "Restaurants");

            migrationBuilder.DropColumn(
                name: "CityId",
                table: "Restaurants");

            migrationBuilder.InsertData(
                table: "Cutlery",
                columns: new[] { "Id", "Name" },
                values: new object[] { "0", "Fork" });

            migrationBuilder.InsertData(
                table: "Cutlery",
                columns: new[] { "Id", "Name" },
                values: new object[] { "1", "Sticks" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Cutlery",
                keyColumn: "Id",
                keyValue: "0");

            migrationBuilder.DeleteData(
                table: "Cutlery",
                keyColumn: "Id",
                keyValue: "1");

            migrationBuilder.AddColumn<string>(
                name: "CityId",
                table: "Restaurants",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Cities",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cities", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Restaurants_CityId",
                table: "Restaurants",
                column: "CityId");

            migrationBuilder.AddForeignKey(
                name: "FK_Restaurants_Cities_CityId",
                table: "Restaurants",
                column: "CityId",
                principalTable: "Cities",
                principalColumn: "Id");
        }
    }
}
