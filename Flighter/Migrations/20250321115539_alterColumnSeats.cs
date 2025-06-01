using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Flighter.Migrations
{
    /// <inheritdoc />
    public partial class alterColumnSeats : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "classTypes",
                keyColumn: "classTypeId",
                keyValue: 1,
                column: "className",
                value: "Business");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "classTypes",
                keyColumn: "classTypeId",
                keyValue: 1,
                column: "className",
                value: "First class");
        }
    }
}
