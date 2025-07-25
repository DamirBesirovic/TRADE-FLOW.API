using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace OTSupply.API.Migrations
{
    /// <inheritdoc />
    public partial class Dodavanjekorisnikarolovaiautorizacije : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Oglasi_Gradovi_GradId",
                table: "Oglasi");

            migrationBuilder.DropForeignKey(
                name: "FK_Oglasi_Kategorije_KategorijaId",
                table: "Oglasi");

            migrationBuilder.DropIndex(
                name: "IX_Oglasi_GradId",
                table: "Oglasi");

            migrationBuilder.DropColumn(
                name: "GradId",
                table: "Oglasi");

            migrationBuilder.RenameColumn(
                name: "KategorijaId",
                table: "Oglasi",
                newName: "Prodavac_Id");

            migrationBuilder.RenameIndex(
                name: "IX_Oglasi_KategorijaId",
                table: "Oglasi",
                newName: "IX_Oglasi_Prodavac_Id");

            migrationBuilder.AlterColumn<string>(
                name: "Naslov",
                table: "Oglasi",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<int>(
                name: "Cena",
                table: "Oglasi",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<string>(
                name: "KorisnikId",
                table: "Ocene",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "ProdavacId",
                table: "Ocene",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "AspNetRoles",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUsers",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Ime = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Prezime = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    isFirstLogin = table.Column<bool>(type: "bit", nullable: false),
                    DatumRegistracije = table.Column<DateTime>(type: "datetime2", nullable: false),
                    isDeleted = table.Column<bool>(type: "bit", nullable: false),
                    UserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedUserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    Email = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedEmail = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SecurityStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    TwoFactorEnabled = table.Column<bool>(type: "bit", nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    LockoutEnabled = table.Column<bool>(type: "bit", nullable: false),
                    AccessFailedCount = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUsers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ImageUrls",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Url = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    OglasId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ImageUrls", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ImageUrls_Oglasi_OglasId",
                        column: x => x.OglasId,
                        principalTable: "Oglasi",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoleClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoleId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoleClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetRoleClaims_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetUserClaims_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserLogins",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProviderKey = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProviderDisplayName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserLogins", x => new { x.LoginProvider, x.ProviderKey });
                    table.ForeignKey(
                        name: "FK_AspNetUserLogins_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserRoles",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    RoleId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserRoles", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserTokens",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserTokens", x => new { x.UserId, x.LoginProvider, x.Name });
                    table.ForeignKey(
                        name: "FK_AspNetUserTokens_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Prodavci",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    bio = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Ime = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Ocena = table.Column<double>(type: "float", maxLength: 50, nullable: false),
                    pfpUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    isVerified = table.Column<bool>(type: "bit", nullable: false),
                    Id_Korisnik = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Prodavci", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Prodavci_AspNetUsers_Id_Korisnik",
                        column: x => x.Id_Korisnik,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "97871e9b-0f3f-4589-9358-1adbc1c873f8", "97871e9b-0f3f-4589-9358-1adbc1c873f8", "User", "USER" },
                    { "b24e986e-9b13-4355-9227-0f6e4d2b9cb2", "b24e986e-9b13-4355-9227-0f6e4d2b9cb2", "Admin", "ADMIN" },
                    { "f9992759-0283-4d2a-bf63-14f8155ee258", "f9992759-0283-4d2a-bf63-14f8155ee258", "Seller", "SELLER" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Oglasi_Grad_Id",
                table: "Oglasi",
                column: "Grad_Id");

            migrationBuilder.CreateIndex(
                name: "IX_Oglasi_Kategorija_Id",
                table: "Oglasi",
                column: "Kategorija_Id");

            migrationBuilder.CreateIndex(
                name: "IX_Oglasi_Naslov",
                table: "Oglasi",
                column: "Naslov");

            migrationBuilder.CreateIndex(
                name: "IX_Ocene_KorisnikId",
                table: "Ocene",
                column: "KorisnikId");

            migrationBuilder.CreateIndex(
                name: "IX_Ocene_ProdavacId",
                table: "Ocene",
                column: "ProdavacId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetRoleClaims_RoleId",
                table: "AspNetRoleClaims",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "AspNetRoles",
                column: "NormalizedName",
                unique: true,
                filter: "[NormalizedName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserClaims_UserId",
                table: "AspNetUserClaims",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserLogins_UserId",
                table: "AspNetUserLogins",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserRoles_RoleId",
                table: "AspNetUserRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                table: "AspNetUsers",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "AspNetUsers",
                column: "NormalizedUserName",
                unique: true,
                filter: "[NormalizedUserName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_ImageUrls_OglasId",
                table: "ImageUrls",
                column: "OglasId");

            migrationBuilder.CreateIndex(
                name: "IX_Prodavci_Id_Korisnik",
                table: "Prodavci",
                column: "Id_Korisnik");

            migrationBuilder.CreateIndex(
                name: "IX_Prodavci_Ime",
                table: "Prodavci",
                column: "Ime");

            migrationBuilder.AddForeignKey(
                name: "FK_Ocene_AspNetUsers_KorisnikId",
                table: "Ocene",
                column: "KorisnikId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Ocene_Prodavci_ProdavacId",
                table: "Ocene",
                column: "ProdavacId",
                principalTable: "Prodavci",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Oglasi_Gradovi_Grad_Id",
                table: "Oglasi",
                column: "Grad_Id",
                principalTable: "Gradovi",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Oglasi_Kategorije_Kategorija_Id",
                table: "Oglasi",
                column: "Kategorija_Id",
                principalTable: "Kategorije",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Oglasi_Prodavci_Prodavac_Id",
                table: "Oglasi",
                column: "Prodavac_Id",
                principalTable: "Prodavci",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Ocene_AspNetUsers_KorisnikId",
                table: "Ocene");

            migrationBuilder.DropForeignKey(
                name: "FK_Ocene_Prodavci_ProdavacId",
                table: "Ocene");

            migrationBuilder.DropForeignKey(
                name: "FK_Oglasi_Gradovi_Grad_Id",
                table: "Oglasi");

            migrationBuilder.DropForeignKey(
                name: "FK_Oglasi_Kategorije_Kategorija_Id",
                table: "Oglasi");

            migrationBuilder.DropForeignKey(
                name: "FK_Oglasi_Prodavci_Prodavac_Id",
                table: "Oglasi");

            migrationBuilder.DropTable(
                name: "AspNetRoleClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserLogins");

            migrationBuilder.DropTable(
                name: "AspNetUserRoles");

            migrationBuilder.DropTable(
                name: "AspNetUserTokens");

            migrationBuilder.DropTable(
                name: "ImageUrls");

            migrationBuilder.DropTable(
                name: "Prodavci");

            migrationBuilder.DropTable(
                name: "AspNetRoles");

            migrationBuilder.DropTable(
                name: "AspNetUsers");

            migrationBuilder.DropIndex(
                name: "IX_Oglasi_Grad_Id",
                table: "Oglasi");

            migrationBuilder.DropIndex(
                name: "IX_Oglasi_Kategorija_Id",
                table: "Oglasi");

            migrationBuilder.DropIndex(
                name: "IX_Oglasi_Naslov",
                table: "Oglasi");

            migrationBuilder.DropIndex(
                name: "IX_Ocene_KorisnikId",
                table: "Ocene");

            migrationBuilder.DropIndex(
                name: "IX_Ocene_ProdavacId",
                table: "Ocene");

            migrationBuilder.DropColumn(
                name: "KorisnikId",
                table: "Ocene");

            migrationBuilder.DropColumn(
                name: "ProdavacId",
                table: "Ocene");

            migrationBuilder.RenameColumn(
                name: "Prodavac_Id",
                table: "Oglasi",
                newName: "KategorijaId");

            migrationBuilder.RenameIndex(
                name: "IX_Oglasi_Prodavac_Id",
                table: "Oglasi",
                newName: "IX_Oglasi_KategorijaId");

            migrationBuilder.AlterColumn<string>(
                name: "Naslov",
                table: "Oglasi",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AlterColumn<int>(
                name: "Cena",
                table: "Oglasi",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldDefaultValue: 0);

            migrationBuilder.AddColumn<Guid>(
                name: "GradId",
                table: "Oglasi",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_Oglasi_GradId",
                table: "Oglasi",
                column: "GradId");

            migrationBuilder.AddForeignKey(
                name: "FK_Oglasi_Gradovi_GradId",
                table: "Oglasi",
                column: "GradId",
                principalTable: "Gradovi",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Oglasi_Kategorije_KategorijaId",
                table: "Oglasi",
                column: "KategorijaId",
                principalTable: "Kategorije",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
