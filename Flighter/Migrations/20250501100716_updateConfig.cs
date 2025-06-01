using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Flighter.Migrations
{
    /// <inheritdoc />
    public partial class updateConfig : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_bookings_Users_userId",
                table: "bookings");

            migrationBuilder.DropForeignKey(
                name: "FK_bookings_tickets_ticketId",
                table: "bookings");

            migrationBuilder.DropForeignKey(
                name: "FK_flights_Companies_CompanyId",
                table: "flights");

            migrationBuilder.DropForeignKey(
                name: "FK_Flightseats_tickets_ticketId",
                table: "Flightseats");

            migrationBuilder.DropForeignKey(
                name: "FK_schedules_flights_flightId",
                table: "schedules");

            migrationBuilder.DropForeignKey(
                name: "FK_tickets_Companies_CompanyId",
                table: "tickets");

            migrationBuilder.DropForeignKey(
                name: "FK_tickets_Users_userId",
                table: "tickets");

            migrationBuilder.DropForeignKey(
                name: "FK_tickets_flights_flightId",
                table: "tickets");

            migrationBuilder.DropForeignKey(
                name: "FK_tickets_schedules_scheduleId",
                table: "tickets");

            migrationBuilder.DropForeignKey(
                name: "FK_Users_Companies_CompanyId",
                table: "Users");

            migrationBuilder.AddForeignKey(
                name: "FK_bookings_Users_userId",
                table: "bookings",
                column: "userId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_bookings_tickets_ticketId",
                table: "bookings",
                column: "ticketId",
                principalTable: "tickets",
                principalColumn: "TicketId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_flights_Companies_CompanyId",
                table: "flights",
                column: "CompanyId",
                principalTable: "Companies",
                principalColumn: "CompanyId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Flightseats_tickets_ticketId",
                table: "Flightseats",
                column: "ticketId",
                principalTable: "tickets",
                principalColumn: "TicketId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_schedules_flights_flightId",
                table: "schedules",
                column: "flightId",
                principalTable: "flights",
                principalColumn: "FlightId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_tickets_Companies_CompanyId",
                table: "tickets",
                column: "CompanyId",
                principalTable: "Companies",
                principalColumn: "CompanyId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_tickets_Users_userId",
                table: "tickets",
                column: "userId",
                principalTable: "Users",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_tickets_flights_flightId",
                table: "tickets",
                column: "flightId",
                principalTable: "flights",
                principalColumn: "FlightId");

            migrationBuilder.AddForeignKey(
                name: "FK_tickets_schedules_scheduleId",
                table: "tickets",
                column: "scheduleId",
                principalTable: "schedules",
                principalColumn: "ScheduleId");

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Companies_CompanyId",
                table: "Users",
                column: "CompanyId",
                principalTable: "Companies",
                principalColumn: "CompanyId",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_bookings_Users_userId",
                table: "bookings");

            migrationBuilder.DropForeignKey(
                name: "FK_bookings_tickets_ticketId",
                table: "bookings");

            migrationBuilder.DropForeignKey(
                name: "FK_flights_Companies_CompanyId",
                table: "flights");

            migrationBuilder.DropForeignKey(
                name: "FK_Flightseats_tickets_ticketId",
                table: "Flightseats");

            migrationBuilder.DropForeignKey(
                name: "FK_schedules_flights_flightId",
                table: "schedules");

            migrationBuilder.DropForeignKey(
                name: "FK_tickets_Companies_CompanyId",
                table: "tickets");

            migrationBuilder.DropForeignKey(
                name: "FK_tickets_Users_userId",
                table: "tickets");

            migrationBuilder.DropForeignKey(
                name: "FK_tickets_flights_flightId",
                table: "tickets");

            migrationBuilder.DropForeignKey(
                name: "FK_tickets_schedules_scheduleId",
                table: "tickets");

            migrationBuilder.DropForeignKey(
                name: "FK_Users_Companies_CompanyId",
                table: "Users");

            migrationBuilder.AddForeignKey(
                name: "FK_bookings_Users_userId",
                table: "bookings",
                column: "userId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_bookings_tickets_ticketId",
                table: "bookings",
                column: "ticketId",
                principalTable: "tickets",
                principalColumn: "TicketId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_flights_Companies_CompanyId",
                table: "flights",
                column: "CompanyId",
                principalTable: "Companies",
                principalColumn: "CompanyId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Flightseats_tickets_ticketId",
                table: "Flightseats",
                column: "ticketId",
                principalTable: "tickets",
                principalColumn: "TicketId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_schedules_flights_flightId",
                table: "schedules",
                column: "flightId",
                principalTable: "flights",
                principalColumn: "FlightId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_tickets_Companies_CompanyId",
                table: "tickets",
                column: "CompanyId",
                principalTable: "Companies",
                principalColumn: "CompanyId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_tickets_Users_userId",
                table: "tickets",
                column: "userId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_tickets_flights_flightId",
                table: "tickets",
                column: "flightId",
                principalTable: "flights",
                principalColumn: "FlightId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_tickets_schedules_scheduleId",
                table: "tickets",
                column: "scheduleId",
                principalTable: "schedules",
                principalColumn: "ScheduleId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Companies_CompanyId",
                table: "Users",
                column: "CompanyId",
                principalTable: "Companies",
                principalColumn: "CompanyId",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
