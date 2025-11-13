using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Ficticia.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddTypeAndValueToPersonAttributes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Description",
                table: "AttributeTypes");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "AttributeTypes");

            migrationBuilder.AlterColumn<string>(
                name: "Value",
                table: "PersonAttributes",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(200)",
                oldMaxLength: 200);

            migrationBuilder.AddColumn<string>(
                name: "Type",
                table: "AttributeTypes",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Value",
                table: "AttributeTypes",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Type",
                table: "AttributeTypes");

            migrationBuilder.DropColumn(
                name: "Value",
                table: "AttributeTypes");

            migrationBuilder.AlterColumn<string>(
                name: "Value",
                table: "PersonAttributes",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "AttributeTypes",
                type: "nvarchar(250)",
                maxLength: 250,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "AttributeTypes",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");
        }
    }
}
