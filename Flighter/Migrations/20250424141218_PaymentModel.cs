using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Flighter.Migrations
{
    /// <inheritdoc />
    public partial class PaymentModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_payments_Users_userId",
                table: "payments");

            migrationBuilder.DropIndex(
                name: "IX_payments_userId",
                table: "payments");

            migrationBuilder.DropColumn(
                name: "cardHolderName",
                table: "payments");

            migrationBuilder.DropColumn(
                name: "cardNumber",
                table: "payments");

            migrationBuilder.DropColumn(
                name: "expiryDate",
                table: "payments");

            migrationBuilder.DropColumn(
                name: "paymentDate",
                table: "payments");

            migrationBuilder.RenameColumn(
                name: "paymentMethod",
                table: "payments",
                newName: "Amount");

            migrationBuilder.AlterColumn<string>(
                name: "userId",
                table: "payments",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddColumn<int>(
                name: "Payment_Intent_Id",
                table: "payments",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Payment_Intent_Id",
                table: "payments");

            migrationBuilder.RenameColumn(
                name: "Amount",
                table: "payments",
                newName: "paymentMethod");

            migrationBuilder.AlterColumn<string>(
                name: "userId",
                table: "payments",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<string>(
                name: "cardHolderName",
                table: "payments",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "cardNumber",
                table: "payments",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "expiryDate",
                table: "payments",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "paymentDate",
                table: "payments",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.CreateIndex(
                name: "IX_payments_userId",
                table: "payments",
                column: "userId");

            migrationBuilder.AddForeignKey(
                name: "FK_payments_Users_userId",
                table: "payments",
                column: "userId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
