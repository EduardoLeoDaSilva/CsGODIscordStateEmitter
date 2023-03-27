using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CsGOStateEmitter.Migrations
{
    /// <inheritdoc />
    public partial class AddTableGameScreemShot : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "GameScreemShots",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    SteamId = table.Column<long>(type: "bigint", nullable: false),
                    MatchId = table.Column<int>(type: "int", nullable: false),
                    ImageBase64 = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CreatedDate = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    PlayersSteamId = table.Column<string>(type: "varchar(95)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "GameScreemShots");
        }
    }
}
