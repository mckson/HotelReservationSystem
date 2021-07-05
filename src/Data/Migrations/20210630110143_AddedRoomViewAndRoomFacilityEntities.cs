using Microsoft.EntityFrameworkCore.Migrations;
using System;

namespace HotelReservation.Data.Migrations
{
    /// <summary>
    /// /
    /// </summary>
    public partial class AddedRoomViewAndRoomFacilityEntities : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "Area",
                table: "Rooms",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "Rooms",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "Rooms",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "Parking",
                table: "Rooms",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "Smoking",
                table: "Rooms",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateTable(
                name: "RoomFacilityEntity",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    RoomId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RoomFacilityEntity", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RoomFacilityEntity_Rooms_RoomId",
                        column: x => x.RoomId,
                        principalTable: "Rooms",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RoomViewEntity",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RoomViewEntity", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "RoomRoomViewEntity",
                columns: table => new
                {
                    RoomId = table.Column<Guid>(nullable: false),
                    RoomViewId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RoomRoomViewEntity", x => new { x.RoomId, x.RoomViewId });
                    table.ForeignKey(
                        name: "FK_RoomRoomViewEntity_Rooms_RoomId",
                        column: x => x.RoomId,
                        principalTable: "Rooms",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RoomRoomViewEntity_RoomViewEntity_RoomViewId",
                        column: x => x.RoomViewId,
                        principalTable: "RoomViewEntity",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_RoomFacilityEntity_RoomId",
                table: "RoomFacilityEntity",
                column: "RoomId");

            migrationBuilder.CreateIndex(
                name: "IX_RoomRoomViewEntity_RoomViewId",
                table: "RoomRoomViewEntity",
                column: "RoomViewId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RoomFacilityEntity");

            migrationBuilder.DropTable(
                name: "RoomRoomViewEntity");

            migrationBuilder.DropTable(
                name: "RoomViewEntity");

            migrationBuilder.DropColumn(
                name: "Area",
                table: "Rooms");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "Rooms");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "Rooms");

            migrationBuilder.DropColumn(
                name: "Parking",
                table: "Rooms");

            migrationBuilder.DropColumn(
                name: "Smoking",
                table: "Rooms");
        }
    }
}
