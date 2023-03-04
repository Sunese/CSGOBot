using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Repository.Migrations
{
    /// <inheritdoc />
    public partial class FKeySteamID64 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Games_Csgo_csgogame_player_id",
                table: "Games");

            migrationBuilder.DropIndex(
                name: "IX_Games_csgogame_player_id",
                table: "Games");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Csgo",
                table: "Csgo");

            migrationBuilder.DropColumn(
                name: "csgogame_player_id",
                table: "Games");

            migrationBuilder.AddColumn<Guid>(
                name: "csgoId",
                table: "Games",
                type: "char(36)",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                collation: "ascii_general_ci");

            migrationBuilder.AlterColumn<string>(
                name: "game_player_id",
                table: "Csgo",
                type: "longtext",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(255)")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<Guid>(
                name: "Id",
                table: "Csgo",
                type: "char(36)",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                collation: "ascii_general_ci");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Csgo",
                table: "Csgo",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_Games_csgoId",
                table: "Games",
                column: "csgoId");

            migrationBuilder.AddForeignKey(
                name: "FK_Games_Csgo_csgoId",
                table: "Games",
                column: "csgoId",
                principalTable: "Csgo",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Games_Csgo_csgoId",
                table: "Games");

            migrationBuilder.DropIndex(
                name: "IX_Games_csgoId",
                table: "Games");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Csgo",
                table: "Csgo");

            migrationBuilder.DropColumn(
                name: "csgoId",
                table: "Games");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "Csgo");

            migrationBuilder.AddColumn<string>(
                name: "csgogame_player_id",
                table: "Games",
                type: "varchar(255)",
                nullable: false,
                defaultValue: "")
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "game_player_id",
                table: "Csgo",
                type: "varchar(255)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "longtext")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Csgo",
                table: "Csgo",
                column: "game_player_id");

            migrationBuilder.CreateIndex(
                name: "IX_Games_csgogame_player_id",
                table: "Games",
                column: "csgogame_player_id");

            migrationBuilder.AddForeignKey(
                name: "FK_Games_Csgo_csgogame_player_id",
                table: "Games",
                column: "csgogame_player_id",
                principalTable: "Csgo",
                principalColumn: "game_player_id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
