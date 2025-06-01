using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Flighter.Migrations
{
    /// <inheritdoc />
    public partial class dropcompanyidtable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: "71699069-d07d-4a49-978a-3c8102ca76b6");

            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: "7e1e01c0-9c46-4b25-9d42-66d6310e728a");

            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: "94d83f35-c333-4732-941d-57c2c7c1cd7a");

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
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
                nullable: false,
                defaultValue: 0);

            migrationBuilder.InsertData(
                table: "Roles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "71699069-d07d-4a49-978a-3c8102ca76b6", "a83aa1f7-ffd0-4959-8d61-8500d7d3afec", "Company", "COMPANY" },
                    { "7e1e01c0-9c46-4b25-9d42-66d6310e728a", "618ddfd8-d5c5-4425-9baf-3155289af5ed", "Owner", "OWNER" },
                    { "94d83f35-c333-4732-941d-57c2c7c1cd7a", "c9113d82-a3bd-479f-97f4-2f81744004ec", "User", "USER" }
                });
        }
    }
}
