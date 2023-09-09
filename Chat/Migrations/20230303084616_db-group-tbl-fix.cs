using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Chat.Migrations
{
    /// <inheritdoc />
    public partial class dbgrouptblfix : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ChatHistories_Groups_ToRoleId",
                table: "ChatHistories");

            migrationBuilder.RenameColumn(
                name: "ToRoleId",
                table: "ChatHistories",
                newName: "ToGroupId");

            migrationBuilder.RenameIndex(
                name: "IX_ChatHistories_ToRoleId",
                table: "ChatHistories",
                newName: "IX_ChatHistories_ToGroupId");

            migrationBuilder.AddForeignKey(
                name: "FK_ChatHistories_Groups_ToGroupId",
                table: "ChatHistories",
                column: "ToGroupId",
                principalTable: "Groups",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ChatHistories_Groups_ToGroupId",
                table: "ChatHistories");

            migrationBuilder.RenameColumn(
                name: "ToGroupId",
                table: "ChatHistories",
                newName: "ToRoleId");

            migrationBuilder.RenameIndex(
                name: "IX_ChatHistories_ToGroupId",
                table: "ChatHistories",
                newName: "IX_ChatHistories_ToRoleId");

            migrationBuilder.AddForeignKey(
                name: "FK_ChatHistories_Groups_ToRoleId",
                table: "ChatHistories",
                column: "ToRoleId",
                principalTable: "Groups",
                principalColumn: "Id");
        }
    }
}
