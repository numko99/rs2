using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Iter.Infrastructure.Migrations
{
    public partial class Seed3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ClientId", "ConcurrencyStamp", "CreatedAt", "Email", "EmailConfirmed", "EmployeeId", "IsActive", "LockoutEnabled", "LockoutEnd", "ModifiedAt", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "Role", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[,]
                {
                    { "0bfe9409-2df3-405a-b34d-530f8e86293b", 0, null, "cb95abf4-74b5-48bb-9ab1-8f3464e937eb", new DateTime(2024, 8, 25, 17, 32, 57, 584, DateTimeKind.Unspecified).AddTicks(3177), "elvir@gmail.com", true, new Guid("f6fe658e-37f8-4a52-a314-08dcc51a8157"), true, true, null, new DateTime(2024, 8, 25, 17, 32, 57, 584, DateTimeKind.Unspecified).AddTicks(3186), "ELVIR@GMAIL.COM", "ELVIR@GMAIL.COM", "AQAAAAEAACcQAAAAENwxCISpQ2gSz9zdlvkKHz90aSoozrdC3mGgVXv4iHPFWP+S2OB8A5SBsctbXeeEvA==", "061-123-123", false, 3, "64OWPHMODTWBFBIJR57VHMI74JYAX5M7", false, "elvir@gmail.com" },
                    { "0dfe16b1-d7cd-4545-be89-a0f3f5945de4", 0, new Guid("6240d8f1-b80d-49e3-77d0-08dc8003d380"), "8704efa2-5dca-43d5-aa68-87c46d6c4916", new DateTime(2024, 7, 6, 16, 1, 33, 463, DateTimeKind.Unspecified).AddTicks(3333), "admir.numanovic@breakpoint.software", true, null, true, true, null, new DateTime(2024, 7, 6, 16, 1, 40, 670, DateTimeKind.Unspecified), "ADMIR.NUMANOVIC@BREAKPOINT.SOFTWARE", "ADMIR.NUMANOVIC@BREAKPOINT.SOFTWARE", "AQAAAAEAACcQAAAAEDlqTJ4s5iUbbPEVguXiHgcFe8rNCW422Acc+6olVegqIeeBvQWzGfD7j1sFQRZxMQ==", "062-867-340", false, 4, "VGLOKC7GB3MX3JDQSIUMUWK3NEQ3GDV3", false, "admir.numanovic@breakpoint.software" },
                    { "0EA58B09-0CC1-4DDD-8D89-35F820CC740B", 0, new Guid("1eb9eee6-d946-4111-a2e6-a3a67445ad97"), "stamp10", new DateTime(2024, 7, 6, 16, 1, 33, 463, DateTimeKind.Unspecified).AddTicks(3333), "mike.davis@example.com", true, null, true, false, null, new DateTime(2024, 7, 6, 16, 1, 40, 670, DateTimeKind.Unspecified), "MIKE.DAVIS@EXAMPLE.COM", "MIKE.DAVIS", "hashedpassword", "062-003-546", true, 4, "stamp9", false, "mike.davis" },
                    { "13EC6CFE-DE83-400E-A80B-C38E39FDBE56", 0, new Guid("8fd6aa7e-2afa-402b-a05c-541ab2ddbc5a"), "stamp6", new DateTime(2024, 7, 6, 16, 1, 33, 463, DateTimeKind.Unspecified).AddTicks(3333), "alice.johnson@example.com", true, null, true, false, null, new DateTime(2024, 7, 6, 16, 1, 40, 670, DateTimeKind.Unspecified), "ALICE.JOHNSON@EXAMPLE.COM", "ALICE.JOHNSON", "hashedpassword", "062-212-347", true, 4, "stamp5", false, "alice.johnson" },
                    { "39539a1b-2f21-46f7-87b8-bee4745d50f7", 0, null, "c589953d-b524-4b19-b237-9dc026453307", new DateTime(2024, 8, 25, 17, 34, 7, 158, DateTimeKind.Unspecified).AddTicks(9156), "mustafa@gmail.com", true, new Guid("027d20a4-dcc3-4aee-a315-08dcc51a8157"), true, true, null, new DateTime(2024, 8, 25, 17, 34, 7, 158, DateTimeKind.Unspecified).AddTicks(9162), "MUSTAFA@GMAIL.COM", "MUSTAFA@GMAIL.COM", "AQAAAAEAACcQAAAAEKdPkL2s7L+d+B0q2kQshFNffIyQLXiwznQaObneNnmSK7x+vAhEUd8gxM+tCUcTUA==", "061-123-123", false, 2, "JARKXVHAKD3A573EERBMKD5JCFWP3JAO", false, "mustafa@gmail.com" },
                    { "56D30AA9-CADE-46BF-962E-E9873527F491", 0, new Guid("fd72d7b7-a9e8-4740-b370-e3a2a33d61df"), "stamp16", new DateTime(2024, 7, 6, 16, 1, 33, 463, DateTimeKind.Unspecified).AddTicks(3333), "kate.moore@example.com", true, null, true, false, null, new DateTime(2024, 7, 6, 16, 1, 40, 670, DateTimeKind.Unspecified), "KATE.MOORE@EXAMPLE.COM", "KATE.MOORE", "hashedpassword", "061-893-892", true, 4, "stamp15", false, "kate.moore" },
                    { "6789925f-fdf3-4c76-8236-0a50a57f1c2c", 0, null, "73abb213-9331-4e09-bd92-e41e6b21afaf", new DateTime(2024, 8, 25, 17, 36, 31, 519, DateTimeKind.Unspecified).AddTicks(5018), "ramo@gmail.com", true, new Guid("755fc934-044c-40e2-a316-08dcc51a8157"), true, true, null, new DateTime(2024, 8, 25, 17, 36, 31, 519, DateTimeKind.Unspecified).AddTicks(5022), "RAMO@GMAIL.COM", "RAMO@GMAIL.COM", "AQAAAAEAACcQAAAAEJoWDQqjEtTQcudsZPsazKxke5PHOfUTidMF+bLcALUjL5bGojnJuusqpf2xyhZ3Dg==", "061-123-132", false, 2, "25JG4G5BRBLTBEV4AZGADU5MOTVZ7EPL", false, "ramo@gmail.com" },
                    { "67be2123-c0ad-4b94-a4c3-cbfe002a7313", 0, new Guid("67be2123-c0ad-4b94-a4c3-cbfe002a7313"), "6580062c-915f-4c69-a4e0-13056532d87c", new DateTime(2024, 7, 6, 16, 1, 33, 463, DateTimeKind.Unspecified).AddTicks(3333), "klijent_test@hotmail.com", true, null, true, true, null, new DateTime(2024, 7, 6, 16, 1, 40, 670, DateTimeKind.Unspecified), "KLIJENT_TEST@HOTMAIL.COM", "KLIJENT_TEST@HOTMAIL.COM", "AQAAAAEAACcQAAAAEJLBH6bjzzbhQ9O2s09wFge92haC4J8HweRIc07q68Nu0lXVpjNEUqtHzTbpod1n9w==", "061-489-372", false, 4, "AGMXKTN3R7TDVKD7LKETTYNDGAOMBW3T", false, "klijent_test@hotmail.com" },
                    { "67fc1dec-4ba5-460e-b4a1-eabae216efe9", 0, new Guid("52c17ce6-c3c2-49f8-246f-08dc9dc44f56"), "6d9f1629-ff38-46ef-b186-b3f2d6c81536", new DateTime(2024, 7, 6, 16, 2, 44, 904, DateTimeKind.Unspecified).AddTicks(8493), "tajib.tajci@gmail.com", true, null, true, true, null, new DateTime(2024, 7, 6, 16, 2, 44, 904, DateTimeKind.Unspecified).AddTicks(8864), "TAJIB.TAJCI@GMAIL.COM", "TAJIB.TAJCI@GMAIL.COM", "AQAAAAEAACcQAAAAEIO2utFzqqj34h3e8GGYrZkm3XeTb6FS4EiO+9M27+v66nuvCUC2wleFKIF7GJ/hag==", "061-523-472", false, 4, "2PP7UUACNXYPT5QBLHMIWDSBG6HDEEIQ", false, "tajib.tajci@gmail.com" },
                    { "6A40A586-6D3E-4001-A06E-EEFEAA85DF37", 0, new Guid("157d8713-1176-4050-b53f-49e38670337a"), "stamp4", new DateTime(2024, 7, 6, 16, 1, 33, 463, DateTimeKind.Unspecified).AddTicks(3333), "jane.smith@example.com", true, null, true, false, null, new DateTime(2024, 7, 6, 16, 1, 40, 670, DateTimeKind.Unspecified), "JANE.SMITH@EXAMPLE.COM", "JANE.SMITH", "hashedpassword", "061-654-918", true, 4, "stamp3", false, "jane.smith" },
                    { "7B0BE148-F7D3-48A4-8E0C-47966E7AC7A7", 0, new Guid("0523fca9-d9bd-4e14-98f5-aa73c208a274"), "stamp12", new DateTime(2024, 7, 6, 16, 1, 33, 463, DateTimeKind.Unspecified).AddTicks(3333), "laura.miller@example.com", true, null, true, false, null, new DateTime(2024, 7, 6, 16, 1, 40, 670, DateTimeKind.Unspecified), "LAURA.MILLER@EXAMPLE.COM", "LAURA.MILLER", "hashedpassword", "062-356-542", true, 4, "stamp11", false, "laura.miller" },
                    { "8a55e790-9562-45aa-a1e9-4c26022582d2", 0, null, "c5bac6bd-f3c5-4c74-a662-8e96b8ef02e4", new DateTime(2024, 8, 25, 17, 28, 0, 859, DateTimeKind.Unspecified).AddTicks(1093), "test1@gmail.com", true, new Guid("e9605352-c3f4-4568-a312-08dcc51a8157"), true, true, null, new DateTime(2024, 8, 25, 17, 28, 0, 859, DateTimeKind.Unspecified).AddTicks(2151), "TEST1@GMAIL.COM", "TEST1@GMAIL.COM", "AQAAAAEAACcQAAAAEOuAdIUWoQbdZs+kXV8J2P1OUkFdVYZLpZB/S1lNpz4uelQzhOP2XamTwAgMxMBxRA==", "061-123-137", false, 3, "F5OHSA4PAVFBCKD7Y4TLLIHC35YXQJPT", false, "test1@gmail.com" },
                    { "8b778d69-5d48-4690-8d8a-945c7fb250b1", 0, null, "45e54582-df33-48fd-970f-6ee16162fb31", new DateTime(2024, 7, 6, 16, 1, 33, 463, DateTimeKind.Unspecified).AddTicks(3333), "vodic_test@hotmail.com", true, new Guid("8b778d69-5d48-4690-8d8a-945c7fb250b1"), true, true, null, new DateTime(2024, 7, 6, 16, 1, 40, 670, DateTimeKind.Unspecified), "VODIC_TEST@HOTMAIL.COM", "VODIC_TEST@HOTMAIL.COM", "AQAAAAEAACcQAAAAEMkrQ5sL16s5tbNUH2zj6qxiVdfbI/umWC6Ljy+ntm2T89rxhaRz92T+xNGmWRsECQ==", "061-270-329", false, 3, "NTQJKPKJVS5HPRZXRAR4SY2FAZ2EIZYQ", false, "vodic_test@hotmail.com" },
                    { "94652C54-5E43-4772-BEE5-4003BA56BCF9", 0, new Guid("c2f846f6-c807-404b-a803-9f1e26e1bdd2"), "stamp8", new DateTime(2024, 7, 6, 16, 1, 33, 463, DateTimeKind.Unspecified).AddTicks(3333), "bob.brown@example.com", true, null, true, false, null, new DateTime(2024, 7, 6, 16, 1, 40, 670, DateTimeKind.Unspecified), "BOB.BROWN@EXAMPLE.COM", "BOB.BROWN", "hashedpassword", "061-104-934", true, 4, "stamp7", false, "bob.brown" },
                    { "A0835469-C39E-4B05-8629-5D1E24225BE5", 0, new Guid("7f5734f6-d8e6-4d66-82d4-f1f7da73a6d3"), "stamp20", new DateTime(2024, 7, 6, 16, 1, 33, 463, DateTimeKind.Unspecified).AddTicks(3333), "pat.anderson@example.com", true, null, true, false, null, new DateTime(2024, 7, 6, 16, 1, 40, 670, DateTimeKind.Unspecified), "PAT.ANDERSON@EXAMPLE.COM", "PAT.ANDERSON", "hashedpassword", "061-094-955", true, 4, "stamp19", false, "pat.anderson" },
                    { "aabf1ca0-abb2-4082-81d9-4275fcdd080b", 0, null, "40274e17-5602-46ad-b834-50e9e95e8f38", new DateTime(2024, 7, 6, 16, 1, 33, 463, DateTimeKind.Unspecified).AddTicks(3333), "test000@gmail.com", false, new Guid("16cda006-dc9c-4071-012e-08dc81a7b237"), false, true, null, new DateTime(2024, 7, 6, 16, 1, 40, 670, DateTimeKind.Unspecified), "TEST000@GMAIL.COM", "TEST000@GMAIL.COM", "AQAAAAEAACcQAAAAEGMU8Nuzt+y5Di+3SmGe3IVfIsTYuTGU3TcVIDDAXT2EOI0jDfwrsaY5RnORds0O5w==", "062-628-127", false, 3, "JCAKSDLDID7NXAWJNIECMS7OFMS6UROF", false, "test000@gmail.com" },
                    { "B5B08AF1-E932-4F4E-A6C8-D93F125843A5", 0, new Guid("534322c0-4ea4-488a-946c-e67d044ba637"), "stamp18", new DateTime(2024, 7, 6, 16, 1, 33, 463, DateTimeKind.Unspecified).AddTicks(3333), "chris.taylor@example.com", true, null, true, false, null, new DateTime(2024, 7, 6, 16, 1, 40, 670, DateTimeKind.Unspecified), "CHRIS.TAYLOR@EXAMPLE.COM", "CHRIS.TAYLOR", "hashedpassword", "061-691-581", true, 4, "stamp17", false, "chris.taylor" },
                    { "bb23c4f8-afc5-401f-91d0-e8908648c60e", 0, null, "b72db4c2-ec80-4ae1-97ad-9cf1347a29fd", new DateTime(2024, 7, 6, 16, 1, 33, 463, DateTimeKind.Unspecified).AddTicks(3333), "admir.numanovic@edu.fit.ba", true, new Guid("9ec6edeb-02b6-4c4f-139d-08dc2fe46497"), true, false, null, new DateTime(2024, 7, 6, 16, 1, 40, 670, DateTimeKind.Unspecified), "ADMIR.NUMANOVIC@EDU.FIT.BA", "ADMIR.NUMANOVIC@EDU.FIT.BA", "AQAAAAEAACcQAAAAEGTkBva2G9v0eN6XBZpP2OLPNfds2oJdwtlN9u1Ad1VeASijrsQWfS2gBmnXKKo8RQ==", "062-538-720", false, 2, "7ROQWKA54SRX6ZRB5ON3DE4D673UAWQU", false, "admir.numanovic@edu.fit.ba" },
                    { "bf85b04a-2c8d-4e29-a9f3-0b34210b7d63", 0, null, "40c09cc1-9733-4e1f-baad-6bbfa445e2d5", new DateTime(2024, 7, 6, 16, 1, 33, 463, DateTimeKind.Unspecified).AddTicks(3333), "admir.numanovic@breakpoint.ba", true, null, true, true, null, new DateTime(2024, 7, 6, 16, 1, 40, 670, DateTimeKind.Unspecified), "ADMIR.NUMANOVIC@BREAKPOINT.BA", "ADMIN", "AQAAAAEAACcQAAAAENH0ZVxGgQZyo4NnFobBHHR/ZHO9yD6QfWgrjuR56mNlnl5lPyfCBXRhyp9xyqVxSw==", "061-644-113", false, 1, "AV2AUNOYYIOW4DIWP4TEEO2CZGP63IOJ", false, "admin" },
                    { "C455A79A-9D56-4540-B499-4CE9F84B1644", 0, new Guid("e61d7eee-9996-4dd5-b987-37f10bddb38c"), "stamp2", new DateTime(2024, 7, 6, 16, 1, 33, 463, DateTimeKind.Unspecified).AddTicks(3333), "john.doe@example.com", true, null, true, false, null, new DateTime(2024, 7, 6, 16, 1, 40, 670, DateTimeKind.Unspecified), "JOHN.DOE@EXAMPLE.COM", "JOHN.DOE", "hashedpassword", "062-192-599", true, 4, "stamp1", false, "john.doe" },
                    { "c6e805ba-56eb-4958-913d-cabd5f5f8495", 0, null, "2e6556ae-9b3a-4484-9d82-51b7be1d2be4", new DateTime(2024, 7, 6, 16, 1, 33, 463, DateTimeKind.Unspecified).AddTicks(3333), "agencija_test@hotmail.com", true, new Guid("d0903cb4-e560-4547-37b0-08dc95e40210"), true, true, null, new DateTime(2024, 7, 6, 16, 1, 40, 670, DateTimeKind.Unspecified), "AGENCIJA_TEST@HOTMAIL.COM", "AGENCIJA_TEST@HOTMAIL.COM", "AQAAAAEAACcQAAAAELYbOWUuwolhTeB1FGvz9P33Sn54V1mnBYNLU+5sbugdOVvmYS3/rbQQUtJD6LdnCQ==", "062-827-446", false, 2, "B26Z6UJIJBHTYARQQSDTLZ54YUWYRJVR", false, "agencija_test@hotmail.com" },
                    { "d0b17a41-d2b6-40cc-9f70-45ec162ec043", 0, null, "f89172c2-a986-4e7e-b0bf-933afa59f3e9", new DateTime(2024, 8, 25, 17, 31, 14, 161, DateTimeKind.Unspecified).AddTicks(5768), "mujo@gmail.com", true, new Guid("0403e6d4-7cfe-4f25-a313-08dcc51a8157"), true, true, null, new DateTime(2024, 8, 25, 17, 31, 14, 161, DateTimeKind.Unspecified).AddTicks(5774), "MUJO@GMAIL.COM", "MUJO@GMAIL.COM", "AQAAAAEAACcQAAAAEOfxeVrbwzk3GnILWlwIzrNIbCZQ+M0oGX/MkcIzJeYU7ziV8eR+yrfwZYBEydJc2w==", "061-123-4555", false, 3, "YZFZZB243BCYQTUHZJQPAX2ODV4QJWPF", false, "mujo@gmail.com" },
                    { "d143d790-dd84-483b-819a-5423303c4ceb", 0, new Guid("20affdb3-aa89-4387-d057-08dc9dc24d12"), "30e39890-6ed9-4831-abf6-f88b56401ff4", new DateTime(2024, 7, 6, 16, 1, 33, 463, DateTimeKind.Unspecified).AddTicks(3333), "tajci@gmail.com", true, null, true, true, null, new DateTime(2024, 7, 6, 16, 1, 40, 670, DateTimeKind.Unspecified), "TAJCI@GMAIL.COM", "TAJCI@GMAIL.COM", "AQAAAAEAACcQAAAAEDZUnbMMju1jWfVrzDvO0S0i9bWzj1PmZgCmClJWp0EOZVOSVerS4DLf8TfvJfYjIw==", "062-182-846", false, 4, "5CGMM26WVRIY6TZZ47NIZ6EKQVZDHYZE", false, "tajci@gmail.com" },
                    { "F1741DE6-1DF3-4C61-BB1A-596ADA2E8F53", 0, new Guid("5780ee88-d758-4ead-861a-cd19e001bf3d"), "stamp14", new DateTime(2024, 7, 6, 16, 1, 33, 463, DateTimeKind.Unspecified).AddTicks(3333), "sam.wilson@example.com", true, null, true, false, null, new DateTime(2024, 7, 6, 16, 1, 40, 670, DateTimeKind.Unspecified), "SAM.WILSON@EXAMPLE.COM", "SAM.WILSON", "hashedpassword", "061-988-621", true, 4, "stamp13", false, "sam.wilson" }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "0bfe9409-2df3-405a-b34d-530f8e86293b");

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "0dfe16b1-d7cd-4545-be89-a0f3f5945de4");

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "0EA58B09-0CC1-4DDD-8D89-35F820CC740B");

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "13EC6CFE-DE83-400E-A80B-C38E39FDBE56");

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "39539a1b-2f21-46f7-87b8-bee4745d50f7");

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "56D30AA9-CADE-46BF-962E-E9873527F491");

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "6789925f-fdf3-4c76-8236-0a50a57f1c2c");

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "67be2123-c0ad-4b94-a4c3-cbfe002a7313");

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "67fc1dec-4ba5-460e-b4a1-eabae216efe9");

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "6A40A586-6D3E-4001-A06E-EEFEAA85DF37");

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "7B0BE148-F7D3-48A4-8E0C-47966E7AC7A7");

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "8a55e790-9562-45aa-a1e9-4c26022582d2");

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "8b778d69-5d48-4690-8d8a-945c7fb250b1");

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "94652C54-5E43-4772-BEE5-4003BA56BCF9");

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "A0835469-C39E-4B05-8629-5D1E24225BE5");

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "aabf1ca0-abb2-4082-81d9-4275fcdd080b");

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "B5B08AF1-E932-4F4E-A6C8-D93F125843A5");

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "bb23c4f8-afc5-401f-91d0-e8908648c60e");

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "bf85b04a-2c8d-4e29-a9f3-0b34210b7d63");

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "C455A79A-9D56-4540-B499-4CE9F84B1644");

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "c6e805ba-56eb-4958-913d-cabd5f5f8495");

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "d0b17a41-d2b6-40cc-9f70-45ec162ec043");

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "d143d790-dd84-483b-819a-5423303c4ceb");

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "F1741DE6-1DF3-4C61-BB1A-596ADA2E8F53");
        }
    }
}
