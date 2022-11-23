using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AOMForum.Data.Migrations
{
    public partial class AlterVotes : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CommentVotes_AspNetUsers_AuthorId",
                table: "CommentVotes");

            migrationBuilder.AlterColumn<string>(
                name: "Content",
                table: "Settings",
                type: "nvarchar(3000)",
                maxLength: 3000,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldMaxLength: 10000);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedOn",
                table: "PostVotes",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "PostVotes",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AlterColumn<string>(
                name: "Content",
                table: "Posts",
                type: "nvarchar(3000)",
                maxLength: 3000,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldMaxLength: 10000,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "AuthorId",
                table: "CommentVotes",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedOn",
                table: "CommentVotes",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "CommentVotes",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AlterColumn<string>(
                name: "Content",
                table: "Comments",
                type: "nvarchar(3000)",
                maxLength: 3000,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldMaxLength: 10000);

            migrationBuilder.CreateIndex(
                name: "IX_PostVotes_IsDeleted",
                table: "PostVotes",
                column: "IsDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_CommentVotes_IsDeleted",
                table: "CommentVotes",
                column: "IsDeleted");

            migrationBuilder.AddForeignKey(
                name: "FK_CommentVotes_AspNetUsers_AuthorId",
                table: "CommentVotes",
                column: "AuthorId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CommentVotes_AspNetUsers_AuthorId",
                table: "CommentVotes");

            migrationBuilder.DropIndex(
                name: "IX_PostVotes_IsDeleted",
                table: "PostVotes");

            migrationBuilder.DropIndex(
                name: "IX_CommentVotes_IsDeleted",
                table: "CommentVotes");

            migrationBuilder.DropColumn(
                name: "DeletedOn",
                table: "PostVotes");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "PostVotes");

            migrationBuilder.DropColumn(
                name: "DeletedOn",
                table: "CommentVotes");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "CommentVotes");

            migrationBuilder.AlterColumn<string>(
                name: "Content",
                table: "Settings",
                type: "nvarchar(max)",
                maxLength: 10000,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(3000)",
                oldMaxLength: 3000);

            migrationBuilder.AlterColumn<string>(
                name: "Content",
                table: "Posts",
                type: "nvarchar(max)",
                maxLength: 10000,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(3000)",
                oldMaxLength: 3000,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "AuthorId",
                table: "CommentVotes",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AlterColumn<string>(
                name: "Content",
                table: "Comments",
                type: "nvarchar(max)",
                maxLength: 10000,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(3000)",
                oldMaxLength: 3000);

            migrationBuilder.AddForeignKey(
                name: "FK_CommentVotes_AspNetUsers_AuthorId",
                table: "CommentVotes",
                column: "AuthorId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }
    }
}
