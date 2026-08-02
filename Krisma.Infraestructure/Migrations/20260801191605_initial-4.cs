using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Krisma.Infraestructure.Migrations
{
    /// <inheritdoc />
    public partial class initial4 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Developers_GitHubLogin",
                table: "Developers");

            migrationBuilder.AlterColumn<string>(
                name: "GitHubLogin",
                table: "Developers",
                type: "nvarchar(39)",
                maxLength: 39,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50);

            migrationBuilder.AddColumn<string>(
                name: "GitHubNodeId",
                table: "Developers",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<long>(
                name: "GitHubUserId",
                table: "Developers",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<Guid>(
                name: "OrganizationId",
                table: "Developers",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_Developers_OrganizationId_GitHubNodeId",
                table: "Developers",
                columns: new[] { "OrganizationId", "GitHubNodeId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Developers_OrganizationId_GitHubUserId",
                table: "Developers",
                columns: new[] { "OrganizationId", "GitHubUserId" },
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Developers_Organizations_OrganizationId",
                table: "Developers",
                column: "OrganizationId",
                principalTable: "Organizations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Developers_Organizations_OrganizationId",
                table: "Developers");

            migrationBuilder.DropIndex(
                name: "IX_Developers_OrganizationId_GitHubNodeId",
                table: "Developers");

            migrationBuilder.DropIndex(
                name: "IX_Developers_OrganizationId_GitHubUserId",
                table: "Developers");

            migrationBuilder.DropColumn(
                name: "GitHubNodeId",
                table: "Developers");

            migrationBuilder.DropColumn(
                name: "GitHubUserId",
                table: "Developers");

            migrationBuilder.DropColumn(
                name: "OrganizationId",
                table: "Developers");

            migrationBuilder.AlterColumn<string>(
                name: "GitHubLogin",
                table: "Developers",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(39)",
                oldMaxLength: 39);

            migrationBuilder.CreateIndex(
                name: "IX_Developers_GitHubLogin",
                table: "Developers",
                column: "GitHubLogin",
                unique: true);
        }
    }
}
