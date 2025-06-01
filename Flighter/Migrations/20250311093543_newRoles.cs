using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Flighter.Migrations
{
    /// <inheritdoc />
    public partial class newRoles : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Roles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "56e9eecd-d8d5-4788-ab03-0a94b1658aeb", "48e5a7e7-cc99-4704-8211-183499f53197", "Owner", "OWNER" },
                    { "bc8704a8-4e10-488a-80fd-45db56cfc100", "0efa774d-15cd-4d3a-b722-43f5f11f0d4b", "Company", "COMPANY" },
                    { "ce19fb7f-bb35-4abd-be2b-edc162fcb5d5", "982d3d80-3e54-48cd-bc9e-483867ceff12", "User", "USER" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: "56e9eecd-d8d5-4788-ab03-0a94b1658aeb");

            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: "bc8704a8-4e10-488a-80fd-45db56cfc100");

            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: "ce19fb7f-bb35-4abd-be2b-edc162fcb5d5");
        }
    }
}
