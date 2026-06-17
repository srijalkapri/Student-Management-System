using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using CRUD.Models;

namespace CRUD.Data.Configurations
{
    public class StudentConfiguration: IEntityTypeConfiguration<Student>
    {

    public void Configure(EntityTypeBuilder<Student> builder)
        {
            builder.ToTable("Students");
            builder.HasKey(s => s.Id);


            builder.Property(s => s.Name)
                .IsRequired()
                .HasMaxLength(100);
            
            builder.Property(s => s.Grade)
                .IsRequired()
                .HasMaxLength(10);

            builder.HasOne(s => s.Teacher)
                .WithMany(t => t.Students)
                .HasForeignKey(s => s.TeacherId)
                .OnDelete(DeleteBehavior.SetNull);
        }
    }
}
