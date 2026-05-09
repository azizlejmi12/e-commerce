using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EcommerciApp.Migrations
{
    /// <inheritdoc />
    public partial class AddPointsFidelite : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "PointsFidelite",
                table: "AspNetUsers",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "TotalPointsGagnes",
                table: "AspNetUsers",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PointsFidelite",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "TotalPointsGagnes",
                table: "AspNetUsers");
        }
    }
}
