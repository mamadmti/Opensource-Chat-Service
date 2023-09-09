using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Chat.Migrations
{
    /// <inheritdoc />
    public partial class databaseredesign2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ChatHistories_Roles_GroupsId",
                table: "ChatHistories");

            migrationBuilder.DropForeignKey(
                name: "FK_ChatHistories_Users_UsersId",
                table: "ChatHistories");

            migrationBuilder.DropForeignKey(
                name: "FK_UserRoles_Roles_GroupsId",
                table: "UserRoles");

            migrationBuilder.DropForeignKey(
                name: "FK_UserRoles_Users_UsersId",
                table: "UserRoles");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UserRoles",
                table: "UserRoles");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Roles",
                table: "Roles");

            migrationBuilder.RenameTable(
                name: "UserRoles",
                newName: "UserGroups");

            migrationBuilder.RenameTable(
                name: "Roles",
                newName: "Groups");

            migrationBuilder.RenameColumn(
                name: "UsersId",
                table: "ChatHistories",
                newName: "ToUserId");

            migrationBuilder.RenameColumn(
                name: "GroupsId",
                table: "ChatHistories",
                newName: "ToRoleId");

            migrationBuilder.RenameIndex(
                name: "IX_ChatHistories_UsersId",
                table: "ChatHistories",
                newName: "IX_ChatHistories_ToUserId");

            migrationBuilder.RenameIndex(
                name: "IX_ChatHistories_GroupsId",
                table: "ChatHistories",
                newName: "IX_ChatHistories_ToRoleId");

            migrationBuilder.RenameIndex(
                name: "IX_UserRoles_UsersId",
                table: "UserGroups",
                newName: "IX_UserGroups_UsersId");

            migrationBuilder.RenameIndex(
                name: "IX_UserRoles_GroupsId",
                table: "UserGroups",
                newName: "IX_UserGroups_GroupsId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserGroups",
                table: "UserGroups",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Groups",
                table: "Groups",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ChatHistories_Groups_ToRoleId",
                table: "ChatHistories",
                column: "ToRoleId",
                principalTable: "Groups",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ChatHistories_Users_ToUserId",
                table: "ChatHistories",
                column: "ToUserId",
                principalTable: "Users",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_UserGroups_Groups_GroupsId",
                table: "UserGroups",
                column: "GroupsId",
                principalTable: "Groups",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserGroups_Users_UsersId",
                table: "UserGroups",
                column: "UsersId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ChatHistories_Groups_ToRoleId",
                table: "ChatHistories");

            migrationBuilder.DropForeignKey(
                name: "FK_ChatHistories_Users_ToUserId",
                table: "ChatHistories");

            migrationBuilder.DropForeignKey(
                name: "FK_UserGroups_Groups_GroupsId",
                table: "UserGroups");

            migrationBuilder.DropForeignKey(
                name: "FK_UserGroups_Users_UsersId",
                table: "UserGroups");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UserGroups",
                table: "UserGroups");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Groups",
                table: "Groups");

            migrationBuilder.RenameTable(
                name: "UserGroups",
                newName: "UserRoles");

            migrationBuilder.RenameTable(
                name: "Groups",
                newName: "Roles");

            migrationBuilder.RenameColumn(
                name: "ToUserId",
                table: "ChatHistories",
                newName: "UsersId");

            migrationBuilder.RenameColumn(
                name: "ToRoleId",
                table: "ChatHistories",
                newName: "GroupsId");

            migrationBuilder.RenameIndex(
                name: "IX_ChatHistories_ToUserId",
                table: "ChatHistories",
                newName: "IX_ChatHistories_UsersId");

            migrationBuilder.RenameIndex(
                name: "IX_ChatHistories_ToRoleId",
                table: "ChatHistories",
                newName: "IX_ChatHistories_GroupsId");

            migrationBuilder.RenameIndex(
                name: "IX_UserGroups_UsersId",
                table: "UserRoles",
                newName: "IX_UserRoles_UsersId");

            migrationBuilder.RenameIndex(
                name: "IX_UserGroups_GroupsId",
                table: "UserRoles",
                newName: "IX_UserRoles_GroupsId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserRoles",
                table: "UserRoles",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Roles",
                table: "Roles",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ChatHistories_Roles_GroupsId",
                table: "ChatHistories",
                column: "GroupsId",
                principalTable: "Roles",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ChatHistories_Users_UsersId",
                table: "ChatHistories",
                column: "UsersId",
                principalTable: "Users",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_UserRoles_Roles_GroupsId",
                table: "UserRoles",
                column: "GroupsId",
                principalTable: "Roles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserRoles_Users_UsersId",
                table: "UserRoles",
                column: "UsersId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
