using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Ofix.Migrations
{
    /// <inheritdoc />
    public partial class CarListingsAddBrandAndModelNullable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Title",
                table: "AppCarListings",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(256)",
                oldMaxLength: 256);

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "AppCarListings",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(2000)",
                oldMaxLength: 2000,
                oldNullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "BrandId",
                table: "AppCarListings",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "ModelId",
                table: "AppCarListings",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_AppCarListings_BrandId",
                table: "AppCarListings",
                column: "BrandId");

            migrationBuilder.CreateIndex(
                name: "IX_AppCarListings_ModelId",
                table: "AppCarListings",
                column: "ModelId");

            migrationBuilder.AddForeignKey(
                name: "FK_AppCarListings_AppBrands_BrandId",
                table: "AppCarListings",
                column: "BrandId",
                principalTable: "AppBrands",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_AppCarListings_AppModels_ModelId",
                table: "AppCarListings",
                column: "ModelId",
                principalTable: "AppModels",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AppCarListings_AppBrands_BrandId",
                table: "AppCarListings");

            migrationBuilder.DropForeignKey(
                name: "FK_AppCarListings_AppModels_ModelId",
                table: "AppCarListings");

            migrationBuilder.DropIndex(
                name: "IX_AppCarListings_BrandId",
                table: "AppCarListings");

            migrationBuilder.DropIndex(
                name: "IX_AppCarListings_ModelId",
                table: "AppCarListings");

            migrationBuilder.DropColumn(
                name: "BrandId",
                table: "AppCarListings");

            migrationBuilder.DropColumn(
                name: "ModelId",
                table: "AppCarListings");

            migrationBuilder.AlterColumn<string>(
                name: "Title",
                table: "AppCarListings",
                type: "nvarchar(256)",
                maxLength: 256,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(200)",
                oldMaxLength: 200);

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "AppCarListings",
                type: "nvarchar(2000)",
                maxLength: 2000,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);
        }
    }
}
