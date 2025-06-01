using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Flighter.Migrations
{
    /// <inheritdoc />
    public partial class removeFlightSeat : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Flightseats_flights_flightId",
                table: "Flightseats");

            migrationBuilder.DropIndex(
                name: "IX_Flightseats_flightId",
                table: "Flightseats");

            migrationBuilder.DropColumn(
                name: "flightId",
                table: "Flightseats");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "flightId",
                table: "Flightseats",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Flightseats_flightId",
                table: "Flightseats",
                column: "flightId");

            migrationBuilder.AddForeignKey(
                name: "FK_Flightseats_flights_flightId",
                table: "Flightseats",
                column: "flightId",
                principalTable: "flights",
                principalColumn: "FlightId",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
