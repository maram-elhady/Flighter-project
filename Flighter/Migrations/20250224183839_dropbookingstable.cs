using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Flighter.Migrations
{
    /// <inheritdoc />
    public partial class dropbookingstable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BookingModel");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "BookingModel",
                columns: table => new
                {
                    BookingId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PaymentId = table.Column<int>(type: "int", nullable: false),
                    ticketId = table.Column<int>(type: "int", nullable: false),
                    userId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    bookingDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    paymentStatus = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BookingModel", x => x.BookingId);
                    table.ForeignKey(
                        name: "FK_BookingModel_Users_userId",
                        column: x => x.userId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BookingModel_payments_PaymentId",
                        column: x => x.PaymentId,
                        principalTable: "payments",
                        principalColumn: "PaymentId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BookingModel_tickets_ticketId",
                        column: x => x.ticketId,
                        principalTable: "tickets",
                        principalColumn: "TicketId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BookingModel_PaymentId",
                table: "BookingModel",
                column: "PaymentId");

            migrationBuilder.CreateIndex(
                name: "IX_BookingModel_ticketId",
                table: "BookingModel",
                column: "ticketId");

            migrationBuilder.CreateIndex(
                name: "IX_BookingModel_userId",
                table: "BookingModel",
                column: "userId");
        }
    }
}
