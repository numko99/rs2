using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Iter.Infrastructure.Migrations
{
    public partial class AddedVerificationTokenTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "VerificationToken",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Token = table.Column<string>(type: "nvarchar(5)", maxLength: 5, nullable: false),
                    VerificationTokenType = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    ExpiryDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VerificationToken", x => x.Id);
                    table.ForeignKey(
                        name: "FK_VerificationToken_User",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_VerificationToken_UserId",
                table: "VerificationToken",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "VerificationToken");
        }
    }
}
