using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Iter.Infrastructure.Migrations
{
    public partial class ByteArrayTypeForLogo : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LogoUrl",
                table: "Agency");

            migrationBuilder.AddColumn<byte[]>(
                name: "Logo",
                table: "Agency",
                type: "varbinary(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Logo",
                table: "Agency");

            migrationBuilder.AddColumn<string>(
                name: "LogoUrl",
                table: "Agency",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: false,
                defaultValue: "");
        }
    }
}
