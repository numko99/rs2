using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Iter.Infrastructure.Migrations
{
    public partial class Seed1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "ArrangementStatus",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 0, "U Pripremi" },
                    { 1, "Dostupan za Rezervaciju" },
                    { 2, "Rezervacije Zatvorene" },
                    { 10, "U Pripremi" },
                    { 11, "U Pripremi" },
                    { 12, "U Pripremi" },
                    { 13, "U Pripremi" },
                    { 14, "U Pripremi" }
                });

            migrationBuilder.InsertData(
                table: "Client",
                columns: new[] { "Id", "BirthDate", "CreatedAt", "FirstName", "IsDeleted", "LastName", "ModifiedAt", "ResidencePlace" },
                values: new object[,]
                {
                    { new Guid("0523fca9-d9bd-4e14-98f5-aa73c208a274"), new DateTime(1990, 2, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 6, 24, 0, 0, 0, 0, DateTimeKind.Unspecified), "Jane", false, "Smith", new DateTime(2024, 6, 24, 0, 0, 0, 0, DateTimeKind.Unspecified), "Los Angeles" },
                    { new Guid("157d8713-1176-4050-b53f-49e38670337a"), new DateTime(1985, 3, 22, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 6, 23, 0, 0, 0, 0, DateTimeKind.Unspecified), "Alice", false, "Johnson", new DateTime(2024, 6, 23, 0, 0, 0, 0, DateTimeKind.Unspecified), "Chicago" },
                    { new Guid("1eb9eee6-d946-4111-a2e6-a3a67445ad97"), new DateTime(1992, 5, 16, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 6, 21, 0, 0, 0, 0, DateTimeKind.Unspecified), "Mike", false, "Davis", new DateTime(2024, 6, 21, 0, 0, 0, 0, DateTimeKind.Unspecified), "Phoenix" },
                    { new Guid("20affdb3-aa89-4387-d057-08dc9dc24d12"), new DateTime(1999, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 7, 6, 15, 48, 22, 101, DateTimeKind.Unspecified).AddTicks(4672), "Tajib", false, "Vikalo", new DateTime(2024, 7, 6, 15, 48, 22, 101, DateTimeKind.Unspecified).AddTicks(5430), "Srebreni" },
                    { new Guid("52c17ce6-c3c2-49f8-246f-08dc9dc44f56"), new DateTime(1999, 11, 11, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 7, 6, 16, 2, 44, 904, DateTimeKind.Unspecified).AddTicks(6235), "Tajib", false, "Tajci", new DateTime(2024, 7, 6, 16, 2, 44, 904, DateTimeKind.Unspecified).AddTicks(6632), "Mostar" },
                    { new Guid("534322c0-4ea4-488a-946c-e67d044ba637"), new DateTime(1980, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 6, 25, 0, 0, 0, 0, DateTimeKind.Unspecified), "John", false, "Doe", new DateTime(2024, 6, 25, 0, 0, 0, 0, DateTimeKind.Unspecified), "New York" },
                    { new Guid("5780ee88-d758-4ead-861a-cd19e001bf3d"), new DateTime(1975, 4, 30, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 6, 22, 0, 0, 0, 0, DateTimeKind.Unspecified), "Bob", false, "Brown", new DateTime(2024, 6, 22, 0, 0, 0, 0, DateTimeKind.Unspecified), "Houston" },
                    { new Guid("6240d8f1-b80d-49e3-77d0-08dc8003d380"), new DateTime(1999, 6, 7, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 5, 29, 19, 33, 4, 985, DateTimeKind.Unspecified).AddTicks(3286), "Admir", false, "Numanović", new DateTime(2024, 5, 29, 19, 35, 3, 925, DateTimeKind.Unspecified).AddTicks(4234), "Mostar" },
                    { new Guid("67be2123-c0ad-4b94-a4c3-cbfe002a7313"), new DateTime(1999, 2, 16, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 2, 16, 0, 0, 0, 0, DateTimeKind.Unspecified), "Adnann", false, "Numanovic", new DateTime(2024, 8, 24, 18, 0, 25, 468, DateTimeKind.Unspecified).AddTicks(6081), "Tuzla" },
                    { new Guid("7f5734f6-d8e6-4d66-82d4-f1f7da73a6d3"), new DateTime(1983, 8, 11, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 6, 18, 0, 0, 0, 0, DateTimeKind.Unspecified), "Kate", false, "Moore", new DateTime(2024, 6, 18, 0, 0, 0, 0, DateTimeKind.Unspecified), "San Diego" },
                    { new Guid("8fd6aa7e-2afa-402b-a05c-541ab2ddbc5a"), new DateTime(1970, 7, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 6, 19, 0, 0, 0, 0, DateTimeKind.Unspecified), "Sam", false, "Wilson", new DateTime(2024, 6, 19, 0, 0, 0, 0, DateTimeKind.Unspecified), "San Antonio" },
                    { new Guid("c2f846f6-c807-404b-a803-9f1e26e1bdd2"), new DateTime(1988, 6, 9, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 6, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), "Laura", false, "Miller", new DateTime(2024, 6, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), "Philadelphia" },
                    { new Guid("e61d7eee-9996-4dd5-b987-37f10bddb38c"), new DateTime(1965, 10, 30, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 6, 16, 0, 0, 0, 0, DateTimeKind.Unspecified), "Pat", false, "Anderson", new DateTime(2024, 6, 16, 0, 0, 0, 0, DateTimeKind.Unspecified), "San Jose" },
                    { new Guid("fd72d7b7-a9e8-4740-b370-e3a2a33d61df"), new DateTime(1995, 9, 25, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 6, 17, 0, 0, 0, 0, DateTimeKind.Unspecified), "Chris", false, "Taylor", new DateTime(2024, 6, 17, 0, 0, 0, 0, DateTimeKind.Unspecified), "Dallas" },
                    { new Guid("fda36cb7-4616-48e9-cf2a-08dc728077ad"), new DateTime(2001, 12, 3, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 5, 12, 14, 38, 46, 817, DateTimeKind.Unspecified).AddTicks(1754), "Klijent", false, "Mujic", new DateTime(2024, 5, 22, 0, 47, 41, 807, DateTimeKind.Unspecified).AddTicks(7223), "Mostar" }
                });

            migrationBuilder.InsertData(
                table: "Country",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "Bosna" },
                    { 2, "Hrvatska" },
                    { 3, "Austrija" },
                    { 4, "Njemacka" },
                    { 5, "Italija" },
                    { 6, "Francuska" },
                    { 7, "Španija" },
                    { 8, "Portugal" },
                    { 9, "Turska" },
                    { 10, "Grcka" },
                    { 11, "Ceška" },
                    { 12, "Madarska" },
                    { 13, "Nizozemska" }
                });

            migrationBuilder.InsertData(
                table: "ReservationStatus",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "Na čekanju" },
                    { 2, "Odbijeno" },
                    { 3, "Otkazano" },
                    { 4, "Potvrđeno" },
                    { 6, "Isteklo" }
                });

            migrationBuilder.InsertData(
                table: "City",
                columns: new[] { "Id", "CountryId", "Name" },
                values: new object[,]
                {
                    { 1, 3, "Vienna" },
                    { 2, 3, "Graz" },
                    { 3, 3, "Linz" },
                    { 4, 3, "Salzburg" },
                    { 5, 3, "Innsbruck" },
                    { 6, 3, "Klagenfurt" },
                    { 7, 3, "Villach" },
                    { 8, 3, "Wels" },
                    { 9, 3, "Sankt Pölten" },
                    { 10, 3, "Dornbirn" },
                    { 11, 4, "Berlin" },
                    { 12, 4, "Munich" },
                    { 13, 4, "Frankfurt" },
                    { 14, 4, "Hamburg" },
                    { 15, 4, "Cologne" },
                    { 16, 4, "Stuttgart" },
                    { 17, 4, "Düsseldorf" },
                    { 18, 4, "Dortmund" },
                    { 19, 4, "Essen" },
                    { 20, 4, "Leipzig" },
                    { 21, 5, "Rome" },
                    { 22, 5, "Milan" },
                    { 23, 5, "Naples" },
                    { 24, 5, "Turin" },
                    { 25, 5, "Palermo" },
                    { 26, 5, "Genoa" },
                    { 27, 5, "Bologna" },
                    { 28, 5, "Florence" },
                    { 29, 5, "Bari" },
                    { 30, 5, "Catania" },
                    { 31, 6, "Paris" },
                    { 32, 6, "Marseille" },
                    { 33, 6, "Lyon" },
                    { 34, 6, "Toulouse" },
                    { 35, 6, "Nice" },
                    { 36, 6, "Nantes" },
                    { 37, 6, "Strasbourg" },
                    { 38, 6, "Montpellier" },
                    { 39, 6, "Bordeaux" },
                    { 40, 6, "Lille" },
                    { 41, 7, "Madrid" },
                    { 42, 7, "Barcelona" }
                });

            migrationBuilder.InsertData(
                table: "City",
                columns: new[] { "Id", "CountryId", "Name" },
                values: new object[,]
                {
                    { 43, 7, "Valencia" },
                    { 44, 7, "Seville" },
                    { 45, 7, "Zaragoza" },
                    { 46, 7, "Malaga" },
                    { 47, 7, "Murcia" },
                    { 48, 7, "Palma" },
                    { 49, 7, "Las Palmas" },
                    { 50, 7, "Bilbao" },
                    { 51, 8, "Lisabon" },
                    { 52, 8, "Porto" },
                    { 53, 8, "Braga" },
                    { 54, 8, "Funchal" },
                    { 55, 8, "Coimbra" },
                    { 56, 8, "Setúbal" },
                    { 57, 8, "Almada" },
                    { 58, 8, "Aveiro" },
                    { 59, 8, "Évora" },
                    { 60, 8, "Faro" },
                    { 61, 9, "Istanbul" },
                    { 62, 9, "Ankara" },
                    { 63, 9, "Izmir" },
                    { 64, 9, "Bursa" },
                    { 65, 9, "Adana" },
                    { 66, 9, "Gaziantep" },
                    { 67, 9, "Konya" },
                    { 68, 9, "Antalya" },
                    { 69, 9, "Kayseri" },
                    { 70, 9, "Mersin" },
                    { 71, 10, "Athens" },
                    { 72, 10, "Thessaloniki" },
                    { 73, 10, "Patras" },
                    { 74, 10, "Heraklion" },
                    { 75, 10, "Larissa" },
                    { 76, 10, "Volos" },
                    { 77, 10, "Ioannina" },
                    { 78, 10, "Trikala" },
                    { 79, 10, "Chania" },
                    { 80, 10, "Kavala" },
                    { 81, 11, "Prague" },
                    { 82, 11, "Brno" },
                    { 83, 11, "Ostrava" },
                    { 84, 11, "Plzen" }
                });

            migrationBuilder.InsertData(
                table: "City",
                columns: new[] { "Id", "CountryId", "Name" },
                values: new object[,]
                {
                    { 85, 11, "Liberec" },
                    { 86, 11, "Olomouc" },
                    { 87, 11, "Ústí nad Labem" },
                    { 88, 11, "Hradec Králové" },
                    { 89, 11, "Ceské Budejovice" },
                    { 90, 11, "Pardubice" },
                    { 91, 12, "Budapest" },
                    { 92, 12, "Debrecen" },
                    { 93, 12, "Szeged" },
                    { 94, 12, "Miskolc" },
                    { 95, 12, "Pécs" },
                    { 96, 12, "Gyor" },
                    { 97, 12, "Nyíregyháza" },
                    { 98, 12, "Kecskemét" },
                    { 99, 12, "Székesfehérvár" },
                    { 100, 12, "Szombathely" },
                    { 101, 1, "Sarajevo" },
                    { 102, 1, "Mostar" },
                    { 103, 1, "Banja Luka" },
                    { 104, 1, "Tuzla" },
                    { 105, 1, "Zenica" },
                    { 106, 1, "Bihac" },
                    { 107, 1, "Brcko" },
                    { 108, 1, "Travnik" },
                    { 109, 1, "Doboj" },
                    { 110, 1, "Prijedor" },
                    { 111, 2, "Zagreb" },
                    { 112, 2, "Split" },
                    { 113, 2, "Rijeka" },
                    { 114, 2, "Osijek" },
                    { 115, 2, "Zadar" },
                    { 116, 2, "Slavonski Brod" },
                    { 117, 2, "Pula" },
                    { 118, 2, "Karlovac" },
                    { 119, 2, "Varaždin" },
                    { 120, 2, "Sibenik" },
                    { 121, 13, "Amsterdam" },
                    { 122, 5, "Venice" },
                    { 123, 2, "Makarska" },
                    { 124, 2, "Dubrovnik" }
                });

            migrationBuilder.InsertData(
                table: "Address",
                columns: new[] { "Id", "CityId", "CreatedAt", "HouseNumber", "IsDeleted", "ModifiedAt", "PostalCode", "Street" },
                values: new object[] { new Guid("39c6bbd9-57a1-4b5a-3d83-08dc8233593d"), 104, new DateTime(2024, 6, 1, 14, 7, 3, 125, DateTimeKind.Unspecified).AddTicks(7525), "55", false, new DateTime(2024, 8, 23, 23, 48, 15, 975, DateTimeKind.Unspecified).AddTicks(3442), "77000", "Ulica Ivana krndelja" });

            migrationBuilder.InsertData(
                table: "Address",
                columns: new[] { "Id", "CityId", "CreatedAt", "HouseNumber", "IsDeleted", "ModifiedAt", "PostalCode", "Street" },
                values: new object[] { new Guid("462e18e6-5009-43b5-15be-08dc298c5934"), 101, new DateTime(2024, 2, 9, 17, 37, 28, 33, DateTimeKind.Unspecified).AddTicks(5556), "99", false, new DateTime(2024, 6, 1, 14, 5, 34, 722, DateTimeKind.Unspecified).AddTicks(2045), "1234", "Ivana krndelja" });

            migrationBuilder.InsertData(
                table: "Address",
                columns: new[] { "Id", "CityId", "CreatedAt", "HouseNumber", "IsDeleted", "ModifiedAt", "PostalCode", "Street" },
                values: new object[] { new Guid("8af7c2da-c52a-4322-b2d7-08dc37b68f24"), 101, new DateTime(2024, 2, 27, 18, 7, 20, 446, DateTimeKind.Unspecified).AddTicks(9789), "99", false, new DateTime(2024, 8, 20, 22, 28, 37, 937, DateTimeKind.Unspecified).AddTicks(1298), "74000", "uLICA MARSALA TITA" });

            migrationBuilder.InsertData(
                table: "Agency",
                columns: new[] { "Id", "AddressId", "ContactEmail", "ContactPhone", "CreatedAt", "ImageId", "IsActive", "IsDeleted", "LicenseNumber", "ModifiedAt", "Name", "Rating", "Website" },
                values: new object[] { new Guid("1b73b899-6578-4120-0caf-08dc8233593a"), new Guid("39c6bbd9-57a1-4b5a-3d83-08dc8233593d"), "trans.tourist@gmail.com", "035-882-085", new DateTime(2024, 6, 1, 14, 7, 3, 125, DateTimeKind.Unspecified).AddTicks(7917), null, false, false, "01475214", new DateTime(2024, 8, 23, 23, 48, 15, 975, DateTimeKind.Unspecified).AddTicks(9403), "Trans turist", 0.00m, "www.transtourist.com" });

            migrationBuilder.InsertData(
                table: "Agency",
                columns: new[] { "Id", "AddressId", "ContactEmail", "ContactPhone", "CreatedAt", "ImageId", "IsActive", "IsDeleted", "LicenseNumber", "ModifiedAt", "Name", "Rating", "Website" },
                values: new object[] { new Guid("62f78fa6-4e5e-4420-a91d-08dc37b68f22"), new Guid("8af7c2da-c52a-4322-b2d7-08dc37b68f24"), "pohodi@gmail.com", "062-123-1234", new DateTime(2024, 2, 27, 18, 7, 20, 447, DateTimeKind.Unspecified).AddTicks(3028), null, false, false, "1234", new DateTime(2024, 8, 20, 22, 28, 37, 937, DateTimeKind.Unspecified).AddTicks(2096), "Studentski pohodi", 0.00m, "www.pohodi.com" });

            migrationBuilder.InsertData(
                table: "Agency",
                columns: new[] { "Id", "AddressId", "ContactEmail", "ContactPhone", "CreatedAt", "ImageId", "IsActive", "IsDeleted", "LicenseNumber", "ModifiedAt", "Name", "Rating", "Website" },
                values: new object[] { new Guid("66f20b4e-9e6d-4c95-6a66-08dc298d67da"), new Guid("462e18e6-5009-43b5-15be-08dc298c5934"), "letsgo@gmail.com", "062-123-123", new DateTime(2024, 2, 9, 17, 37, 25, 271, DateTimeKind.Unspecified).AddTicks(2606), null, false, false, "12343", new DateTime(2024, 6, 1, 14, 5, 34, 722, DateTimeKind.Unspecified).AddTicks(5465), "Letsgo", 3.08m, "www.letsgo.com" });

            migrationBuilder.InsertData(
                table: "Employee",
                columns: new[] { "Id", "AgencyId", "BirthDate", "CreatedAt", "FirstName", "IsDeleted", "LastName", "ModifiedAt", "ResidencePlace" },
                values: new object[,]
                {
                    { new Guid("027d20a4-dcc3-4aee-a315-08dcc51a8157"), new Guid("1b73b899-6578-4120-0caf-08dc8233593a"), new DateTime(2005, 8, 25, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 8, 25, 17, 34, 7, 158, DateTimeKind.Unspecified).AddTicks(8909), "Mustafa", false, "Koordinator", new DateTime(2024, 8, 25, 17, 34, 7, 158, DateTimeKind.Unspecified).AddTicks(8993), "Ribnica" },
                    { new Guid("0403e6d4-7cfe-4f25-a313-08dcc51a8157"), new Guid("66f20b4e-9e6d-4c95-6a66-08dc298d67da"), new DateTime(2000, 8, 25, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 8, 25, 17, 31, 14, 161, DateTimeKind.Unspecified).AddTicks(5638), "Mujo", false, "Vodič", new DateTime(2024, 8, 25, 17, 45, 20, 188, DateTimeKind.Unspecified).AddTicks(8628), "Vrapčići" },
                    { new Guid("16cda006-dc9c-4071-012e-08dc81a7b237"), new Guid("62f78fa6-4e5e-4420-a91d-08dc37b68f22"), new DateTime(1999, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 5, 31, 21, 27, 6, 984, DateTimeKind.Unspecified).AddTicks(6437), "Hašim", false, "Vodič", new DateTime(2024, 8, 25, 17, 31, 53, 599, DateTimeKind.Unspecified).AddTicks(4486), "Mostar" },
                    { new Guid("755fc934-044c-40e2-a316-08dcc51a8157"), new Guid("66f20b4e-9e6d-4c95-6a66-08dc298d67da"), new DateTime(1997, 8, 25, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 8, 25, 17, 36, 31, 519, DateTimeKind.Unspecified).AddTicks(4870), "Ramo", false, "Koordinator", new DateTime(2024, 8, 25, 17, 36, 31, 519, DateTimeKind.Unspecified).AddTicks(4931), "Ribnica" },
                    { new Guid("8b778d69-5d48-4690-8d8a-945c7fb250b1"), new Guid("62f78fa6-4e5e-4420-a91d-08dc37b68f22"), new DateTime(1999, 7, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(1999, 6, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Dalila", false, "Bajrić", new DateTime(2024, 8, 15, 12, 35, 12, 550, DateTimeKind.Unspecified).AddTicks(5416), "Bugojno" },
                    { new Guid("9ec6edeb-02b6-4c4f-139d-08dc2fe46497"), new Guid("66f20b4e-9e6d-4c95-6a66-08dc298d67da"), new DateTime(2001, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Test", true, "Test", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Zenica" },
                    { new Guid("d0903cb4-e560-4547-37b0-08dc95e40210"), new Guid("62f78fa6-4e5e-4420-a91d-08dc37b68f22"), new DateTime(1999, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 6, 26, 15, 29, 29, 895, DateTimeKind.Unspecified).AddTicks(3378), "Koordinator", false, "Koordinatorić", new DateTime(2024, 6, 26, 15, 29, 29, 895, DateTimeKind.Unspecified).AddTicks(3806), "Mostar" },
                    { new Guid("e9605352-c3f4-4568-a312-08dcc51a8157"), new Guid("66f20b4e-9e6d-4c95-6a66-08dc298d67da"), new DateTime(1999, 8, 25, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 8, 25, 17, 28, 0, 858, DateTimeKind.Unspecified).AddTicks(527), "Izet", false, "Vodič", new DateTime(2024, 8, 25, 17, 32, 13, 390, DateTimeKind.Unspecified).AddTicks(8083), "Ribnica Mujići" },
                    { new Guid("f6fe658e-37f8-4a52-a314-08dcc51a8157"), new Guid("1b73b899-6578-4120-0caf-08dc8233593a"), new DateTime(1994, 8, 25, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 8, 25, 17, 32, 57, 584, DateTimeKind.Unspecified).AddTicks(2989), "Elvir", false, "Vodič", new DateTime(2024, 8, 25, 17, 32, 57, 584, DateTimeKind.Unspecified).AddTicks(3113), "Bugojno" }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "ArrangementStatus",
                keyColumn: "Id",
                keyValue: 0);

            migrationBuilder.DeleteData(
                table: "ArrangementStatus",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "ArrangementStatus",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "ArrangementStatus",
                keyColumn: "Id",
                keyValue: 10);

            migrationBuilder.DeleteData(
                table: "ArrangementStatus",
                keyColumn: "Id",
                keyValue: 11);

            migrationBuilder.DeleteData(
                table: "ArrangementStatus",
                keyColumn: "Id",
                keyValue: 12);

            migrationBuilder.DeleteData(
                table: "ArrangementStatus",
                keyColumn: "Id",
                keyValue: 13);

            migrationBuilder.DeleteData(
                table: "ArrangementStatus",
                keyColumn: "Id",
                keyValue: 14);

            migrationBuilder.DeleteData(
                table: "City",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "City",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "City",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "City",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "City",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "City",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "City",
                keyColumn: "Id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "City",
                keyColumn: "Id",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "City",
                keyColumn: "Id",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "City",
                keyColumn: "Id",
                keyValue: 10);

            migrationBuilder.DeleteData(
                table: "City",
                keyColumn: "Id",
                keyValue: 11);

            migrationBuilder.DeleteData(
                table: "City",
                keyColumn: "Id",
                keyValue: 12);

            migrationBuilder.DeleteData(
                table: "City",
                keyColumn: "Id",
                keyValue: 13);

            migrationBuilder.DeleteData(
                table: "City",
                keyColumn: "Id",
                keyValue: 14);

            migrationBuilder.DeleteData(
                table: "City",
                keyColumn: "Id",
                keyValue: 15);

            migrationBuilder.DeleteData(
                table: "City",
                keyColumn: "Id",
                keyValue: 16);

            migrationBuilder.DeleteData(
                table: "City",
                keyColumn: "Id",
                keyValue: 17);

            migrationBuilder.DeleteData(
                table: "City",
                keyColumn: "Id",
                keyValue: 18);

            migrationBuilder.DeleteData(
                table: "City",
                keyColumn: "Id",
                keyValue: 19);

            migrationBuilder.DeleteData(
                table: "City",
                keyColumn: "Id",
                keyValue: 20);

            migrationBuilder.DeleteData(
                table: "City",
                keyColumn: "Id",
                keyValue: 21);

            migrationBuilder.DeleteData(
                table: "City",
                keyColumn: "Id",
                keyValue: 22);

            migrationBuilder.DeleteData(
                table: "City",
                keyColumn: "Id",
                keyValue: 23);

            migrationBuilder.DeleteData(
                table: "City",
                keyColumn: "Id",
                keyValue: 24);

            migrationBuilder.DeleteData(
                table: "City",
                keyColumn: "Id",
                keyValue: 25);

            migrationBuilder.DeleteData(
                table: "City",
                keyColumn: "Id",
                keyValue: 26);

            migrationBuilder.DeleteData(
                table: "City",
                keyColumn: "Id",
                keyValue: 27);

            migrationBuilder.DeleteData(
                table: "City",
                keyColumn: "Id",
                keyValue: 28);

            migrationBuilder.DeleteData(
                table: "City",
                keyColumn: "Id",
                keyValue: 29);

            migrationBuilder.DeleteData(
                table: "City",
                keyColumn: "Id",
                keyValue: 30);

            migrationBuilder.DeleteData(
                table: "City",
                keyColumn: "Id",
                keyValue: 31);

            migrationBuilder.DeleteData(
                table: "City",
                keyColumn: "Id",
                keyValue: 32);

            migrationBuilder.DeleteData(
                table: "City",
                keyColumn: "Id",
                keyValue: 33);

            migrationBuilder.DeleteData(
                table: "City",
                keyColumn: "Id",
                keyValue: 34);

            migrationBuilder.DeleteData(
                table: "City",
                keyColumn: "Id",
                keyValue: 35);

            migrationBuilder.DeleteData(
                table: "City",
                keyColumn: "Id",
                keyValue: 36);

            migrationBuilder.DeleteData(
                table: "City",
                keyColumn: "Id",
                keyValue: 37);

            migrationBuilder.DeleteData(
                table: "City",
                keyColumn: "Id",
                keyValue: 38);

            migrationBuilder.DeleteData(
                table: "City",
                keyColumn: "Id",
                keyValue: 39);

            migrationBuilder.DeleteData(
                table: "City",
                keyColumn: "Id",
                keyValue: 40);

            migrationBuilder.DeleteData(
                table: "City",
                keyColumn: "Id",
                keyValue: 41);

            migrationBuilder.DeleteData(
                table: "City",
                keyColumn: "Id",
                keyValue: 42);

            migrationBuilder.DeleteData(
                table: "City",
                keyColumn: "Id",
                keyValue: 43);

            migrationBuilder.DeleteData(
                table: "City",
                keyColumn: "Id",
                keyValue: 44);

            migrationBuilder.DeleteData(
                table: "City",
                keyColumn: "Id",
                keyValue: 45);

            migrationBuilder.DeleteData(
                table: "City",
                keyColumn: "Id",
                keyValue: 46);

            migrationBuilder.DeleteData(
                table: "City",
                keyColumn: "Id",
                keyValue: 47);

            migrationBuilder.DeleteData(
                table: "City",
                keyColumn: "Id",
                keyValue: 48);

            migrationBuilder.DeleteData(
                table: "City",
                keyColumn: "Id",
                keyValue: 49);

            migrationBuilder.DeleteData(
                table: "City",
                keyColumn: "Id",
                keyValue: 50);

            migrationBuilder.DeleteData(
                table: "City",
                keyColumn: "Id",
                keyValue: 51);

            migrationBuilder.DeleteData(
                table: "City",
                keyColumn: "Id",
                keyValue: 52);

            migrationBuilder.DeleteData(
                table: "City",
                keyColumn: "Id",
                keyValue: 53);

            migrationBuilder.DeleteData(
                table: "City",
                keyColumn: "Id",
                keyValue: 54);

            migrationBuilder.DeleteData(
                table: "City",
                keyColumn: "Id",
                keyValue: 55);

            migrationBuilder.DeleteData(
                table: "City",
                keyColumn: "Id",
                keyValue: 56);

            migrationBuilder.DeleteData(
                table: "City",
                keyColumn: "Id",
                keyValue: 57);

            migrationBuilder.DeleteData(
                table: "City",
                keyColumn: "Id",
                keyValue: 58);

            migrationBuilder.DeleteData(
                table: "City",
                keyColumn: "Id",
                keyValue: 59);

            migrationBuilder.DeleteData(
                table: "City",
                keyColumn: "Id",
                keyValue: 60);

            migrationBuilder.DeleteData(
                table: "City",
                keyColumn: "Id",
                keyValue: 61);

            migrationBuilder.DeleteData(
                table: "City",
                keyColumn: "Id",
                keyValue: 62);

            migrationBuilder.DeleteData(
                table: "City",
                keyColumn: "Id",
                keyValue: 63);

            migrationBuilder.DeleteData(
                table: "City",
                keyColumn: "Id",
                keyValue: 64);

            migrationBuilder.DeleteData(
                table: "City",
                keyColumn: "Id",
                keyValue: 65);

            migrationBuilder.DeleteData(
                table: "City",
                keyColumn: "Id",
                keyValue: 66);

            migrationBuilder.DeleteData(
                table: "City",
                keyColumn: "Id",
                keyValue: 67);

            migrationBuilder.DeleteData(
                table: "City",
                keyColumn: "Id",
                keyValue: 68);

            migrationBuilder.DeleteData(
                table: "City",
                keyColumn: "Id",
                keyValue: 69);

            migrationBuilder.DeleteData(
                table: "City",
                keyColumn: "Id",
                keyValue: 70);

            migrationBuilder.DeleteData(
                table: "City",
                keyColumn: "Id",
                keyValue: 71);

            migrationBuilder.DeleteData(
                table: "City",
                keyColumn: "Id",
                keyValue: 72);

            migrationBuilder.DeleteData(
                table: "City",
                keyColumn: "Id",
                keyValue: 73);

            migrationBuilder.DeleteData(
                table: "City",
                keyColumn: "Id",
                keyValue: 74);

            migrationBuilder.DeleteData(
                table: "City",
                keyColumn: "Id",
                keyValue: 75);

            migrationBuilder.DeleteData(
                table: "City",
                keyColumn: "Id",
                keyValue: 76);

            migrationBuilder.DeleteData(
                table: "City",
                keyColumn: "Id",
                keyValue: 77);

            migrationBuilder.DeleteData(
                table: "City",
                keyColumn: "Id",
                keyValue: 78);

            migrationBuilder.DeleteData(
                table: "City",
                keyColumn: "Id",
                keyValue: 79);

            migrationBuilder.DeleteData(
                table: "City",
                keyColumn: "Id",
                keyValue: 80);

            migrationBuilder.DeleteData(
                table: "City",
                keyColumn: "Id",
                keyValue: 81);

            migrationBuilder.DeleteData(
                table: "City",
                keyColumn: "Id",
                keyValue: 82);

            migrationBuilder.DeleteData(
                table: "City",
                keyColumn: "Id",
                keyValue: 83);

            migrationBuilder.DeleteData(
                table: "City",
                keyColumn: "Id",
                keyValue: 84);

            migrationBuilder.DeleteData(
                table: "City",
                keyColumn: "Id",
                keyValue: 85);

            migrationBuilder.DeleteData(
                table: "City",
                keyColumn: "Id",
                keyValue: 86);

            migrationBuilder.DeleteData(
                table: "City",
                keyColumn: "Id",
                keyValue: 87);

            migrationBuilder.DeleteData(
                table: "City",
                keyColumn: "Id",
                keyValue: 88);

            migrationBuilder.DeleteData(
                table: "City",
                keyColumn: "Id",
                keyValue: 89);

            migrationBuilder.DeleteData(
                table: "City",
                keyColumn: "Id",
                keyValue: 90);

            migrationBuilder.DeleteData(
                table: "City",
                keyColumn: "Id",
                keyValue: 91);

            migrationBuilder.DeleteData(
                table: "City",
                keyColumn: "Id",
                keyValue: 92);

            migrationBuilder.DeleteData(
                table: "City",
                keyColumn: "Id",
                keyValue: 93);

            migrationBuilder.DeleteData(
                table: "City",
                keyColumn: "Id",
                keyValue: 94);

            migrationBuilder.DeleteData(
                table: "City",
                keyColumn: "Id",
                keyValue: 95);

            migrationBuilder.DeleteData(
                table: "City",
                keyColumn: "Id",
                keyValue: 96);

            migrationBuilder.DeleteData(
                table: "City",
                keyColumn: "Id",
                keyValue: 97);

            migrationBuilder.DeleteData(
                table: "City",
                keyColumn: "Id",
                keyValue: 98);

            migrationBuilder.DeleteData(
                table: "City",
                keyColumn: "Id",
                keyValue: 99);

            migrationBuilder.DeleteData(
                table: "City",
                keyColumn: "Id",
                keyValue: 100);

            migrationBuilder.DeleteData(
                table: "City",
                keyColumn: "Id",
                keyValue: 102);

            migrationBuilder.DeleteData(
                table: "City",
                keyColumn: "Id",
                keyValue: 103);

            migrationBuilder.DeleteData(
                table: "City",
                keyColumn: "Id",
                keyValue: 105);

            migrationBuilder.DeleteData(
                table: "City",
                keyColumn: "Id",
                keyValue: 106);

            migrationBuilder.DeleteData(
                table: "City",
                keyColumn: "Id",
                keyValue: 107);

            migrationBuilder.DeleteData(
                table: "City",
                keyColumn: "Id",
                keyValue: 108);

            migrationBuilder.DeleteData(
                table: "City",
                keyColumn: "Id",
                keyValue: 109);

            migrationBuilder.DeleteData(
                table: "City",
                keyColumn: "Id",
                keyValue: 110);

            migrationBuilder.DeleteData(
                table: "City",
                keyColumn: "Id",
                keyValue: 111);

            migrationBuilder.DeleteData(
                table: "City",
                keyColumn: "Id",
                keyValue: 112);

            migrationBuilder.DeleteData(
                table: "City",
                keyColumn: "Id",
                keyValue: 113);

            migrationBuilder.DeleteData(
                table: "City",
                keyColumn: "Id",
                keyValue: 114);

            migrationBuilder.DeleteData(
                table: "City",
                keyColumn: "Id",
                keyValue: 115);

            migrationBuilder.DeleteData(
                table: "City",
                keyColumn: "Id",
                keyValue: 116);

            migrationBuilder.DeleteData(
                table: "City",
                keyColumn: "Id",
                keyValue: 117);

            migrationBuilder.DeleteData(
                table: "City",
                keyColumn: "Id",
                keyValue: 118);

            migrationBuilder.DeleteData(
                table: "City",
                keyColumn: "Id",
                keyValue: 119);

            migrationBuilder.DeleteData(
                table: "City",
                keyColumn: "Id",
                keyValue: 120);

            migrationBuilder.DeleteData(
                table: "City",
                keyColumn: "Id",
                keyValue: 121);

            migrationBuilder.DeleteData(
                table: "City",
                keyColumn: "Id",
                keyValue: 122);

            migrationBuilder.DeleteData(
                table: "City",
                keyColumn: "Id",
                keyValue: 123);

            migrationBuilder.DeleteData(
                table: "City",
                keyColumn: "Id",
                keyValue: 124);

            migrationBuilder.DeleteData(
                table: "Client",
                keyColumn: "Id",
                keyValue: new Guid("0523fca9-d9bd-4e14-98f5-aa73c208a274"));

            migrationBuilder.DeleteData(
                table: "Client",
                keyColumn: "Id",
                keyValue: new Guid("157d8713-1176-4050-b53f-49e38670337a"));

            migrationBuilder.DeleteData(
                table: "Client",
                keyColumn: "Id",
                keyValue: new Guid("1eb9eee6-d946-4111-a2e6-a3a67445ad97"));

            migrationBuilder.DeleteData(
                table: "Client",
                keyColumn: "Id",
                keyValue: new Guid("20affdb3-aa89-4387-d057-08dc9dc24d12"));

            migrationBuilder.DeleteData(
                table: "Client",
                keyColumn: "Id",
                keyValue: new Guid("52c17ce6-c3c2-49f8-246f-08dc9dc44f56"));

            migrationBuilder.DeleteData(
                table: "Client",
                keyColumn: "Id",
                keyValue: new Guid("534322c0-4ea4-488a-946c-e67d044ba637"));

            migrationBuilder.DeleteData(
                table: "Client",
                keyColumn: "Id",
                keyValue: new Guid("5780ee88-d758-4ead-861a-cd19e001bf3d"));

            migrationBuilder.DeleteData(
                table: "Client",
                keyColumn: "Id",
                keyValue: new Guid("6240d8f1-b80d-49e3-77d0-08dc8003d380"));

            migrationBuilder.DeleteData(
                table: "Client",
                keyColumn: "Id",
                keyValue: new Guid("67be2123-c0ad-4b94-a4c3-cbfe002a7313"));

            migrationBuilder.DeleteData(
                table: "Client",
                keyColumn: "Id",
                keyValue: new Guid("7f5734f6-d8e6-4d66-82d4-f1f7da73a6d3"));

            migrationBuilder.DeleteData(
                table: "Client",
                keyColumn: "Id",
                keyValue: new Guid("8fd6aa7e-2afa-402b-a05c-541ab2ddbc5a"));

            migrationBuilder.DeleteData(
                table: "Client",
                keyColumn: "Id",
                keyValue: new Guid("c2f846f6-c807-404b-a803-9f1e26e1bdd2"));

            migrationBuilder.DeleteData(
                table: "Client",
                keyColumn: "Id",
                keyValue: new Guid("e61d7eee-9996-4dd5-b987-37f10bddb38c"));

            migrationBuilder.DeleteData(
                table: "Client",
                keyColumn: "Id",
                keyValue: new Guid("fd72d7b7-a9e8-4740-b370-e3a2a33d61df"));

            migrationBuilder.DeleteData(
                table: "Client",
                keyColumn: "Id",
                keyValue: new Guid("fda36cb7-4616-48e9-cf2a-08dc728077ad"));

            migrationBuilder.DeleteData(
                table: "Employee",
                keyColumn: "Id",
                keyValue: new Guid("027d20a4-dcc3-4aee-a315-08dcc51a8157"));

            migrationBuilder.DeleteData(
                table: "Employee",
                keyColumn: "Id",
                keyValue: new Guid("0403e6d4-7cfe-4f25-a313-08dcc51a8157"));

            migrationBuilder.DeleteData(
                table: "Employee",
                keyColumn: "Id",
                keyValue: new Guid("16cda006-dc9c-4071-012e-08dc81a7b237"));

            migrationBuilder.DeleteData(
                table: "Employee",
                keyColumn: "Id",
                keyValue: new Guid("755fc934-044c-40e2-a316-08dcc51a8157"));

            migrationBuilder.DeleteData(
                table: "Employee",
                keyColumn: "Id",
                keyValue: new Guid("8b778d69-5d48-4690-8d8a-945c7fb250b1"));

            migrationBuilder.DeleteData(
                table: "Employee",
                keyColumn: "Id",
                keyValue: new Guid("9ec6edeb-02b6-4c4f-139d-08dc2fe46497"));

            migrationBuilder.DeleteData(
                table: "Employee",
                keyColumn: "Id",
                keyValue: new Guid("d0903cb4-e560-4547-37b0-08dc95e40210"));

            migrationBuilder.DeleteData(
                table: "Employee",
                keyColumn: "Id",
                keyValue: new Guid("e9605352-c3f4-4568-a312-08dcc51a8157"));

            migrationBuilder.DeleteData(
                table: "Employee",
                keyColumn: "Id",
                keyValue: new Guid("f6fe658e-37f8-4a52-a314-08dcc51a8157"));

            migrationBuilder.DeleteData(
                table: "ReservationStatus",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "ReservationStatus",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "ReservationStatus",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "ReservationStatus",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "ReservationStatus",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "Agency",
                keyColumn: "Id",
                keyValue: new Guid("1b73b899-6578-4120-0caf-08dc8233593a"));

            migrationBuilder.DeleteData(
                table: "Agency",
                keyColumn: "Id",
                keyValue: new Guid("62f78fa6-4e5e-4420-a91d-08dc37b68f22"));

            migrationBuilder.DeleteData(
                table: "Agency",
                keyColumn: "Id",
                keyValue: new Guid("66f20b4e-9e6d-4c95-6a66-08dc298d67da"));

            migrationBuilder.DeleteData(
                table: "Country",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Country",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Country",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Country",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Country",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "Country",
                keyColumn: "Id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "Country",
                keyColumn: "Id",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "Country",
                keyColumn: "Id",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "Country",
                keyColumn: "Id",
                keyValue: 10);

            migrationBuilder.DeleteData(
                table: "Country",
                keyColumn: "Id",
                keyValue: 11);

            migrationBuilder.DeleteData(
                table: "Country",
                keyColumn: "Id",
                keyValue: 12);

            migrationBuilder.DeleteData(
                table: "Country",
                keyColumn: "Id",
                keyValue: 13);

            migrationBuilder.DeleteData(
                table: "Address",
                keyColumn: "Id",
                keyValue: new Guid("39c6bbd9-57a1-4b5a-3d83-08dc8233593d"));

            migrationBuilder.DeleteData(
                table: "Address",
                keyColumn: "Id",
                keyValue: new Guid("462e18e6-5009-43b5-15be-08dc298c5934"));

            migrationBuilder.DeleteData(
                table: "Address",
                keyColumn: "Id",
                keyValue: new Guid("8af7c2da-c52a-4322-b2d7-08dc37b68f24"));

            migrationBuilder.DeleteData(
                table: "City",
                keyColumn: "Id",
                keyValue: 101);

            migrationBuilder.DeleteData(
                table: "City",
                keyColumn: "Id",
                keyValue: 104);

            migrationBuilder.DeleteData(
                table: "Country",
                keyColumn: "Id",
                keyValue: 1);
        }
    }
}
