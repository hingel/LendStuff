using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LendStuff.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class Updatedordermodel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "OrderId",
                table: "InternalMessages",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_InternalMessages_OrderId",
                table: "InternalMessages",
                column: "OrderId");

            migrationBuilder.AddForeignKey(
                name: "FK_InternalMessages_Orders_OrderId",
                table: "InternalMessages",
                column: "OrderId",
                principalTable: "Orders",
                principalColumn: "OrderId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_InternalMessages_Orders_OrderId",
                table: "InternalMessages");

            migrationBuilder.DropIndex(
                name: "IX_InternalMessages_OrderId",
                table: "InternalMessages");

            migrationBuilder.DropColumn(
                name: "OrderId",
                table: "InternalMessages");
        }
    }
}
