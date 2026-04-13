using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Ofix.Migrations
{
    /// <inheritdoc />
    public partial class Added_SubModels : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Status",
                table: "AppSubModels");

            migrationBuilder.AlterColumn<string>(
                name: "Slug",
                table: "AppSubModels",
                type: "nvarchar(256)",
                maxLength: 256,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ListingStatus",
                table: "AppSubModels",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ListingStatus",
                table: "AppSubModels");

            migrationBuilder.AlterColumn<string>(
                name: "Slug",
                table: "AppSubModels",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(256)",
                oldMaxLength: 256);

            migrationBuilder.AddColumn<bool>(
                name: "Status",
                table: "AppSubModels",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }
    }
}
