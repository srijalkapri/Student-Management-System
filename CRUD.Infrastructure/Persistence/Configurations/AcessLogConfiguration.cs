using CRUD.Domain.Models; 
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CRUD.Infrastructure.Persistence.Configurations
{
    public class AccessLogConfiguration : IEntityTypeConfiguration<AcessLog>
    {
        public void Configure(EntityTypeBuilder<AcessLog> builder)
        {
            builder.ToTable("AccessLogs");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.ApiPath)
                .HasMaxLength(500)
                .IsRequired();

            builder.Property(x => x.HttpMethod)
                .HasMaxLength(10)
                .IsRequired();

            builder.Property(x => x.UserName)
                .HasMaxLength(100);

            builder.Property(x => x.UserId)
                .IsRequired(false);

            builder.HasIndex(x => x.CreatedAtUtc);
            builder.HasIndex(x => x.UserId);
        }
    }
}