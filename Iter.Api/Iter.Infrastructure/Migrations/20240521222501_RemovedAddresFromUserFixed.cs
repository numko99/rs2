using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Iter.Infrastructure.Migrations
{
    public partial class RemovedAddresFromUserFixed : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Client_Address_AddressId",
                table: "Client");

            migrationBuilder.DropForeignKey(
                name: "FK_Employee_Address_AddressId",
                table: "Employee");

            migrationBuilder.DropIndex(
                name: "IX_Employee_AddressId",
                table: "Employee");

            migrationBuilder.DropIndex(
                name: "IX_Client_AddressId",
                table: "Client");

            migrationBuilder.DropColumn(
                name: "AddressId",
                table: "Employee");

            migrationBuilder.DropColumn(
                name: "AddressId",
                table: "Client");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "AddressId",
                table: "Employee",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "AddressId",
                table: "Client",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Employee_AddressId",
                table: "Employee",
                column: "AddressId");

            migrationBuilder.CreateIndex(
                name: "IX_Client_AddressId",
                table: "Client",
                column: "AddressId");

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
    }
}
