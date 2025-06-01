using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Flighter.Migrations
{
    /// <inheritdoc />
    public partial class seatidentity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "Flightseats",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Flightseats_UserId",
                table: "Flightseats",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Flightseats_Users_UserId",
                table: "Flightseats",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Flightseats_Users_UserId",
                table: "Flightseats");

            migrationBuilder.DropIndex(
                name: "IX_Flightseats_UserId",
                table: "Flightseats");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Flightseats");
        }
    }
}
