using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Japaninja.Repositories.Migrations
{
    public partial class SplitCustomerAddress : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Address",
                table: "CustomerAddresses");

            migrationBuilder.AddColumn<string>(
                name: "Entrance",
                table: "CustomerAddresses",
                type: "nvarchar(300)",
                maxLength: 300,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FlatNumber",
                table: "CustomerAddresses",
                type: "nvarchar(300)",
                maxLength: 300,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Floor",
                table: "CustomerAddresses",
                type: "nvarchar(300)",
                maxLength: 300,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "HouseNumber",
                table: "CustomerAddresses",
                type: "nvarchar(300)",
                maxLength: 300,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Street",
                table: "CustomerAddresses",
                type: "nvarchar(300)",
                maxLength: 300,
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Entrance",
                table: "CustomerAddresses");

            migrationBuilder.DropColumn(
                name: "FlatNumber",
                table: "CustomerAddresses");

            migrationBuilder.DropColumn(
                name: "Floor",
                table: "CustomerAddresses");

            migrationBuilder.DropColumn(
                name: "HouseNumber",
                table: "CustomerAddresses");

            migrationBuilder.DropColumn(
                name: "Street",
                table: "CustomerAddresses");

            migrationBuilder.AddColumn<string>(
                name: "Address",
                table: "CustomerAddresses",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
