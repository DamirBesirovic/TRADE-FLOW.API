using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OTSupply.API.Migrations
{
    /// <inheritdoc />
    public partial class Prvamigracija : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Gradovi",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Gradovi", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Kategorije",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Kategorije", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Ocene",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Ocena = table.Column<double>(type: "float", nullable: false),
                    Komentar = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Ocene", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Oglasi",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Naslov = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Opis = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Materijal = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Cena = table.Column<int>(type: "int", nullable: false),
                    Mesto = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Kategorija_Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Grad_Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    KategorijaId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    GradId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Oglasi", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Oglasi_Gradovi_GradId",
                        column: x => x.GradId,
                        principalTable: "Gradovi",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Oglasi_Kategorije_KategorijaId",
                        column: x => x.KategorijaId,
                        principalTable: "Kategorije",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Oglasi_GradId",
                table: "Oglasi",
                column: "GradId");

            migrationBuilder.CreateIndex(
                name: "IX_Oglasi_KategorijaId",
                table: "Oglasi",
                column: "KategorijaId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Ocene");

            migrationBuilder.DropTable(
                name: "Oglasi");

            migrationBuilder.DropTable(
                name: "Gradovi");

            migrationBuilder.DropTable(
                name: "Kategorije");
        }
    }
}
