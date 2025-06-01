using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Flighter.Migrations
{
    /// <inheritdoc />
    public partial class lastmig : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_bookings_tickets_ticketId",
                table: "bookings");

            migrationBuilder.DropPrimaryKey(
                name: "PK_bookings",
                table: "bookings");

            migrationBuilder.RenameTable(
                name: "bookings",
                newName: "BookingModel");

            migrationBuilder.RenameIndex(
                name: "IX_bookings_ticketId",
                table: "BookingModel",
                newName: "IX_BookingModel_ticketId");

            migrationBuilder.AlterColumn<string>(
                name: "flightNumber",
                table: "tickets",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<int>(
                name: "availableSeats",
                table: "tickets",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "isOffer",
                table: "tickets",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "totalSeats",
                table: "tickets",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateOnly>(
                name: "arrivalDate",
                table: "schedules",
                type: "date",
                nullable: false,
                defaultValue: new DateOnly(1, 1, 1));

            migrationBuilder.AddColumn<TimeOnly>(
                name: "arrivalTime",
                table: "schedules",
                type: "time",
                nullable: false,
                defaultValue: new TimeOnly(0, 0, 0));

            migrationBuilder.AddColumn<DateOnly>(
                name: "departureDate",
                table: "schedules",
                type: "date",
                nullable: false,
                defaultValue: new DateOnly(1, 1, 1));

            migrationBuilder.AddColumn<TimeOnly>(
                name: "departureTime",
                table: "schedules",
                type: "time",
                nullable: false,
                defaultValue: new TimeOnly(0, 0, 0));

            migrationBuilder.AddColumn<int>(
                name: "flightId",
                table: "schedules",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateOnly>(
                name: "returnDate",
                table: "schedules",
                type: "date",
                nullable: true);

            migrationBuilder.AddColumn<TimeOnly>(
                name: "returnTime",
                table: "schedules",
                type: "time",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "bookingId",
                table: "payments",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "FlightTypeId",
                table: "flights",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "PaymentId",
                table: "BookingModel",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "userId",
                table: "BookingModel",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddPrimaryKey(
                name: "PK_BookingModel",
                table: "BookingModel",
                column: "BookingId");

            migrationBuilder.CreateTable(
                name: "Flightseats",
                columns: table => new
                {
                    SeatId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    flightId = table.Column<int>(type: "int", nullable: false),
                    SeatName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    isBooked = table.Column<bool>(type: "bit", nullable: false),
                    ticketId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Flightseats", x => x.SeatId);
                    table.ForeignKey(
                        name: "FK_Flightseats_flights_flightId",
                        column: x => x.flightId,
                        principalTable: "flights",
                        principalColumn: "FlightId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Flightseats_tickets_ticketId",
                        column: x => x.ticketId,
                        principalTable: "tickets",
                        principalColumn: "TicketId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "FlightTypes",
                columns: table => new
                {
                    FlightTypeId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FlightName = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FlightTypes", x => x.FlightTypeId);
                });

            migrationBuilder.CreateIndex(
                name: "IX_schedules_flightId",
                table: "schedules",
                column: "flightId");

            migrationBuilder.CreateIndex(
                name: "IX_flights_FlightTypeId",
                table: "flights",
                column: "FlightTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_BookingModel_PaymentId",
                table: "BookingModel",
                column: "PaymentId");

            migrationBuilder.CreateIndex(
                name: "IX_BookingModel_userId",
                table: "BookingModel",
                column: "userId");

            migrationBuilder.CreateIndex(
                name: "IX_Flightseats_flightId",
                table: "Flightseats",
                column: "flightId");

            migrationBuilder.CreateIndex(
                name: "IX_Flightseats_ticketId",
                table: "Flightseats",
                column: "ticketId");

            migrationBuilder.AddForeignKey(
                name: "FK_BookingModel_Users_userId",
                table: "BookingModel",
                column: "userId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_BookingModel_payments_PaymentId",
                table: "BookingModel",
                column: "PaymentId",
                principalTable: "payments",
                principalColumn: "PaymentId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_BookingModel_tickets_ticketId",
                table: "BookingModel",
                column: "ticketId",
                principalTable: "tickets",
                principalColumn: "TicketId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_flights_FlightTypes_FlightTypeId",
                table: "flights",
                column: "FlightTypeId",
                principalTable: "FlightTypes",
                principalColumn: "FlightTypeId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_schedules_flights_flightId",
                table: "schedules",
                column: "flightId",
                principalTable: "flights",
                principalColumn: "FlightId",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BookingModel_Users_userId",
                table: "BookingModel");

            migrationBuilder.DropForeignKey(
                name: "FK_BookingModel_payments_PaymentId",
                table: "BookingModel");

            migrationBuilder.DropForeignKey(
                name: "FK_BookingModel_tickets_ticketId",
                table: "BookingModel");

            migrationBuilder.DropForeignKey(
                name: "FK_flights_FlightTypes_FlightTypeId",
                table: "flights");

            migrationBuilder.DropForeignKey(
                name: "FK_schedules_flights_flightId",
                table: "schedules");

            migrationBuilder.DropTable(
                name: "Flightseats");

            migrationBuilder.DropTable(
                name: "FlightTypes");

            migrationBuilder.DropIndex(
                name: "IX_schedules_flightId",
                table: "schedules");

            migrationBuilder.DropIndex(
                name: "IX_flights_FlightTypeId",
                table: "flights");

            migrationBuilder.DropPrimaryKey(
                name: "PK_BookingModel",
                table: "BookingModel");

            migrationBuilder.DropIndex(
                name: "IX_BookingModel_PaymentId",
                table: "BookingModel");

            migrationBuilder.DropIndex(
                name: "IX_BookingModel_userId",
                table: "BookingModel");

            migrationBuilder.DropColumn(
                name: "availableSeats",
                table: "tickets");

            migrationBuilder.DropColumn(
                name: "isOffer",
                table: "tickets");

            migrationBuilder.DropColumn(
                name: "totalSeats",
                table: "tickets");

            migrationBuilder.DropColumn(
                name: "arrivalDate",
                table: "schedules");

            migrationBuilder.DropColumn(
                name: "arrivalTime",
                table: "schedules");

            migrationBuilder.DropColumn(
                name: "departureDate",
                table: "schedules");

            migrationBuilder.DropColumn(
                name: "departureTime",
                table: "schedules");

            migrationBuilder.DropColumn(
                name: "flightId",
                table: "schedules");

            migrationBuilder.DropColumn(
                name: "returnDate",
                table: "schedules");

            migrationBuilder.DropColumn(
                name: "returnTime",
                table: "schedules");

            migrationBuilder.DropColumn(
                name: "bookingId",
                table: "payments");

            migrationBuilder.DropColumn(
                name: "FlightTypeId",
                table: "flights");

            migrationBuilder.DropColumn(
                name: "PaymentId",
                table: "BookingModel");

            migrationBuilder.DropColumn(
                name: "userId",
                table: "BookingModel");

            migrationBuilder.RenameTable(
                name: "BookingModel",
                newName: "bookings");

            migrationBuilder.RenameIndex(
                name: "IX_BookingModel_ticketId",
                table: "bookings",
                newName: "IX_bookings_ticketId");

            migrationBuilder.AlterColumn<int>(
                name: "flightNumber",
                table: "tickets",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddPrimaryKey(
                name: "PK_bookings",
                table: "bookings",
                column: "BookingId");

            migrationBuilder.AddForeignKey(
                name: "FK_bookings_tickets_ticketId",
                table: "bookings",
                column: "ticketId",
                principalTable: "tickets",
                principalColumn: "TicketId",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
