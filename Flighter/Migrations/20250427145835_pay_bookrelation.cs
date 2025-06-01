using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Flighter.Migrations
{
    /// <inheritdoc />
    public partial class pay_bookrelation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "BookingId",
                table: "payments",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_payments_BookingId",
                table: "payments",
                column: "BookingId",
                unique: true);

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
                name: "FK_payments_bookings_BookingId",
                table: "payments");

            migrationBuilder.DropIndex(
                name: "IX_payments_BookingId",
                table: "payments");

            migrationBuilder.DropColumn(
                name: "BookingId",
                table: "payments");
        }
    }
}
