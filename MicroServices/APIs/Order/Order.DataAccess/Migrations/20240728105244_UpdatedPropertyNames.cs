using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Order.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class UpdatedPropertyNames : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "OrderMessagesGuid",
                table: "Orders",
                newName: "OrderMessageGuids");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "OrderMessageGuids",
                table: "Orders",
                newName: "OrderMessagesGuid");
        }
    }
}
