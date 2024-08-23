using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Iter.Infrastructure.Migrations
{
    public partial class AddedRelationshipReservationCity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "DepartureCityId",
                table: "Reservation",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Reservation_DepartureCityId",
                table: "Reservation",
                column: "DepartureCityId");

            migrationBuilder.AddForeignKey(
                name: "FK_Reservation_DepartureCity",
                table: "Reservation",
                column: "DepartureCityId",
                principalTable: "City",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Reservation_DepartureCity",
                table: "Reservation");

            migrationBuilder.DropIndex(
                name: "IX_Reservation_DepartureCityId",
                table: "Reservation");

            migrationBuilder.DropColumn(
                name: "DepartureCityId",
                table: "Reservation");
        }
    }
}
