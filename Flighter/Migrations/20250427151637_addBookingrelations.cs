using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Flighter.Migrations
{
    /// <inheritdoc />
    public partial class addBookingrelations : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_bookings_tickets_ticketId",
                table: "bookings");

            migrationBuilder.AlterColumn<string>(
                name: "userId",
                table: "bookings",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.CreateIndex(
                name: "IX_payments_BookingId",
                table: "payments",
                column: "BookingId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_bookings_userId",
                table: "bookings",
                column: "userId");

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
                name: "FK_payments_bookings_BookingId",
                table: "payments",
                column: "BookingId",
                principalTable: "bookings",
                principalColumn: "BookingId",
                onDelete: ReferentialAction.Restrict);
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
                name: "FK_payments_bookings_BookingId",
                table: "payments");

            migrationBuilder.DropIndex(
                name: "IX_payments_BookingId",
                table: "payments");

            migrationBuilder.DropIndex(
                name: "IX_bookings_userId",
                table: "bookings");

            migrationBuilder.AlterColumn<string>(
                name: "userId",
                table: "bookings",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddForeignKey(
                name: "FK_bookings_tickets_ticketId",
                table: "bookings",
                column: "ticketId",
                principalTable: "tickets",
                principalColumn: "TicketId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
