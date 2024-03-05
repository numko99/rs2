using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Iter.Infrastructure.Migrations
{
    public partial class AddedArrangementStatusToArrangement : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "Arrangement");

            migrationBuilder.AddColumn<int>(
                name: "ArrangementStatusId",
                table: "Arrangement",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "ArrangementStatus",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ArrangementStatus", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Arrangement_ArrangementStatusId",
                table: "Arrangement",
                column: "ArrangementStatusId");

            migrationBuilder.AddForeignKey(
                name: "FK_Arrangement_ArrangementStatus",
                table: "Arrangement",
                column: "ArrangementStatusId",
                principalTable: "ArrangementStatus",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Arrangement_ArrangementStatus",
                table: "Arrangement");

            migrationBuilder.DropTable(
                name: "ArrangementStatus");

            migrationBuilder.DropIndex(
                name: "IX_Arrangement_ArrangementStatusId",
                table: "Arrangement");

            migrationBuilder.DropColumn(
                name: "ArrangementStatusId",
                table: "Arrangement");

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "Arrangement",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }
    }
}
