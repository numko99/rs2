using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Iter.Infrastructure.Migrations
{
    public partial class CreatedRelationshipBetweenAgencyAndImage : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Logo",
                table: "Agency");

            migrationBuilder.AddColumn<Guid>(
                name: "ImageId",
                table: "Agency",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Agency_ImageId",
                table: "Agency",
                column: "ImageId");

            migrationBuilder.AddForeignKey(
                name: "FK_Agency_Image",
                table: "Agency",
                column: "ImageId",
                principalTable: "Image",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Agency_Image",
                table: "Agency");

            migrationBuilder.DropIndex(
                name: "IX_Agency_ImageId",
                table: "Agency");

            migrationBuilder.DropColumn(
                name: "ImageId",
                table: "Agency");

            migrationBuilder.AddColumn<byte[]>(
                name: "Logo",
                table: "Agency",
                type: "varbinary(max)",
                nullable: true);
        }
    }
}
