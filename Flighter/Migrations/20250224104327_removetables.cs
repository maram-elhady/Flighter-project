using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Flighter.Migrations
{
    /// <inheritdoc />
    public partial class removetables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_tickets_Users_userId",
                table: "tickets");

            migrationBuilder.DropTable(
                name: "AirlineAdmin");

            migrationBuilder.DropTable(
                name: "offers");

            migrationBuilder.DropTable(
                name: "Payment");

            migrationBuilder.DropTable(
                name: "Airline");

            migrationBuilder.DropIndex(
                name: "IX_tickets_userId",
                table: "tickets");

            migrationBuilder.DropColumn(
                name: "userId",
                table: "tickets");

            migrationBuilder.DropColumn(
                name: "arrivalTime",
                table: "schedules");

            migrationBuilder.DropColumn(
                name: "availableSeats",
                table: "schedules");

            migrationBuilder.DropColumn(
                name: "departureTime",
                table: "schedules");

            migrationBuilder.DropColumn(
                name: "ReturnDate",
                table: "flights");

            migrationBuilder.DropColumn(
                name: "departureDate",
                table: "flights");

            migrationBuilder.DropColumn(
                name: "distance",
                table: "flights");

            migrationBuilder.DropColumn(
                name: "flightType",
                table: "flights");

            migrationBuilder.DropColumn(
                name: "classFactor",
                table: "classTypes");

            migrationBuilder.CreateTable(
                name: "payments",
                columns: table => new
                {
                    PaymentId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    userId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    cardNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    cardHolderName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    expiryDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    paymentDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    paymentMethod = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_payments", x => x.PaymentId);
                    table.ForeignKey(
                        name: "FK_payments_Users_userId",
                        column: x => x.userId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_payments_userId",
                table: "payments",
                column: "userId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "payments");

            migrationBuilder.AddColumn<string>(
                name: "userId",
                table: "tickets",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "arrivalTime",
                table: "schedules",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "availableSeats",
                table: "schedules",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "departureTime",
                table: "schedules",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "ReturnDate",
                table: "flights",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "departureDate",
                table: "flights",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<double>(
                name: "distance",
                table: "flights",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<string>(
                name: "flightType",
                table: "flights",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<double>(
                name: "classFactor",
                table: "classTypes",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.CreateTable(
                name: "Airline",
                columns: table => new
                {
                    airlineId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    airlineName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    basePrice = table.Column<double>(type: "float", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Airline", x => x.airlineId);
                });

            migrationBuilder.CreateTable(
                name: "Payment",
                columns: table => new
                {
                    PaymentId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    userId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    cardHolderName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    cardNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    expiryDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    paymentDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    paymentMethod = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Payment", x => x.PaymentId);
                    table.ForeignKey(
                        name: "FK_Payment_Users_userId",
                        column: x => x.userId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "AirlineAdmin",
                columns: table => new
                {
                    AirlineId = table.Column<int>(type: "int", nullable: false),
                    AdminId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AirlineAdmin", x => new { x.AirlineId, x.AdminId });
                    table.ForeignKey(
                        name: "FK_AirlineAdmin_Airline_AirlineId",
                        column: x => x.AirlineId,
                        principalTable: "Airline",
                        principalColumn: "airlineId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AirlineAdmin_Users_AdminId",
                        column: x => x.AdminId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "offers",
                columns: table => new
                {
                    OfferId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    airlineId = table.Column<int>(type: "int", nullable: false),
                    offerDescription = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    validFrom = table.Column<DateTime>(type: "datetime2", nullable: false),
                    validTo = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_offers", x => x.OfferId);
                    table.ForeignKey(
                        name: "FK_offers_Airline_airlineId",
                        column: x => x.airlineId,
                        principalTable: "Airline",
                        principalColumn: "airlineId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_tickets_userId",
                table: "tickets",
                column: "userId");

            migrationBuilder.CreateIndex(
                name: "IX_AirlineAdmin_AdminId",
                table: "AirlineAdmin",
                column: "AdminId");

            migrationBuilder.CreateIndex(
                name: "IX_offers_airlineId",
                table: "offers",
                column: "airlineId");

            migrationBuilder.CreateIndex(
                name: "IX_Payment_userId",
                table: "Payment",
                column: "userId");

            migrationBuilder.AddForeignKey(
                name: "FK_tickets_Users_userId",
                table: "tickets",
                column: "userId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
