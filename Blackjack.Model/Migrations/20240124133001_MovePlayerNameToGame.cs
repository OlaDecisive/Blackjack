using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Blackjack.Model.Migrations
{
    /// <inheritdoc />
    public partial class MovePlayerNameToGame : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PlayerName",
                table: "GameStates");

            migrationBuilder.AddColumn<string>(
                name: "PlayerName",
                table: "Games",
                type: "TEXT",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PlayerName",
                table: "Games");

            migrationBuilder.AddColumn<string>(
                name: "PlayerName",
                table: "GameStates",
                type: "TEXT",
                nullable: false,
                defaultValue: "");
        }
    }
}
