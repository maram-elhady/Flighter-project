using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Flighter.Migrations
{
    /// <inheritdoc />
    public partial class erUpdates16 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CompanyId",
                table: "flights",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_flights_CompanyId",
                table: "flights",
                column: "CompanyId");

            migrationBuilder.AddForeignKey(
                name: "FK_flights_Companies_CompanyId",
                table: "flights",
                column: "CompanyId",
                principalTable: "Companies",
                principalColumn: "CompanyId",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_flights_Companies_CompanyId",
                table: "flights");

            migrationBuilder.DropIndex(
                name: "IX_flights_CompanyId",
                table: "flights");

            migrationBuilder.DropColumn(
                name: "CompanyId",
                table: "flights");
        }
    }
}
