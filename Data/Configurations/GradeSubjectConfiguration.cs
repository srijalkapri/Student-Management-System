using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using CRUD.Models;

namespace CRUD.Data.Configurations
{
    public class GradeSubjectConfiguration : IEntityTypeConfiguration<GradeSubject>
    {
        public void Configure(EntityTypeBuilder<GradeSubject> builder)
        {
            builder.ToTable("GradeSubjects");
            builder.HasKey(gs => gs.Id);

            // Unique constraint: A grade can't have the same subject twice

            builder.HasIndex(gs => new { gs.GradeId, gs.SubjectId }).IsUnique();

            builder.HasOne(gs => gs.Grade)
                .WithMany(g => g.GradeSubjects)
                .HasForeignKey(gs => gs.GradeId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(gs => gs.Subject)
                .WithMany(s => s.GradeSubjects)
                .HasForeignKey(gs => gs.SubjectId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Property(gs => gs.IsOptional)
                .IsRequired()
                .HasDefaultValue(false);
        }
    }
}
