using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Chat.Migrations
{
    /// <inheritdoc />
    public partial class modifiedUsersFields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ModifiedAt",
                table: "Users",
                newName: "VerificationCreatedAt");

            migrationBuilder.AddColumn<DateTime>(
                name: "LastTryAt",
                table: "Users",
                type: "datetime2",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LastTryAt",
                table: "Users");

            migrationBuilder.RenameColumn(
                name: "VerificationCreatedAt",
                table: "Users",
                newName: "ModifiedAt");
        }
    }
}
