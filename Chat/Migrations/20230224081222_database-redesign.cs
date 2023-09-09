using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Chat.Migrations
{
    /// <inheritdoc />
    public partial class databaseredesign : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ChatHistories_Users_UsersId",
                table: "ChatHistories");

            migrationBuilder.DropForeignKey(
                name: "FK_UserRoles_Roles_RolesId",
                table: "UserRoles");

            migrationBuilder.DropColumn(
                name: "ToRoleId",
                table: "ChatHistories");

            migrationBuilder.DropColumn(
                name: "ToUserId",
                table: "ChatHistories");

            migrationBuilder.RenameColumn(
                name: "RolesId",
                table: "UserRoles",
                newName: "GroupsId");

            migrationBuilder.RenameIndex(
                name: "IX_UserRoles_RolesId",
                table: "UserRoles",
                newName: "IX_UserRoles_GroupsId");

            migrationBuilder.RenameColumn(
                name: "RoleName",
                table: "Roles",
                newName: "GroupName");

            migrationBuilder.AlterColumn<long>(
                name: "UsersId",
                table: "ChatHistories",
                type: "bigint",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AddColumn<long>(
                name: "GroupsId",
                table: "ChatHistories",
                type: "bigint",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ChatHistories_GroupsId",
                table: "ChatHistories",
                column: "GroupsId");

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
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
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

            migrationBuilder.DropIndex(
                name: "IX_ChatHistories_GroupsId",
                table: "ChatHistories");

            migrationBuilder.DropColumn(
                name: "GroupsId",
                table: "ChatHistories");

            migrationBuilder.RenameColumn(
                name: "GroupsId",
                table: "UserRoles",
                newName: "RolesId");

            migrationBuilder.RenameIndex(
                name: "IX_UserRoles_GroupsId",
                table: "UserRoles",
                newName: "IX_UserRoles_RolesId");

            migrationBuilder.RenameColumn(
                name: "GroupName",
                table: "Roles",
                newName: "RoleName");

            migrationBuilder.AlterColumn<long>(
                name: "UsersId",
                table: "ChatHistories",
                type: "bigint",
                nullable: false,
                defaultValue: 0L,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "ToRoleId",
                table: "ChatHistories",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "ToUserId",
                table: "ChatHistories",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_ChatHistories_Users_UsersId",
                table: "ChatHistories",
                column: "UsersId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserRoles_Roles_RolesId",
                table: "UserRoles",
                column: "RolesId",
                principalTable: "Roles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
