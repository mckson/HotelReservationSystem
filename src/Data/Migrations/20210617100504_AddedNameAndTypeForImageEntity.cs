using Microsoft.EntityFrameworkCore.Migrations;

namespace HotelReservation.Data.Migrations
{
    /// <summary>
    /// /
    /// </summary>
    public partial class AddedNameAndTypeForImageEntity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "MainImageEntity",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Type",
                table: "MainImageEntity",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "ImageEntity",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Type",
                table: "ImageEntity",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Name",
                table: "MainImageEntity");

            migrationBuilder.DropColumn(
                name: "Type",
                table: "MainImageEntity");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "ImageEntity");

            migrationBuilder.DropColumn(
                name: "Type",
                table: "ImageEntity");
        }
    }
}
