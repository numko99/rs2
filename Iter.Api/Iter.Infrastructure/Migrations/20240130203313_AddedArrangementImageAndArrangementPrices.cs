using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Iter.Infrastructure.Migrations
{
    public partial class AddedArrangementImageAndArrangementPrices : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ArrangementPrice_AccommodationType_AccommodationTypeId",
                table: "ArrangementPrice");

            migrationBuilder.DropTable(
                name: "AccommodationType");

            migrationBuilder.DropIndex(
                name: "IX_ArrangementPrice_AccommodationTypeId",
                table: "ArrangementPrice");

            migrationBuilder.DropColumn(
                name: "AccommodationTypeId",
                table: "ArrangementPrice");

            migrationBuilder.DropColumn(
                name: "Capacity",
                table: "Arrangement");

            migrationBuilder.DropColumn(
                name: "Price",
                table: "Arrangement");

            migrationBuilder.AddColumn<string>(
                name: "AccommodationType",
                table: "ArrangementPrice",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "Image",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ImageContent = table.Column<byte[]>(type: "varbinary(max)", nullable: true),
                    ImageThumb = table.Column<byte[]>(type: "varbinary(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Image", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ArrangementImage",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ArrangementId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ImageId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    IsMainImage = table.Column<bool>(type: "bit", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DateCreated = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DateModified = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ArrangementImage", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ArrangementImage_Arrangement_ArrangementId",
                        column: x => x.ArrangementId,
                        principalTable: "Arrangement",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ArrangementImage_Image_ImageId",
                        column: x => x.ImageId,
                        principalTable: "Image",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ArrangementImage_ArrangementId",
                table: "ArrangementImage",
                column: "ArrangementId");

            migrationBuilder.CreateIndex(
                name: "IX_ArrangementImage_ImageId",
                table: "ArrangementImage",
                column: "ImageId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ArrangementImage");

            migrationBuilder.DropTable(
                name: "Image");

            migrationBuilder.DropColumn(
                name: "AccommodationType",
                table: "ArrangementPrice");

            migrationBuilder.AddColumn<int>(
                name: "AccommodationTypeId",
                table: "ArrangementPrice",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Capacity",
                table: "Arrangement",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<decimal>(
                name: "Price",
                table: "Arrangement",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

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

            migrationBuilder.CreateIndex(
                name: "IX_ArrangementPrice_AccommodationTypeId",
                table: "ArrangementPrice",
                column: "AccommodationTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_ArrangementPrice_AccommodationType_AccommodationTypeId",
                table: "ArrangementPrice",
                column: "AccommodationTypeId",
                principalTable: "AccommodationType",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
