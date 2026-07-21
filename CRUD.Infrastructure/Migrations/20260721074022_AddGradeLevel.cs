using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CRUD.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddGradeLevel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Level",
                table: "Grades",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.Sql("""
                UPDATE "Grades"
                SET "Level" = CAST(NULLIF(regexp_replace("ClassName", '[^0-9]', '', 'g'), '') AS INTEGER)
                WHERE NULLIF(regexp_replace("ClassName", '[^0-9]', '', 'g'), '') IS NOT NULL;
                """);

            migrationBuilder.Sql("""
                UPDATE "Grades"
                SET "Level" = "Id"
                WHERE "Level" = 0;
                """);

            migrationBuilder.CreateIndex(
                name: "IX_Grades_Level",
                table: "Grades",
                column: "Level",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Grades_Level",
                table: "Grades");

            migrationBuilder.DropColumn(
                name: "Level",
                table: "Grades");
        }
    }
}
