using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Flighter.Migrations
{
    /// <inheritdoc />
    public partial class AddCompaniesToRoles : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Roles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "38ef3ccf-1905-4148-bee2-992da9feabbf", "976c5341-d999-4d09-9245-5eee48c1e1c6", "Kuwait Airline", "KUWAIT AIRLINE" },
                    { "7c884f76-a248-4d39-8e2d-4c40adc034a7", "2759d044-a06e-4e5f-a22d-37d5f8cab51f", "Egypt Air", "EGYPT AIR" },
                    { "95e5e6d3-09bc-4ec1-aebf-f1688b6e9c70", "db508387-0914-4c86-85c0-4a9af0dce39e", "FlighterOwner", "FLIGHTEROWNER" },
                    { "ab313c25-c64d-49fa-b421-c233c790f130", "70b0d5a8-5ddb-430f-a136-8e6fa4d53ddb", "Emirates", "EMIRATES" },
                    { "d3497da1-a530-4528-8bbe-2ef2af921a02", "d4d6ea26-6915-4517-a95b-a2f4ad340b7e", "User", "USER" },
                    { "d68227f0-76d1-4ef5-b570-e8bbe880c0a2", "44d9214c-6d58-4742-9091-3489257419c6", "Qatar Airways", "QATAR AIRWAYS" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: "38ef3ccf-1905-4148-bee2-992da9feabbf");

            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: "7c884f76-a248-4d39-8e2d-4c40adc034a7");

            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: "95e5e6d3-09bc-4ec1-aebf-f1688b6e9c70");

            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: "ab313c25-c64d-49fa-b421-c233c790f130");

            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: "d3497da1-a530-4528-8bbe-2ef2af921a02");

            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: "d68227f0-76d1-4ef5-b570-e8bbe880c0a2");
        }
    }
}
