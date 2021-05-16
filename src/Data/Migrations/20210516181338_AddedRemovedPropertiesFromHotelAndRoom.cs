using Microsoft.EntityFrameworkCore.Migrations;

namespace HotelReservation.Data.Migrations
{
    /// <summary>
    /// /
    /// </summary>
    public partial class AddedRemovedPropertiesFromHotelAndRoom : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsEmpty",
                table: "Rooms");

            migrationBuilder.AddColumn<double>(
                name: "Price",
                table: "Rooms",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "Deposit",
                table: "Hotels",
                nullable: false,
                defaultValue: 0.0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Price",
                table: "Rooms");

            migrationBuilder.DropColumn(
                name: "Deposit",
                table: "Hotels");

            migrationBuilder.AddColumn<bool>(
                name: "IsEmpty",
                table: "Rooms",
                type: "bit",
                nullable: false,
                defaultValue: true);
        }
    }
}
