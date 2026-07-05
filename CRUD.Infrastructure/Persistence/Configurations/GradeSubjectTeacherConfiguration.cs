using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using CRUD.Domain.Models;

namespace CRUD.Infrastructure.Persistence.Configurations
{
    public class GradeSubjectTeacherConfiguration : IEntityTypeConfiguration<GradeSubjectTeacher>
    {
        public void Configure(EntityTypeBuilder<GradeSubjectTeacher> builder)
        {
            builder.ToTable("GradeSubjectTeachers");
            builder.HasKey(gst => gst.Id);

            // Optional: Unique constraint to prevent duplicate teacher assignments for same GradeSubject
            builder.HasIndex(gst => new { gst.GradeSubjectId, gst.TeacherId }).IsUnique();

            builder.HasOne(gst => gst.GradeSubject)
                .WithMany(gs => gs.GradeSubjectTeachers)
                .HasForeignKey(gst => gst.GradeSubjectId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(gst => gst.Teacher)
                .WithMany(t => t.GradeSubjectTeachers)
                .HasForeignKey(gst => gst.TeacherId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}