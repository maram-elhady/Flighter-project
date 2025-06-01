using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Flighter.Migrations
{
    /// <inheritdoc />
    public partial class offerPercentage : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "isOffer",
                table: "tickets");

            migrationBuilder.RenameColumn(
                name: "returnTime",
                table: "schedules",
                newName: "returnDepartureTime");

            migrationBuilder.RenameColumn(
                name: "returnDate",
                table: "schedules",
                newName: "returnDepartureDate");

            migrationBuilder.AddColumn<string>(
                name: "offer_percentage",
                table: "tickets",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateOnly>(
                name: "returnArrivalDate",
                table: "schedules",
                type: "date",
                nullable: true);

            migrationBuilder.AddColumn<TimeOnly>(
                name: "returnArrivalTime",
                table: "schedules",
                type: "time",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "offer_percentage",
                table: "tickets");

            migrationBuilder.DropColumn(
                name: "returnArrivalDate",
                table: "schedules");

            migrationBuilder.DropColumn(
                name: "returnArrivalTime",
                table: "schedules");

            migrationBuilder.RenameColumn(
                name: "returnDepartureTime",
                table: "schedules",
                newName: "returnTime");

            migrationBuilder.RenameColumn(
                name: "returnDepartureDate",
                table: "schedules",
                newName: "returnDate");

            migrationBuilder.AddColumn<bool>(
                name: "isOffer",
                table: "tickets",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }
    }
}
