using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Japaninja.Repositories.Migrations
{
    public partial class AddOrderNumberId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "NumberId",
                table: "Orders",
                type: "int",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "NumberId",
                table: "Orders");
        }
    }
}
