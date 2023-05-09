using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LendStuff.Server.Data.Migrations
{
    /// <inheritdoc />
    public partial class updatedInternalMessage : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SentFromUserGuid",
                table: "InternalMessages");

            migrationBuilder.DropColumn(
                name: "SentToUserGuid",
                table: "InternalMessages");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "SentFromUserGuid",
                table: "InternalMessages",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "SentToUserGuid",
                table: "InternalMessages",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
