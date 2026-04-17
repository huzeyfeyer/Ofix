using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Ofix.Migrations
{
    /// <inheritdoc />
    public partial class Added_CarListing1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_CarListings",
                table: "CarListings");

            migrationBuilder.RenameTable(
                name: "CarListings",
                newName: "AppCarListings");

            migrationBuilder.AlterColumn<string>(
                name: "Title",
                table: "AppCarListings",
                type: "nvarchar(256)",
                maxLength: 256,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "AppCarListings",
                type: "nvarchar(2000)",
                maxLength: 2000,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_AppCarListings",
                table: "AppCarListings",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_AppCarListings_SubModelId",
                table: "AppCarListings",
                column: "SubModelId");

            migrationBuilder.AddForeignKey(
                name: "FK_AppCarListings_AppSubModels_SubModelId",
                table: "AppCarListings",
                column: "SubModelId",
                principalTable: "AppSubModels",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AppCarListings_AppSubModels_SubModelId",
                table: "AppCarListings");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AppCarListings",
                table: "AppCarListings");

            migrationBuilder.DropIndex(
                name: "IX_AppCarListings_SubModelId",
                table: "AppCarListings");

            migrationBuilder.RenameTable(
                name: "AppCarListings",
                newName: "CarListings");

            migrationBuilder.AlterColumn<string>(
                name: "Title",
                table: "CarListings",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(256)",
                oldMaxLength: 256);

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "CarListings",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(2000)",
                oldMaxLength: 2000,
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_CarListings",
                table: "CarListings",
                column: "Id");
        }
    }
}
