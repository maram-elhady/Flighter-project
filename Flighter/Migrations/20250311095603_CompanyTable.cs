using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Flighter.Migrations
{
    /// <inheritdoc />
    public partial class CompanyTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
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

            migrationBuilder.CreateTable(
                name: "Companies",
                columns: table => new
                {
                    CompanyId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CompanyName = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Companies", x => x.CompanyId);
                });

            migrationBuilder.InsertData(
                table: "Roles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "6c7fcd73-6743-45f7-87e5-5087c032f1af", "cf4a29c5-d1fc-42e1-a82f-456b23508d29", "Owner", "OWNER" },
                    { "817e8d50-26e5-4139-8dc4-84d2be0597be", "f61d8483-b12f-467b-b413-7e562c2cc774", "Company", "COMPANY" },
                    { "bf1c8a8d-8ec8-4df5-a49b-edc58c89dee6", "5c8f6ade-a43e-4912-9b27-b5fc0d3e2b1d", "User", "USER" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Companies");

            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: "6c7fcd73-6743-45f7-87e5-5087c032f1af");

            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: "817e8d50-26e5-4139-8dc4-84d2be0597be");

            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: "bf1c8a8d-8ec8-4df5-a49b-edc58c89dee6");

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
    }
}
