using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LendStuff.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class addedIsReadTOMessages : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_InternalMessages_AspNetUsers_SentToUserId",
                table: "InternalMessages");

            migrationBuilder.AlterColumn<string>(
                name: "SentToUserId",
                table: "InternalMessages",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Message",
                table: "InternalMessages",
                type: "nvarchar(400)",
                maxLength: 400,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<bool>(
                name: "IsRead",
                table: "InternalMessages",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddForeignKey(
                name: "FK_InternalMessages_AspNetUsers_SentToUserId",
                table: "InternalMessages",
                column: "SentToUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_InternalMessages_AspNetUsers_SentToUserId",
                table: "InternalMessages");

            migrationBuilder.DropColumn(
                name: "IsRead",
                table: "InternalMessages");

            migrationBuilder.AlterColumn<string>(
                name: "SentToUserId",
                table: "InternalMessages",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AlterColumn<string>(
                name: "Message",
                table: "InternalMessages",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(400)",
                oldMaxLength: 400);

            migrationBuilder.AddForeignKey(
                name: "FK_InternalMessages_AspNetUsers_SentToUserId",
                table: "InternalMessages",
                column: "SentToUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }
    }
}
