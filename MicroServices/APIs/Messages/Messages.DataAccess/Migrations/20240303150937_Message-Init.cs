using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Messages.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class MessageInit : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "InternalMessages",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Message = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    MessageSent = table.Column<DateTime>(type: "datetime2", nullable: false),
                    SentToUserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SentFromUserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    IsRead = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InternalMessages", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "InternalMessages");
        }
    }
}
