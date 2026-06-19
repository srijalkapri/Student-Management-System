using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CRUD.Migrations
{
    /// <inheritdoc />
    public partial class FixStudentGradeCasing : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Grade",
                table: "Teachers");

            migrationBuilder.RenameColumn(
                name: "grade",
                table: "Students",
                newName: "Grade");

            migrationBuilder.AddColumn<List<string>>(
                name: "Grades",
                table: "Teachers",
                type: "text[]",
                nullable: false);

            migrationBuilder.AlterColumn<int>(
                name: "TeacherId",
                table: "Students",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Grades",
                table: "Teachers");

            migrationBuilder.RenameColumn(
                name: "Grade",
                table: "Students",
                newName: "grade");

            migrationBuilder.AddColumn<string>(
                name: "Grade",
                table: "Teachers",
                type: "character varying(20)",
                maxLength: 20,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<int>(
                name: "TeacherId",
                table: "Students",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);
        }
    }
}
