using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Ofix.Migrations
{
    /// <inheritdoc />
    public partial class Changed_ListingStatus_To_IsActive_For_Brand_Model_SubModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ListingStatus",
                table: "AppSubModels");

            migrationBuilder.DropColumn(
                name: "ListingStatus",
                table: "AppModels");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "AppBrands");

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "AppSubModels",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "AppModels",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "AppBrands",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "AppSubModels");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "AppModels");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "AppBrands");

            migrationBuilder.AddColumn<int>(
                name: "ListingStatus",
                table: "AppSubModels",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ListingStatus",
                table: "AppModels",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "AppBrands",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
