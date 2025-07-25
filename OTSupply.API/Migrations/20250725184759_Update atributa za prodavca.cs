using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OTSupply.API.Migrations
{
    /// <inheritdoc />
    public partial class Updateatributazaprodavca : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "PhoneNumber",
                table: "Prodavci",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PhoneNumber",
                table: "Prodavci");
        }
    }
}
