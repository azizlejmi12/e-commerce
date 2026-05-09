using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EcommerciApp.Migrations
{
    /// <inheritdoc />
    public partial class AddTelephoneToCommande : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Telephone",
                table: "Commandes",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Ville",
                table: "Commandes",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Telephone",
                table: "Commandes");

            migrationBuilder.DropColumn(
                name: "Ville",
                table: "Commandes");
        }
    }
}
