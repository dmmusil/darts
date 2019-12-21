using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Darts.Players.Persistence.SQL.Migrations
{
    public partial class AddAuthToken : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "AuthToken",
                table: "Players",
                nullable: false,
                defaultValueSql: "NEWID()");

            migrationBuilder.CreateIndex(
                name: "IX_Players_AuthToken",
                table: "Players",
                column: "AuthToken",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Players_AuthToken",
                table: "Players");

            migrationBuilder.DropColumn(
                name: "AuthToken",
                table: "Players");
        }
    }
}
