using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Repository.Migrations
{
    /// <inheritdoc />
    public partial class Default100BettingBalance : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<double>(
                name: "BettingBalance",
                table: "Users",
                type: "double",
                nullable: false,
                defaultValue: 100.0,
                oldClrType: typeof(double),
                oldType: "double");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<double>(
                name: "BettingBalance",
                table: "Users",
                type: "double",
                nullable: false,
                oldClrType: typeof(double),
                oldType: "double",
                oldDefaultValue: 100.0);
        }
    }
}
