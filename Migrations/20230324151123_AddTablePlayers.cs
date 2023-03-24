using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CsGOStateEmitter.Migrations
{
    /// <inheritdoc />
    public partial class AddTablePlayers : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PlayersAntiCheating",
                columns: table => new
                {
                    SteamId = table.Column<string>(type: "varchar(95)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Name = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    IsConnected = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    Map = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    LastPhotoTaken = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    IsAntiCheatOpen = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    Expiration = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlayersAntiCheating", x => x.SteamId);
                })
                .Annotation("MySql:CharSet", "utf8mb4");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PlayersAntiCheating");
        }
    }
}
