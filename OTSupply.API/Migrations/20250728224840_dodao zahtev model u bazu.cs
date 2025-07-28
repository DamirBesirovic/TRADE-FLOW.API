using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OTSupply.API.Migrations
{
    /// <inheritdoc />
    public partial class dodaozahtevmodelubazu : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Zahtevi",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Oglas_Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Grad_Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Kupac_Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Kolicina = table.Column<int>(type: "int", nullable: false),
                    Poruka = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Telefon = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PoslatoVreme = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Procitano = table.Column<bool>(type: "bit", nullable: false),
                    VlasnikOglasa_KorisnikId = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Zahtevi", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Zahtevi");
        }
    }
}
