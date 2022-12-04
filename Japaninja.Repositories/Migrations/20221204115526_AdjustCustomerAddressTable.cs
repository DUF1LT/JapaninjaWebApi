using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Japaninja.Repositories.Migrations
{
    public partial class AdjustCustomerAddressTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Entrance",
                table: "CustomerAddresses");

            migrationBuilder.DropColumn(
                name: "Flat",
                table: "CustomerAddresses");

            migrationBuilder.DropColumn(
                name: "Floor",
                table: "CustomerAddresses");

            migrationBuilder.DropColumn(
                name: "House",
                table: "CustomerAddresses");

            migrationBuilder.DropColumn(
                name: "Housing",
                table: "CustomerAddresses");

            migrationBuilder.RenameColumn(
                name: "Street",
                table: "CustomerAddresses",
                newName: "Address");

            migrationBuilder.AddColumn<bool>(
                name: "IsAnonymous",
                table: "Users",
                type: "bit",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Cutlery",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsAnonymous",
                table: "Users");

            migrationBuilder.RenameColumn(
                name: "Address",
                table: "CustomerAddresses",
                newName: "Street");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Cutlery",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Entrance",
                table: "CustomerAddresses",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Flat",
                table: "CustomerAddresses",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Floor",
                table: "CustomerAddresses",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "House",
                table: "CustomerAddresses",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Housing",
                table: "CustomerAddresses",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
