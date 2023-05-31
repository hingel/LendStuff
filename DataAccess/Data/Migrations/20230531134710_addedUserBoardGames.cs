using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LendStuff.Server.Data.Migrations
{
    /// <inheritdoc />
    public partial class addedUserBoardGames : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ApplicationUserBoardGame");

            migrationBuilder.AddColumn<string>(
                name: "BoardGameId",
                table: "AspNetUsers",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "UserBoardGame",
                columns: table => new
                {
                    UserBoardGameId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    BoardGameId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ForLending = table.Column<bool>(type: "bit", nullable: false),
                    ApplicationUserId = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserBoardGame", x => x.UserBoardGameId);
                    table.ForeignKey(
                        name: "FK_UserBoardGame_AspNetUsers_ApplicationUserId",
                        column: x => x.ApplicationUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_UserBoardGame_BoardGames_BoardGameId",
                        column: x => x.BoardGameId,
                        principalTable: "BoardGames",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_BoardGameId",
                table: "AspNetUsers",
                column: "BoardGameId");

            migrationBuilder.CreateIndex(
                name: "IX_UserBoardGame_ApplicationUserId",
                table: "UserBoardGame",
                column: "ApplicationUserId");

            migrationBuilder.CreateIndex(
                name: "IX_UserBoardGame_BoardGameId",
                table: "UserBoardGame",
                column: "BoardGameId");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_BoardGames_BoardGameId",
                table: "AspNetUsers",
                column: "BoardGameId",
                principalTable: "BoardGames",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_BoardGames_BoardGameId",
                table: "AspNetUsers");

            migrationBuilder.DropTable(
                name: "UserBoardGame");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_BoardGameId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "BoardGameId",
                table: "AspNetUsers");

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
    }
}
