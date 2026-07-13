using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CRUD.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddUserIdToTeacherAndStudent : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "UserId",
                table: "Teachers",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "UserId",
                table: "Students",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Teachers_UserId_Active",
                table: "Teachers",
                column: "UserId",
                unique: true,
                filter: "\"UserId\" IS NOT NULL AND \"IsDeleted\" = false");

            migrationBuilder.CreateIndex(
                name: "IX_Students_UserId_Active",
                table: "Students",
                column: "UserId",
                unique: true,
                filter: "\"UserId\" IS NOT NULL AND \"IsDeleted\" = false");

            migrationBuilder.AddForeignKey(
                name: "FK_Students_Users_UserId",
                table: "Students",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_Teachers_Users_UserId",
                table: "Teachers",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Students_Users_UserId",
                table: "Students");

            migrationBuilder.DropForeignKey(
                name: "FK_Teachers_Users_UserId",
                table: "Teachers");

            migrationBuilder.DropIndex(
                name: "IX_Teachers_UserId_Active",
                table: "Teachers");

            migrationBuilder.DropIndex(
                name: "IX_Students_UserId_Active",
                table: "Students");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Teachers");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Students");
        }
    }
}
