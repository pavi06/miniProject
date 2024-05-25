using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HotelBookingSystemAPI.Migrations
{
    public partial class modelsUpdated : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Status",
                table: "Rooms");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "Hotels");

            migrationBuilder.AddColumn<int>(
                name: "NoOfRoomsAvailable",
                table: "Hotels",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "NoOfRoomsAvailable",
                table: "Hotels");

            migrationBuilder.AddColumn<string>(
                name: "Status",
                table: "Rooms",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Status",
                table: "Hotels",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
