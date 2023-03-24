using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CsGOStateEmitter.Migrations
{
    /// <inheritdoc />
    public partial class AddUpdateNameField : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SteamId",
                table: "GameScreemShots");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "SteamId",
                table: "GameScreemShots",
                type: "longtext",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");
        }
    }
}
