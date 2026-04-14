using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace samsungweb.Migrations
{
    /// <inheritdoc />
    public partial class AddProductSpecs : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Battery",
                table: "Products",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CameraInfo",
                table: "Products",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Chipset",
                table: "Products",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "OSFeature",
                table: "Products",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ScreenSize",
                table: "Products",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Battery",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "CameraInfo",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "Chipset",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "OSFeature",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "ScreenSize",
                table: "Products");
        }
    }
}
