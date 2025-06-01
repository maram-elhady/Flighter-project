using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Flighter.Migrations
{
    /// <inheritdoc />
    public partial class useradminrelation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: "998cbca8-399c-465e-889a-54a05f40b901");

            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: "e6288e0d-f807-4a55-b9dd-5f91baded748");

            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: "e9cf7528-3d16-442a-9221-87bcc5c08bf4");

            migrationBuilder.AddColumn<string>(
                name: "userId",
                table: "tickets",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.InsertData(
                table: "Roles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "3be15aff-8c51-4bcf-a3ff-37f6f1678f0b", "a2f2b045-736d-4f6d-9617-c0e2a86b44ae", "Company", "COMPANY" },
                    { "a984a9e5-4d98-4ea9-b3fd-a6b960c19412", "7b524207-7c63-46f6-83a8-cf1dcf2e88d6", "User", "USER" },
                    { "b1719902-b611-427a-8233-3c9025a72677", "6a9fb18d-6413-47c9-8eb8-19563b23bbf6", "Owner", "OWNER" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_tickets_userId",
                table: "tickets",
                column: "userId");

            migrationBuilder.AddForeignKey(
                name: "FK_tickets_Users_userId",
                table: "tickets",
                column: "userId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_tickets_Users_userId",
                table: "tickets");

            migrationBuilder.DropIndex(
                name: "IX_tickets_userId",
                table: "tickets");

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

            migrationBuilder.DropColumn(
                name: "userId",
                table: "tickets");

            migrationBuilder.InsertData(
                table: "Roles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "998cbca8-399c-465e-889a-54a05f40b901", "f5a6ff59-57cc-4c3b-8ace-d18bba87b6ec", "Owner", "OWNER" },
                    { "e6288e0d-f807-4a55-b9dd-5f91baded748", "1bbcd270-0df2-4033-8243-6cdba30a69d5", "User", "USER" },
                    { "e9cf7528-3d16-442a-9221-87bcc5c08bf4", "43d8dfbd-7e5f-4c62-a9d0-8a3dc55ef3da", "Company", "COMPANY" }
                });
        }
    }
}
