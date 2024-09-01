using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Iter.Infrastructure.Migrations
{
    public partial class Seed2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "1", "40c09cc1-9733-4e1f-baad-6bbfa445e2d5", "Admin", "Administrator" },
                    { "2", "40c09cc1-9733-4e1f-baad-6bbfa445e2d5", "Coordinator", "Coordinator" },
                    { "3", "40c09cc1-9733-4e1f-baad-6bbfa445e2d5", "TouristGuide", "TouristGuide" },
                    { "4", "40c09cc1-9733-4e1f-baad-6bbfa445e2d5", "Client", "Client" }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "1");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "2");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "3");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "4");
        }
    }
}
