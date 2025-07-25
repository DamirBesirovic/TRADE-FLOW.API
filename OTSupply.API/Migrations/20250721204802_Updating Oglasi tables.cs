using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OTSupply.API.Migrations
{
    /// <inheritdoc />
    public partial class UpdatingOglasitables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "pfpUrl",
                table: "Prodavci",
                newName: "PfpUrl");

            migrationBuilder.RenameColumn(
                name: "isVerified",
                table: "Prodavci",
                newName: "IsVerified");

            migrationBuilder.RenameColumn(
                name: "bio",
                table: "Prodavci",
                newName: "Bio");

            migrationBuilder.RenameColumn(
                name: "Ime",
                table: "Prodavci",
                newName: "ImeFirme");

            migrationBuilder.RenameIndex(
                name: "IX_Prodavci_Ime",
                table: "Prodavci",
                newName: "IX_Prodavci_ImeFirme");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "PfpUrl",
                table: "Prodavci",
                newName: "pfpUrl");

            migrationBuilder.RenameColumn(
                name: "IsVerified",
                table: "Prodavci",
                newName: "isVerified");

            migrationBuilder.RenameColumn(
                name: "Bio",
                table: "Prodavci",
                newName: "bio");

            migrationBuilder.RenameColumn(
                name: "ImeFirme",
                table: "Prodavci",
                newName: "Ime");

            migrationBuilder.RenameIndex(
                name: "IX_Prodavci_ImeFirme",
                table: "Prodavci",
                newName: "IX_Prodavci_Ime");
        }
    }
}
