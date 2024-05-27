using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HotelBookingSystemAPI.Migrations
{
    public partial class editedModels : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<double>(
                name: "Discount",
                table: "RoomTypes",
                type: "float",
                nullable: false,
                defaultValue: 0.0,
                oldClrType: typeof(double),
                oldType: "float",
                oldNullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<double>(
                name: "Discount",
                table: "RoomTypes",
                type: "float",
                nullable: true,
                oldClrType: typeof(double),
                oldType: "float");
        }
    }
}
