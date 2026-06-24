using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CRUD.Migrations
{
    /// <inheritdoc />
    public partial class AddClassTeacherToGrade : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ClassTeacherId",
                table: "Grades",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Grades_ClassTeacherId",
                table: "Grades",
                column: "ClassTeacherId");

            migrationBuilder.AddForeignKey(
                name: "FK_Grades_Teachers_ClassTeacherId",
                table: "Grades",
                column: "ClassTeacherId",
                principalTable: "Teachers",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Grades_Teachers_ClassTeacherId",
                table: "Grades");

            migrationBuilder.DropIndex(
                name: "IX_Grades_ClassTeacherId",
                table: "Grades");

            migrationBuilder.DropColumn(
                name: "ClassTeacherId",
                table: "Grades");
        }
    }
}
