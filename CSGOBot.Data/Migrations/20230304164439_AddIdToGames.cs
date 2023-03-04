using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Repository.Migrations
{
    /// <inheritdoc />
    public partial class AddIdToGames : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FaceitPlayers_Games_gamesid",
                table: "FaceitPlayers");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "Games",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "gamesid",
                table: "FaceitPlayers",
                newName: "gamesId");

            migrationBuilder.RenameIndex(
                name: "IX_FaceitPlayers_gamesid",
                table: "FaceitPlayers",
                newName: "IX_FaceitPlayers_gamesId");

            migrationBuilder.AddForeignKey(
                name: "FK_FaceitPlayers_Games_gamesId",
                table: "FaceitPlayers",
                column: "gamesId",
                principalTable: "Games",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FaceitPlayers_Games_gamesId",
                table: "FaceitPlayers");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Games",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "gamesId",
                table: "FaceitPlayers",
                newName: "gamesid");

            migrationBuilder.RenameIndex(
                name: "IX_FaceitPlayers_gamesId",
                table: "FaceitPlayers",
                newName: "IX_FaceitPlayers_gamesid");

            migrationBuilder.AddForeignKey(
                name: "FK_FaceitPlayers_Games_gamesid",
                table: "FaceitPlayers",
                column: "gamesid",
                principalTable: "Games",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
