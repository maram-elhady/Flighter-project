using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Flighter.Migrations
{
    /// <inheritdoc />
    public partial class ticketCompenyRelation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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

            migrationBuilder.AddColumn<int>(
                name: "CompanyId",
                table: "Users",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "CompanyId",
                table: "tickets",
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

            migrationBuilder.CreateIndex(
                name: "IX_tickets_CompanyId",
                table: "tickets",
                column: "CompanyId");

            migrationBuilder.AddForeignKey(
                name: "FK_tickets_Companies_CompanyId",
                table: "tickets",
                column: "CompanyId",
                principalTable: "Companies",
                principalColumn: "CompanyId",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_tickets_Companies_CompanyId",
                table: "tickets");

            migrationBuilder.DropIndex(
                name: "IX_tickets_CompanyId",
                table: "tickets");

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

            migrationBuilder.DropColumn(
                name: "CompanyId",
                table: "tickets");

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
    }
}
