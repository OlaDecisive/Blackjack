using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Blackjack.Model.Migrations
{
    /// <inheritdoc />
    public partial class AddSettersToGameStateProperties : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GameStates_Games_GameId",
                table: "GameStates");

            migrationBuilder.AlterColumn<Guid>(
                name: "GameId",
                table: "GameStates",
                type: "TEXT",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "TEXT",
                oldNullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "DealerHandId",
                table: "GameStates",
                type: "TEXT",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "DeckId",
                table: "GameStates",
                type: "TEXT",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "PlayerHandId",
                table: "GameStates",
                type: "TEXT",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_GameStates_DealerHandId",
                table: "GameStates",
                column: "DealerHandId");

            migrationBuilder.CreateIndex(
                name: "IX_GameStates_DeckId",
                table: "GameStates",
                column: "DeckId");

            migrationBuilder.CreateIndex(
                name: "IX_GameStates_PlayerHandId",
                table: "GameStates",
                column: "PlayerHandId");

            migrationBuilder.AddForeignKey(
                name: "FK_GameStates_Decks_DeckId",
                table: "GameStates",
                column: "DeckId",
                principalTable: "Decks",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_GameStates_Games_GameId",
                table: "GameStates",
                column: "GameId",
                principalTable: "Games",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_GameStates_Hands_DealerHandId",
                table: "GameStates",
                column: "DealerHandId",
                principalTable: "Hands",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_GameStates_Hands_PlayerHandId",
                table: "GameStates",
                column: "PlayerHandId",
                principalTable: "Hands",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GameStates_Decks_DeckId",
                table: "GameStates");

            migrationBuilder.DropForeignKey(
                name: "FK_GameStates_Games_GameId",
                table: "GameStates");

            migrationBuilder.DropForeignKey(
                name: "FK_GameStates_Hands_DealerHandId",
                table: "GameStates");

            migrationBuilder.DropForeignKey(
                name: "FK_GameStates_Hands_PlayerHandId",
                table: "GameStates");

            migrationBuilder.DropIndex(
                name: "IX_GameStates_DealerHandId",
                table: "GameStates");

            migrationBuilder.DropIndex(
                name: "IX_GameStates_DeckId",
                table: "GameStates");

            migrationBuilder.DropIndex(
                name: "IX_GameStates_PlayerHandId",
                table: "GameStates");

            migrationBuilder.DropColumn(
                name: "DealerHandId",
                table: "GameStates");

            migrationBuilder.DropColumn(
                name: "DeckId",
                table: "GameStates");

            migrationBuilder.DropColumn(
                name: "PlayerHandId",
                table: "GameStates");

            migrationBuilder.AlterColumn<Guid>(
                name: "GameId",
                table: "GameStates",
                type: "TEXT",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "TEXT");

            migrationBuilder.AddForeignKey(
                name: "FK_GameStates_Games_GameId",
                table: "GameStates",
                column: "GameId",
                principalTable: "Games",
                principalColumn: "Id");
        }
    }
}
