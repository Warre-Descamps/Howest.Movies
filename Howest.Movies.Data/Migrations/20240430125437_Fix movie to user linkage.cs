using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Howest.Movies.Data.Migrations
{
    /// <inheritdoc />
    public partial class Fixmovietouserlinkage : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Movies_Users_AddedByUserId",
                table: "Movies");

            migrationBuilder.DropIndex(
                name: "IX_Movies_AddedByUserId",
                table: "Movies");

            migrationBuilder.DropColumn(
                name: "AddedByUserId",
                table: "Movies");

            migrationBuilder.CreateIndex(
                name: "IX_Movies_AddedById",
                table: "Movies",
                column: "AddedById");

            migrationBuilder.AddForeignKey(
                name: "FK_Movies_Users_AddedById",
                table: "Movies",
                column: "AddedById",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Movies_Users_AddedById",
                table: "Movies");

            migrationBuilder.DropIndex(
                name: "IX_Movies_AddedById",
                table: "Movies");

            migrationBuilder.AddColumn<Guid>(
                name: "AddedByUserId",
                table: "Movies",
                type: "char(36)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Movies_AddedByUserId",
                table: "Movies",
                column: "AddedByUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Movies_Users_AddedByUserId",
                table: "Movies",
                column: "AddedByUserId",
                principalTable: "Users",
                principalColumn: "Id");
        }
    }
}
