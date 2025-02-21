using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Assurances.API.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ContratAssurances",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    CodePartenaire = table.Column<string>(type: "TEXT", nullable: false),
                    IdClient = table.Column<int>(type: "INTEGER", nullable: false),
                    NomDemandeur = table.Column<string>(type: "TEXT", nullable: false),
                    SexeDemandeur = table.Column<string>(type: "TEXT", nullable: false),
                    DateNaissance = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Montant = table.Column<decimal>(type: "TEXT", nullable: false),
                    EstFumeur = table.Column<bool>(type: "INTEGER", nullable: false),
                    EstDiabetique = table.Column<bool>(type: "INTEGER", nullable: false),
                    EstHypertendu = table.Column<bool>(type: "INTEGER", nullable: false),
                    EstPhysiquementActif = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ContratAssurances", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ContratAssurances");
        }
    }
}
