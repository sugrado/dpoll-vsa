using Api.Features.Slides.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Api.Features.Slides.Infrastructure.Persistence;

public class VideoConfiguration : IEntityTypeConfiguration<Video>
{
    public void Configure(EntityTypeBuilder<Video> builder)
    {
        builder.ToTable("videos").HasKey(d => d.Id);

        builder.Property(d => d.Id).HasColumnName("id");
        builder.Property(d => d.Title).HasColumnName("title");
        builder.Property(d => d.Url).HasColumnName("url");
        builder.Property(d => d.CreatedAt).HasColumnName("created_at");
        builder.Property(d => d.UpdatedAt).HasColumnName("updated_at");
        builder.Property(d => d.DeletedAt).HasColumnName("deleted_at");

        builder.HasQueryFilter(d => !d.DeletedAt.HasValue);

        builder.HasOne(p => p.Slide).WithOne(p => p.Video).HasForeignKey<Video>(p => p.Id);
    }
}
