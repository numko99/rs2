using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Iter.Infrastructure.Migrations
{
    public partial class AddedAccomodationTypeAndArrangementPriceTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AccommodationType",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AccommodationType", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ArrangementPrice",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ArrangementId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AccommodationTypeId = table.Column<int>(type: "int", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DateCreated = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DateModified = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ArrangementPrice", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ArrangementPrice_AccommodationType_AccommodationTypeId",
                        column: x => x.AccommodationTypeId,
                        principalTable: "AccommodationType",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ArrangementPrice_Arrangement_ArrangementId",
                        column: x => x.ArrangementId,
                        principalTable: "Arrangement",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ArrangementPrice_AccommodationTypeId",
                table: "ArrangementPrice",
                column: "AccommodationTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_ArrangementPrice_ArrangementId",
                table: "ArrangementPrice",
                column: "ArrangementId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ArrangementPrice");

            migrationBuilder.DropTable(
                name: "AccommodationType");
        }
    }
}
