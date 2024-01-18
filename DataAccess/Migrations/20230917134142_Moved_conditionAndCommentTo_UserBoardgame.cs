using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LendStuff.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class Moved_conditionAndCommentTo_UserBoardgame : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Comment",
                table: "BoardGames");

            migrationBuilder.DropColumn(
                name: "Condition",
                table: "BoardGames");

            migrationBuilder.AddColumn<string>(
                name: "Comment",
                table: "UserBoardGame",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Condition",
                table: "UserBoardGame",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Comment",
                table: "UserBoardGame");

            migrationBuilder.DropColumn(
                name: "Condition",
                table: "UserBoardGame");

            migrationBuilder.AddColumn<string>(
                name: "Comment",
                table: "BoardGames",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Condition",
                table: "BoardGames",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
