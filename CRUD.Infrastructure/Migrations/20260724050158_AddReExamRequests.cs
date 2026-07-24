using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace CRUD.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddReExamRequests : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ReExamRequests",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    StudentId = table.Column<int>(type: "integer", nullable: false),
                    ExamSessionId = table.Column<int>(type: "integer", nullable: false),
                    OriginalResultItemId = table.Column<int>(type: "integer", nullable: false),
                    AttemptNumber = table.Column<int>(type: "integer", nullable: false),
                    StudentReason = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    Status = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false),
                    ReviewedByUserId = table.Column<int>(type: "integer", nullable: true),
                    ReviewedAtUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    AdminComment = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    TeacherId = table.Column<int>(type: "integer", nullable: true),
                    MarksObtained = table.Column<decimal>(type: "numeric(5,2)", precision: 5, scale: 2, nullable: true),
                    TotalMarks = table.Column<decimal>(type: "numeric(5,2)", precision: 5, scale: 2, nullable: true),
                    IsAbsent = table.Column<bool>(type: "boolean", nullable: false),
                    MarksRemarks = table.Column<string>(type: "character varying(300)", maxLength: 300, nullable: true),
                    MarksSubmittedAtUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    MarksReviewedByUserId = table.Column<int>(type: "integer", nullable: true),
                    MarksReviewedAtUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    MarksReviewComment = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    CreatedAtUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAtUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReExamRequests", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ReExamRequests_ExamResultItems_OriginalResultItemId",
                        column: x => x.OriginalResultItemId,
                        principalTable: "ExamResultItems",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ReExamRequests_ExamSessions_ExamSessionId",
                        column: x => x.ExamSessionId,
                        principalTable: "ExamSessions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ReExamRequests_Students_StudentId",
                        column: x => x.StudentId,
                        principalTable: "Students",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ReExamRequests_Teachers_TeacherId",
                        column: x => x.TeacherId,
                        principalTable: "Teachers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ReExamRequests_Users_MarksReviewedByUserId",
                        column: x => x.MarksReviewedByUserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_ReExamRequests_Users_ReviewedByUserId",
                        column: x => x.ReviewedByUserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ReExamRequests_ExamSessionId",
                table: "ReExamRequests",
                column: "ExamSessionId");

            migrationBuilder.CreateIndex(
                name: "IX_ReExamRequests_MarksReviewedByUserId",
                table: "ReExamRequests",
                column: "MarksReviewedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_ReExamRequests_OriginalResultItemId",
                table: "ReExamRequests",
                column: "OriginalResultItemId");

            migrationBuilder.CreateIndex(
                name: "IX_ReExamRequests_ReviewedByUserId",
                table: "ReExamRequests",
                column: "ReviewedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_ReExamRequests_Status",
                table: "ReExamRequests",
                column: "Status");

            migrationBuilder.CreateIndex(
                name: "IX_ReExamRequests_StudentId_ExamSessionId_AttemptNumber",
                table: "ReExamRequests",
                columns: new[] { "StudentId", "ExamSessionId", "AttemptNumber" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ReExamRequests_TeacherId",
                table: "ReExamRequests",
                column: "TeacherId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ReExamRequests");
        }
    }
}
