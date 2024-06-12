using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Iter.Infrastructure.Migrations
{
    public partial class AddedRatingToArrangmenetTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "Rating",
                table: "Arrangement",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Rating",
                table: "Arrangement");
        }
    }
}
