using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CarteDeCredit.API.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CarteCredits",
                columns: table => new
                {
                    Numero = table.Column<int>(type: "INTEGER", nullable: false),
                    NomDemandeur = table.Column<string>(type: "TEXT", nullable: false),
                    TypeCarte = table.Column<string>(type: "TEXT", nullable: false),
                    LimiteCredit = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CarteCredits", x => x.Numero);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CarteCredits");
        }
    }
}
