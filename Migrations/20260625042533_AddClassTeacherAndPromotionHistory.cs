using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace CRUD.Migrations
{
    /// <inheritdoc />
    public partial class AddClassTeacherAndPromotionHistory : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PromotionHistories",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    StudentId = table.Column<int>(type: "integer", nullable: false),
                    FromGradeId = table.Column<int>(type: "integer", nullable: false),
                    ToGradeId = table.Column<int>(type: "integer", nullable: false),
                    PromotedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PromotionHistories", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PromotionHistories_Grades_FromGradeId",
                        column: x => x.FromGradeId,
                        principalTable: "Grades",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PromotionHistories_Grades_ToGradeId",
                        column: x => x.ToGradeId,
                        principalTable: "Grades",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PromotionHistories_Students_StudentId",
                        column: x => x.StudentId,
                        principalTable: "Students",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PromotionHistories_FromGradeId",
                table: "PromotionHistories",
                column: "FromGradeId");

            migrationBuilder.CreateIndex(
                name: "IX_PromotionHistories_StudentId",
                table: "PromotionHistories",
                column: "StudentId");

            migrationBuilder.CreateIndex(
                name: "IX_PromotionHistories_ToGradeId",
                table: "PromotionHistories",
                column: "ToGradeId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PromotionHistories");
        }
    }
}
