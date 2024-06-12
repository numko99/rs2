using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Iter.Infrastructure.Migrations
{
    public partial class RemovedAddresFromUser : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Client_Address",
                table: "Client");

            migrationBuilder.DropForeignKey(
                name: "FK_Employee_Address",
                table: "Employee");

            migrationBuilder.RenameColumn(
                name: "BirthPlace",
                table: "Employee",
                newName: "ResidencePlace");

            migrationBuilder.RenameColumn(
                name: "BirthPlace",
                table: "Client",
                newName: "ResidencePlace");

            migrationBuilder.AlterColumn<Guid>(
                name: "AddressId",
                table: "Employee",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AlterColumn<Guid>(
                name: "AddressId",
                table: "Client",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AddForeignKey(
                name: "FK_Client_Address_AddressId",
                table: "Client",
                column: "AddressId",
                principalTable: "Address",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Employee_Address_AddressId",
                table: "Employee",
                column: "AddressId",
                principalTable: "Address",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Client_Address_AddressId",
                table: "Client");

            migrationBuilder.DropForeignKey(
                name: "FK_Employee_Address_AddressId",
                table: "Employee");

            migrationBuilder.RenameColumn(
                name: "ResidencePlace",
                table: "Employee",
                newName: "BirthPlace");

            migrationBuilder.RenameColumn(
                name: "ResidencePlace",
                table: "Client",
                newName: "BirthPlace");

            migrationBuilder.AlterColumn<Guid>(
                name: "AddressId",
                table: "Employee",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "AddressId",
                table: "Client",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Client_Address",
                table: "Client",
                column: "AddressId",
                principalTable: "Address",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Employee_Address",
                table: "Employee",
                column: "AddressId",
                principalTable: "Address",
                principalColumn: "Id");
        }
    }
}
