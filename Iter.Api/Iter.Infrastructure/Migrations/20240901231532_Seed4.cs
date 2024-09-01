using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Iter.Infrastructure.Migrations
{
    public partial class Seed4 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[,]
                {
                    { "3", "0bfe9409-2df3-405a-b34d-530f8e86293b" },
                    { "4", "0dfe16b1-d7cd-4545-be89-a0f3f5945de4" },
                    { "2", "39539a1b-2f21-46f7-87b8-bee4745d50f7" },
                    { "2", "6789925f-fdf3-4c76-8236-0a50a57f1c2c" },
                    { "3", "67be2123-c0ad-4b94-a4c3-cbfe002a7313" },
                    { "4", "67be2123-c0ad-4b94-a4c3-cbfe002a7313" },
                    { "4", "67fc1dec-4ba5-460e-b4a1-eabae216efe9" },
                    { "3", "8a55e790-9562-45aa-a1e9-4c26022582d2" },
                    { "3", "8b778d69-5d48-4690-8d8a-945c7fb250b1" },
                    { "3", "aabf1ca0-abb2-4082-81d9-4275fcdd080b" },
                    { "2", "bb23c4f8-afc5-401f-91d0-e8908648c60e" },
                    { "1", "bf85b04a-2c8d-4e29-a9f3-0b34210b7d63" },
                    { "2", "c6e805ba-56eb-4958-913d-cabd5f5f8495" },
                    { "3", "d0b17a41-d2b6-40cc-9f70-45ec162ec043" },
                    { "4", "d143d790-dd84-483b-819a-5423303c4ceb" }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "3", "0bfe9409-2df3-405a-b34d-530f8e86293b" });

            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "4", "0dfe16b1-d7cd-4545-be89-a0f3f5945de4" });

            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "2", "39539a1b-2f21-46f7-87b8-bee4745d50f7" });

            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "2", "6789925f-fdf3-4c76-8236-0a50a57f1c2c" });

            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "3", "67be2123-c0ad-4b94-a4c3-cbfe002a7313" });

            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "4", "67be2123-c0ad-4b94-a4c3-cbfe002a7313" });

            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "4", "67fc1dec-4ba5-460e-b4a1-eabae216efe9" });

            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "3", "8a55e790-9562-45aa-a1e9-4c26022582d2" });

            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "3", "8b778d69-5d48-4690-8d8a-945c7fb250b1" });

            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "3", "aabf1ca0-abb2-4082-81d9-4275fcdd080b" });

            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "2", "bb23c4f8-afc5-401f-91d0-e8908648c60e" });

            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "1", "bf85b04a-2c8d-4e29-a9f3-0b34210b7d63" });

            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "2", "c6e805ba-56eb-4958-913d-cabd5f5f8495" });

            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "3", "d0b17a41-d2b6-40cc-9f70-45ec162ec043" });

            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "4", "d143d790-dd84-483b-819a-5423303c4ceb" });
        }
    }
}
