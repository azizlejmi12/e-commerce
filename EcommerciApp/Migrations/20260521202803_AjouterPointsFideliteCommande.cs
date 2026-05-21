using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EcommerciApp.Migrations
{
    /// <inheritdoc />
    public partial class AjouterPointsFideliteCommande : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "PointsUtilises",
                table: "Commandes",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<decimal>(
                name: "Reduction",
                table: "Commandes",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PointsUtilises",
                table: "Commandes");

            migrationBuilder.DropColumn(
                name: "Reduction",
                table: "Commandes");
        }
    }
}
