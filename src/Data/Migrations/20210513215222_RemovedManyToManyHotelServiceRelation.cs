using Microsoft.EntityFrameworkCore.Migrations;

namespace HotelReservation.Data.Migrations
{
    /// <summary>
    /// /
    /// </summary>
    public partial class RemovedManyToManyHotelServiceRelation : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Locations_Hotels_HotelId",
                table: "Locations");

            migrationBuilder.DropForeignKey(
                name: "FK_Rooms_Hotels_HotelId",
                table: "Rooms");

            migrationBuilder.DropTable(
                name: "HotelServices");

            migrationBuilder.AddColumn<int>(
                name: "HotelId",
                table: "Services",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Services_HotelId",
                table: "Services",
                column: "HotelId");

            migrationBuilder.AddForeignKey(
                name: "FK_Locations_Hotels_HotelId",
                table: "Locations",
                column: "HotelId",
                principalTable: "Hotels",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Rooms_Hotels_HotelId",
                table: "Rooms",
                column: "HotelId",
                principalTable: "Hotels",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Services_Hotels_HotelId",
                table: "Services",
                column: "HotelId",
                principalTable: "Hotels",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Locations_Hotels_HotelId",
                table: "Locations");

            migrationBuilder.DropForeignKey(
                name: "FK_Rooms_Hotels_HotelId",
                table: "Rooms");

            migrationBuilder.DropForeignKey(
                name: "FK_Services_Hotels_HotelId",
                table: "Services");

            migrationBuilder.DropIndex(
                name: "IX_Services_HotelId",
                table: "Services");

            migrationBuilder.DropColumn(
                name: "HotelId",
                table: "Services");

            migrationBuilder.CreateTable(
                name: "HotelServices",
                columns: table => new
                {
                    HotelId = table.Column<int>(type: "int", nullable: false),
                    ServiceId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HotelServices", x => new { x.HotelId, x.ServiceId });
                    table.ForeignKey(
                        name: "FK_HotelServices_Hotels_HotelId",
                        column: x => x.HotelId,
                        principalTable: "Hotels",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_HotelServices_Services_ServiceId",
                        column: x => x.ServiceId,
                        principalTable: "Services",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_HotelServices_ServiceId",
                table: "HotelServices",
                column: "ServiceId");

            migrationBuilder.AddForeignKey(
                name: "FK_Locations_Hotels_HotelId",
                table: "Locations",
                column: "HotelId",
                principalTable: "Hotels",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Rooms_Hotels_HotelId",
                table: "Rooms",
                column: "HotelId",
                principalTable: "Hotels",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
