using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LendStuff.Server.Data.Migrations
{
    /// <inheritdoc />
    public partial class updatedInternalMessage2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_InternalMessages_AspNetUsers_ApplicationUserId",
                table: "InternalMessages");

            migrationBuilder.RenameColumn(
                name: "ApplicationUserId",
                table: "InternalMessages",
                newName: "SentToUserId");

            migrationBuilder.RenameIndex(
                name: "IX_InternalMessages_ApplicationUserId",
                table: "InternalMessages",
                newName: "IX_InternalMessages_SentToUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_InternalMessages_AspNetUsers_SentToUserId",
                table: "InternalMessages",
                column: "SentToUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_InternalMessages_AspNetUsers_SentToUserId",
                table: "InternalMessages");

            migrationBuilder.RenameColumn(
                name: "SentToUserId",
                table: "InternalMessages",
                newName: "ApplicationUserId");

            migrationBuilder.RenameIndex(
                name: "IX_InternalMessages_SentToUserId",
                table: "InternalMessages",
                newName: "IX_InternalMessages_ApplicationUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_InternalMessages_AspNetUsers_ApplicationUserId",
                table: "InternalMessages",
                column: "ApplicationUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }
    }
}
