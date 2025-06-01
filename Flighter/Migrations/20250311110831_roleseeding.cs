using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Flighter.Migrations
{
    /// <inheritdoc />
    public partial class roleseeding : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: "3be15aff-8c51-4bcf-a3ff-37f6f1678f0b");

            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: "a984a9e5-4d98-4ea9-b3fd-a6b960c19412");

            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: "b1719902-b611-427a-8233-3c9025a72677");

            migrationBuilder.InsertData(
                table: "Roles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "9a2d1c25-4d5b-42b1-846f-0f74813b3c57", null, "Owner", "OWNER" },
                    { "bbd6f3b8-3fd3-47a9-b1f7-9eaf1c8b4478", null, "Company", "COMPANY" },
                    { "cbd6d8e2-2fe8-47b2-a5dc-0d5a12f98e45", null, "User", "USER" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: "9a2d1c25-4d5b-42b1-846f-0f74813b3c57");

            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: "bbd6f3b8-3fd3-47a9-b1f7-9eaf1c8b4478");

            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: "cbd6d8e2-2fe8-47b2-a5dc-0d5a12f98e45");

            migrationBuilder.InsertData(
                table: "Roles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "3be15aff-8c51-4bcf-a3ff-37f6f1678f0b", "a2f2b045-736d-4f6d-9617-c0e2a86b44ae", "Company", "COMPANY" },
                    { "a984a9e5-4d98-4ea9-b3fd-a6b960c19412", "7b524207-7c63-46f6-83a8-cf1dcf2e88d6", "User", "USER" },
                    { "b1719902-b611-427a-8233-3c9025a72677", "6a9fb18d-6413-47c9-8eb8-19563b23bbf6", "Owner", "OWNER" }
                });
        }
    }
}
