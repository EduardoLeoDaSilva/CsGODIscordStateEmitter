using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CsGOStateEmitter.Migrations
{
    /// <inheritdoc />
    public partial class AddNameImagem : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "NameImage",
                table: "PlayerGameInformations",
                type: "longtext",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "NameImage",
                table: "PlayerGameInformations");
        }
    }
}
