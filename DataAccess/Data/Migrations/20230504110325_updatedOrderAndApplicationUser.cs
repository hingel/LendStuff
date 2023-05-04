using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LendStuff.Server.Data.Migrations
{
    /// <inheritdoc />
    public partial class updatedOrderAndApplicationUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BoardGames_AspNetUsers_ApplicationUserId",
                table: "BoardGames");

            migrationBuilder.DropIndex(
                name: "IX_BoardGames_ApplicationUserId",
                table: "BoardGames");

            migrationBuilder.DropColumn(
                name: "ApplicationUserId",
                table: "BoardGames");

            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "Orders",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "BggLink",
                table: "BoardGames",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "ApplicationUserBoardGame",
                columns: table => new
                {
                    CollectionOfBoardGamesId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    UsersId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ApplicationUserBoardGame", x => new { x.CollectionOfBoardGamesId, x.UsersId });
                    table.ForeignKey(
                        name: "FK_ApplicationUserBoardGame_AspNetUsers_UsersId",
                        column: x => x.UsersId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ApplicationUserBoardGame_BoardGames_CollectionOfBoardGamesId",
                        column: x => x.CollectionOfBoardGamesId,
                        principalTable: "BoardGames",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ApplicationUserBoardGame_UsersId",
                table: "ApplicationUserBoardGame",
                column: "UsersId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ApplicationUserBoardGame");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "BggLink",
                table: "BoardGames");

            migrationBuilder.AddColumn<string>(
                name: "ApplicationUserId",
                table: "BoardGames",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_BoardGames_ApplicationUserId",
                table: "BoardGames",
                column: "ApplicationUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_BoardGames_AspNetUsers_ApplicationUserId",
                table: "BoardGames",
                column: "ApplicationUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }
    }
}
