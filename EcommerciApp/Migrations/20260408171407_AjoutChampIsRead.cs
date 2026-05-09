using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EcommerciApp.Migrations
{
    /// <inheritdoc />
    public partial class AjoutChampIsRead : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsRead",
                table: "Commandes",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsRead",
                table: "Commandes");
        }
    }
}
