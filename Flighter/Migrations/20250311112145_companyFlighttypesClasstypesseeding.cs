using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Flighter.Migrations
{
    /// <inheritdoc />
    public partial class companyFlighttypesClasstypesseeding : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Companies",
                columns: new[] { "CompanyId", "CompanyName" },
                values: new object[,]
                {
                    { 1, "Egypt Air" },
                    { 2, "Qatar Airways" },
                    { 3, "Emirates" },
                    { 4, "Kuwait Airline" }
                });

            migrationBuilder.InsertData(
                table: "FlightTypes",
                columns: new[] { "FlightTypeId", "FlightName" },
                values: new object[,]
                {
                    { 1, "Direct" },
                    { 2, "Round" }
                });

            migrationBuilder.InsertData(
                table: "classTypes",
                columns: new[] { "classTypeId", "className" },
                values: new object[,]
                {
                    { 1, "First class" },
                    { 2, "Economy" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Companies",
                keyColumn: "CompanyId",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Companies",
                keyColumn: "CompanyId",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Companies",
                keyColumn: "CompanyId",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Companies",
                keyColumn: "CompanyId",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "FlightTypes",
                keyColumn: "FlightTypeId",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "FlightTypes",
                keyColumn: "FlightTypeId",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "classTypes",
                keyColumn: "classTypeId",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "classTypes",
                keyColumn: "classTypeId",
                keyValue: 2);
        }
    }
}
