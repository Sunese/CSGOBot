using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Repository.Migrations
{
    /// <inheritdoc />
    public partial class CsgoGamePlayerIdIsPrimary : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Games_Csgo_csgogame_profile_id",
                table: "Games");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Csgo",
                table: "Csgo");

            migrationBuilder.RenameColumn(
                name: "csgogame_profile_id",
                table: "Games",
                newName: "csgogame_player_id");

            migrationBuilder.RenameIndex(
                name: "IX_Games_csgogame_profile_id",
                table: "Games",
                newName: "IX_Games_csgogame_player_id");

            migrationBuilder.AlterColumn<string>(
                name: "game_player_id",
                table: "Csgo",
                type: "varchar(255)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "longtext")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "game_profile_id",
                table: "Csgo",
                type: "longtext",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(255)")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Csgo",
                table: "Csgo",
                column: "game_player_id");

            migrationBuilder.AddForeignKey(
                name: "FK_Games_Csgo_csgogame_player_id",
                table: "Games",
                column: "csgogame_player_id",
                principalTable: "Csgo",
                principalColumn: "game_player_id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Games_Csgo_csgogame_player_id",
                table: "Games");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Csgo",
                table: "Csgo");

            migrationBuilder.RenameColumn(
                name: "csgogame_player_id",
                table: "Games",
                newName: "csgogame_profile_id");

            migrationBuilder.RenameIndex(
                name: "IX_Games_csgogame_player_id",
                table: "Games",
                newName: "IX_Games_csgogame_profile_id");

            migrationBuilder.AlterColumn<string>(
                name: "game_profile_id",
                table: "Csgo",
                type: "varchar(255)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "longtext")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "game_player_id",
                table: "Csgo",
                type: "longtext",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(255)")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Csgo",
                table: "Csgo",
                column: "game_profile_id");

            migrationBuilder.AddForeignKey(
                name: "FK_Games_Csgo_csgogame_profile_id",
                table: "Games",
                column: "csgogame_profile_id",
                principalTable: "Csgo",
                principalColumn: "game_profile_id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
