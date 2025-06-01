using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Flighter.Migrations
{
    /// <inheritdoc />
    public partial class addcompanyidtable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: "238f17a9-a260-4fce-b1bf-457e6eb0d69c");

            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: "6047a7c1-aae8-4e39-8630-639a04e342fb");

            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: "f31f0d5a-7b5d-49e3-af6f-e610270785a4");

            migrationBuilder.AddColumn<int>(
                name: "CompanyId",
                table: "Users",
                type: "int",
                nullable: true);

            migrationBuilder.InsertData(
                table: "Roles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "998cbca8-399c-465e-889a-54a05f40b901", "f5a6ff59-57cc-4c3b-8ace-d18bba87b6ec", "Owner", "OWNER" },
                    { "e6288e0d-f807-4a55-b9dd-5f91baded748", "1bbcd270-0df2-4033-8243-6cdba30a69d5", "User", "USER" },
                    { "e9cf7528-3d16-442a-9221-87bcc5c08bf4", "43d8dfbd-7e5f-4c62-a9d0-8a3dc55ef3da", "Company", "COMPANY" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Users_CompanyId",
                table: "Users",
                column: "CompanyId");

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Companies_CompanyId",
                table: "Users",
                column: "CompanyId",
                principalTable: "Companies",
                principalColumn: "CompanyId",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Users_Companies_CompanyId",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Users_CompanyId",
                table: "Users");

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

            migrationBuilder.DropColumn(
                name: "CompanyId",
                table: "Users");

            migrationBuilder.InsertData(
                table: "Roles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "238f17a9-a260-4fce-b1bf-457e6eb0d69c", "4a2b8135-65a2-436b-9b6a-674db105044d", "Company", "COMPANY" },
                    { "6047a7c1-aae8-4e39-8630-639a04e342fb", "83b5245f-fa26-4e58-8049-e2ac921e374c", "User", "USER" },
                    { "f31f0d5a-7b5d-49e3-af6f-e610270785a4", "fd3a5501-3e13-4855-b4df-5823f3cd79af", "Owner", "OWNER" }
                });
        }
    }
}
