using Microsoft.EntityFrameworkCore.Migrations;

namespace HotelReservation.Data.Migrations
{
    /// <summary>
    /// /
    /// </summary>
    public partial class AddedHotelServicesTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_HotelService_Hotels_HotelId",
                table: "HotelService");

            migrationBuilder.DropForeignKey(
                name: "FK_HotelService_Services_ServiceId",
                table: "HotelService");

            migrationBuilder.DropPrimaryKey(
                name: "PK_HotelService",
                table: "HotelService");

            migrationBuilder.RenameTable(
                name: "HotelService",
                newName: "HotelServices");

            migrationBuilder.RenameIndex(
                name: "IX_HotelService_ServiceId",
                table: "HotelServices",
                newName: "IX_HotelServices_ServiceId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_HotelServices",
                table: "HotelServices",
                columns: new[] { "HotelId", "ServiceId" });

            migrationBuilder.AddForeignKey(
                name: "FK_HotelServices_Hotels_HotelId",
                table: "HotelServices",
                column: "HotelId",
                principalTable: "Hotels",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_HotelServices_Services_ServiceId",
                table: "HotelServices",
                column: "ServiceId",
                principalTable: "Services",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_HotelServices_Hotels_HotelId",
                table: "HotelServices");

            migrationBuilder.DropForeignKey(
                name: "FK_HotelServices_Services_ServiceId",
                table: "HotelServices");

            migrationBuilder.DropPrimaryKey(
                name: "PK_HotelServices",
                table: "HotelServices");

            migrationBuilder.RenameTable(
                name: "HotelServices",
                newName: "HotelService");

            migrationBuilder.RenameIndex(
                name: "IX_HotelServices_ServiceId",
                table: "HotelService",
                newName: "IX_HotelService_ServiceId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_HotelService",
                table: "HotelService",
                columns: new[] { "HotelId", "ServiceId" });

            migrationBuilder.AddForeignKey(
                name: "FK_HotelService_Hotels_HotelId",
                table: "HotelService",
                column: "HotelId",
                principalTable: "Hotels",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_HotelService_Services_ServiceId",
                table: "HotelService",
                column: "ServiceId",
                principalTable: "Services",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
