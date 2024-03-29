using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Order.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class Orderupdate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "OrderMessages",
                table: "Orders",
                newName: "OrderMessagesGuid");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "OrderMessagesGuid",
                table: "Orders",
                newName: "OrderMessages");
        }
    }
}
