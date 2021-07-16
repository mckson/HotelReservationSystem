using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace HotelReservation.Data.Migrations
{
    /// <summary>
    /// /
    /// </summary>
    public partial class AddedPropertiesToReservationAndRoom : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "LockedByUserId",
                table: "Rooms",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "UnlockTime",
                table: "Rooms",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PassportNumber",
                table: "Reservations",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PhoneNumber",
                table: "Reservations",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PassportNumber",
                table: "AspNetUsers",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LockedByUserId",
                table: "Rooms");

            migrationBuilder.DropColumn(
                name: "UnlockTime",
                table: "Rooms");

            migrationBuilder.DropColumn(
                name: "PassportNumber",
                table: "Reservations");

            migrationBuilder.DropColumn(
                name: "PhoneNumber",
                table: "Reservations");

            migrationBuilder.DropColumn(
                name: "PassportNumber",
                table: "AspNetUsers");
        }
    }
}
