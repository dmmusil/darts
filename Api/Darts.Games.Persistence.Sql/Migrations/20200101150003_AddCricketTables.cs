﻿using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Darts.Games.Persistence.Sql.Migrations
{
    public partial class AddCricketTables : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CricketGames",
                columns: table => new
                {
                    GameId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CricketGames", x => x.GameId);
                });

            migrationBuilder.CreateTable(
                name: "Player",
                columns: table => new
                {
                    PlayerId = table.Column<Guid>(nullable: false),
                    GameId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Player", x => x.PlayerId);
                    table.ForeignKey(
                        name: "FK_Player_CricketGames_GameId",
                        column: x => x.GameId,
                        principalTable: "CricketGames",
                        principalColumn: "GameId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Turn",
                columns: table => new
                {
                    TurnId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PlayerId = table.Column<Guid>(nullable: false),
                    Order = table.Column<int>(nullable: false),
                    GameId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Turn", x => x.TurnId);
                    table.ForeignKey(
                        name: "FK_Turn_CricketGames_GameId",
                        column: x => x.GameId,
                        principalTable: "CricketGames",
                        principalColumn: "GameId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Turn_Player_PlayerId",
                        column: x => x.PlayerId,
                        principalTable: "Player",
                        principalColumn: "PlayerId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Score",
                columns: table => new
                {
                    Segment = table.Column<int>(nullable: false),
                    TurnId = table.Column<int>(nullable: false),
                    Count = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Score", x => new { x.TurnId, x.Segment });
                    table.ForeignKey(
                        name: "FK_Score_Turn_TurnId",
                        column: x => x.TurnId,
                        principalTable: "Turn",
                        principalColumn: "TurnId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Player_GameId",
                table: "Player",
                column: "GameId");

            migrationBuilder.CreateIndex(
                name: "IX_Score_TurnId_Segment",
                table: "Score",
                columns: new[] { "TurnId", "Segment" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Turn_GameId",
                table: "Turn",
                column: "GameId");

            migrationBuilder.CreateIndex(
                name: "IX_Turn_PlayerId_GameId_Order",
                table: "Turn",
                columns: new[] { "PlayerId", "GameId", "Order" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Score");

            migrationBuilder.DropTable(
                name: "Turn");

            migrationBuilder.DropTable(
                name: "Player");

            migrationBuilder.DropTable(
                name: "CricketGames");
        }
    }
}
