using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Iter.Infrastructure.Migrations
{
    public partial class RenamedCreatedAtModifiedAt : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DateCreated",
                table: "Accommodation");

            migrationBuilder.RenameColumn(
                name: "DateModified",
                table: "Reservation",
                newName: "ModifiedAt");

            migrationBuilder.RenameColumn(
                name: "DateCreated",
                table: "Reservation",
                newName: "CreatedAt");

            migrationBuilder.RenameColumn(
                name: "DateModified",
                table: "EmployeeArrangment",
                newName: "ModifiedAt");

            migrationBuilder.RenameColumn(
                name: "DateCreated",
                table: "EmployeeArrangment",
                newName: "CreatedAt");

            migrationBuilder.RenameColumn(
                name: "DateModified",
                table: "Destination",
                newName: "ModifiedAt");

            migrationBuilder.RenameColumn(
                name: "DateCreated",
                table: "Destination",
                newName: "CreatedAt");

            migrationBuilder.RenameColumn(
                name: "DateModified",
                table: "Client",
                newName: "ModifiedAt");

            migrationBuilder.RenameColumn(
                name: "DateCreated",
                table: "Client",
                newName: "CreatedAt");

            migrationBuilder.RenameColumn(
                name: "DateModified",
                table: "ArrangementPrice",
                newName: "ModifiedAt");

            migrationBuilder.RenameColumn(
                name: "DateCreated",
                table: "ArrangementPrice",
                newName: "CreatedAt");

            migrationBuilder.RenameColumn(
                name: "DateModified",
                table: "ArrangementImage",
                newName: "ModifiedAt");

            migrationBuilder.RenameColumn(
                name: "DateCreated",
                table: "ArrangementImage",
                newName: "CreatedAt");

            migrationBuilder.RenameColumn(
                name: "DateModified",
                table: "Arrangement",
                newName: "ModifiedAt");

            migrationBuilder.RenameColumn(
                name: "DateCreated",
                table: "Arrangement",
                newName: "CreatedAt");

            migrationBuilder.RenameColumn(
                name: "DateModified",
                table: "Agency",
                newName: "ModifiedAt");

            migrationBuilder.RenameColumn(
                name: "DateCreated",
                table: "Agency",
                newName: "CreatedAt");

            migrationBuilder.RenameColumn(
                name: "DateModified",
                table: "Address",
                newName: "ModifiedAt");

            migrationBuilder.RenameColumn(
                name: "DateCreated",
                table: "Address",
                newName: "CreatedAt");

            migrationBuilder.RenameColumn(
                name: "DateModified",
                table: "Accommodation",
                newName: "CreatedAt");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "Employee",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "ModifiedAt",
                table: "Employee",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "ModifiedAt",
                table: "Accommodation",
                type: "datetime2",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "Employee");

            migrationBuilder.DropColumn(
                name: "ModifiedAt",
                table: "Employee");

            migrationBuilder.DropColumn(
                name: "ModifiedAt",
                table: "Accommodation");

            migrationBuilder.RenameColumn(
                name: "ModifiedAt",
                table: "Reservation",
                newName: "DateModified");

            migrationBuilder.RenameColumn(
                name: "CreatedAt",
                table: "Reservation",
                newName: "DateCreated");

            migrationBuilder.RenameColumn(
                name: "ModifiedAt",
                table: "EmployeeArrangment",
                newName: "DateModified");

            migrationBuilder.RenameColumn(
                name: "CreatedAt",
                table: "EmployeeArrangment",
                newName: "DateCreated");

            migrationBuilder.RenameColumn(
                name: "ModifiedAt",
                table: "Destination",
                newName: "DateModified");

            migrationBuilder.RenameColumn(
                name: "CreatedAt",
                table: "Destination",
                newName: "DateCreated");

            migrationBuilder.RenameColumn(
                name: "ModifiedAt",
                table: "Client",
                newName: "DateModified");

            migrationBuilder.RenameColumn(
                name: "CreatedAt",
                table: "Client",
                newName: "DateCreated");

            migrationBuilder.RenameColumn(
                name: "ModifiedAt",
                table: "ArrangementPrice",
                newName: "DateModified");

            migrationBuilder.RenameColumn(
                name: "CreatedAt",
                table: "ArrangementPrice",
                newName: "DateCreated");

            migrationBuilder.RenameColumn(
                name: "ModifiedAt",
                table: "ArrangementImage",
                newName: "DateModified");

            migrationBuilder.RenameColumn(
                name: "CreatedAt",
                table: "ArrangementImage",
                newName: "DateCreated");

            migrationBuilder.RenameColumn(
                name: "ModifiedAt",
                table: "Arrangement",
                newName: "DateModified");

            migrationBuilder.RenameColumn(
                name: "CreatedAt",
                table: "Arrangement",
                newName: "DateCreated");

            migrationBuilder.RenameColumn(
                name: "ModifiedAt",
                table: "Agency",
                newName: "DateModified");

            migrationBuilder.RenameColumn(
                name: "CreatedAt",
                table: "Agency",
                newName: "DateCreated");

            migrationBuilder.RenameColumn(
                name: "ModifiedAt",
                table: "Address",
                newName: "DateModified");

            migrationBuilder.RenameColumn(
                name: "CreatedAt",
                table: "Address",
                newName: "DateCreated");

            migrationBuilder.RenameColumn(
                name: "CreatedAt",
                table: "Accommodation",
                newName: "DateModified");

            migrationBuilder.AddColumn<DateTime>(
                name: "DateCreated",
                table: "Accommodation",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }
    }
}
