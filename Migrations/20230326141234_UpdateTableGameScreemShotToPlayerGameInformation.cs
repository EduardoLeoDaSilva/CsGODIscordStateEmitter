using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CsGOStateEmitter.Migrations
{
    /// <inheritdoc />
    public partial class UpdateTableGameScreemShotToPlayerGameInformation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "GameScreemShots");

            migrationBuilder.CreateTable(
                name: "PlayerGameInformations",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    PlayersSteamId = table.Column<string>(type: "varchar(95)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    MatchId = table.Column<int>(type: "int", nullable: false),
                    PathImage = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ProssesNamesBase64 = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CreatedDate = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlayerGameInformations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PlayerGameInformations_PlayersAntiCheating_PlayersSteamId",
                        column: x => x.PlayersSteamId,
                        principalTable: "PlayersAntiCheating",
                        principalColumn: "SteamId",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_PlayerGameInformations_PlayersSteamId",
                table: "PlayerGameInformations",
                column: "PlayersSteamId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PlayerGameInformations");

            migrationBuilder.CreateTable(
                name: "GameScreemShots",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    PlayersSteamId = table.Column<string>(type: "varchar(95)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CreatedDate = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    ImageBase64 = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    MatchId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GameScreemShots", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GameScreemShots_PlayersAntiCheating_PlayersSteamId",
                        column: x => x.PlayersSteamId,
                        principalTable: "PlayersAntiCheating",
                        principalColumn: "SteamId",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_GameScreemShots_PlayersSteamId",
                table: "GameScreemShots",
                column: "PlayersSteamId");
        }
    }
}
