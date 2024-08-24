using Api.Features.Slides.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Api.Features.Slides.Infrastructure.Persistence;

public class SlideConfiguration : IEntityTypeConfiguration<Slide>
{
    public void Configure(EntityTypeBuilder<Slide> builder)
    {
        builder.ToTable("slides").HasKey(d => d.Id);

        builder.Property(d => d.Id).HasColumnName("id");
        builder.Property(d => d.PresentationId).HasColumnName("presentation_id");
        builder.Property(d => d.Type).HasColumnName("type");
        builder.Property(d => d.Order).HasColumnName("order");
        builder.Property(d => d.CreatedAt).HasColumnName("created_at");
        builder.Property(d => d.UpdatedAt).HasColumnName("updated_at");
        builder.Property(d => d.DeletedAt).HasColumnName("deleted_at");

        builder.HasQueryFilter(d => !d.DeletedAt.HasValue);

        builder.HasOne(p => p.Presentation).WithMany(p => p.Slides).HasForeignKey(p => p.PresentationId);
        builder.HasOne(p => p.MultipleChoice).WithOne(p => p.Slide).HasForeignKey<MultipleChoice>(p => p.Id);
        builder.HasOne(p => p.Video).WithOne(p => p.Slide).HasForeignKey<Video>(p => p.Id);
    }
}
