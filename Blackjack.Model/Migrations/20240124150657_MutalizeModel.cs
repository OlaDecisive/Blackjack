using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Blackjack.Model.Migrations
{
    /// <inheritdoc />
    public partial class MutalizeModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GameStates_Games_GameId",
                table: "GameStates");

            migrationBuilder.DropIndex(
                name: "IX_GameStates_GameId",
                table: "GameStates");

            migrationBuilder.DropColumn(
                name: "GameId",
                table: "GameStates");

            migrationBuilder.AddColumn<Guid>(
                name: "CurrentRoundId",
                table: "Games",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_Games_CurrentRoundId",
                table: "Games",
                column: "CurrentRoundId");

            migrationBuilder.AddForeignKey(
                name: "FK_Games_GameStates_CurrentRoundId",
                table: "Games",
                column: "CurrentRoundId",
                principalTable: "GameStates",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Games_GameStates_CurrentRoundId",
                table: "Games");

            migrationBuilder.DropIndex(
                name: "IX_Games_CurrentRoundId",
                table: "Games");

            migrationBuilder.DropColumn(
                name: "CurrentRoundId",
                table: "Games");

            migrationBuilder.AddColumn<Guid>(
                name: "GameId",
                table: "GameStates",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_GameStates_GameId",
                table: "GameStates",
                column: "GameId");

            migrationBuilder.AddForeignKey(
                name: "FK_GameStates_Games_GameId",
                table: "GameStates",
                column: "GameId",
                principalTable: "Games",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
