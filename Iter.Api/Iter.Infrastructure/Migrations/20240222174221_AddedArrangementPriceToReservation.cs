using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Iter.Infrastructure.Migrations
{
    public partial class AddedArrangementPriceToReservation : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "ArrangementPriceId",
                table: "Reservation",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_Reservation_ArrangementPriceId",
                table: "Reservation",
                column: "ArrangementPriceId");

            migrationBuilder.AddForeignKey(
                name: "FK_Reservation_ArrangementPrice",
                table: "Reservation",
                column: "ArrangementPriceId",
                principalTable: "ArrangementPrice",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Reservation_ArrangementPrice",
                table: "Reservation");

            migrationBuilder.DropIndex(
                name: "IX_Reservation_ArrangementPriceId",
                table: "Reservation");

            migrationBuilder.DropColumn(
                name: "ArrangementPriceId",
                table: "Reservation");
        }
    }
}
