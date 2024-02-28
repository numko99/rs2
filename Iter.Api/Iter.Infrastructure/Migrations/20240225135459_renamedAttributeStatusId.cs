using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Iter.Infrastructure.Migrations
{
    public partial class renamedAttributeStatusId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "StatusId",
                table: "Reservation",
                newName: "ReservationStatusId");

            migrationBuilder.RenameIndex(
                name: "IX_Reservation_StatusId",
                table: "Reservation",
                newName: "IX_Reservation_ReservationStatusId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ReservationStatusId",
                table: "Reservation",
                newName: "StatusId");

            migrationBuilder.RenameIndex(
                name: "IX_Reservation_ReservationStatusId",
                table: "Reservation",
                newName: "IX_Reservation_StatusId");
        }
    }
}
