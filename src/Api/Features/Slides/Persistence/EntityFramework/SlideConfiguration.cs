using Api.Features.Slides.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Api.Features.Slides.Persistence.EntityFramework;

public class SlideConfiguration : IEntityTypeConfiguration<Slide>
{
    public void Configure(EntityTypeBuilder<Slide> builder)
    {
        builder.ToTable("slides").HasKey(d => d.Id);

        builder.Property(d => d.Id).HasColumnName("id");
        builder.Property(d => d.PresentationId).HasColumnName("presentation_id");
        builder.Property(d => d.Type).HasColumnName("type").IsRequired();
        builder.Property(d => d.Order).HasColumnName("order").IsRequired();
        builder.Property(d => d.Visible).HasColumnName("visible");
        builder.Property(p => p.Content).HasColumnType("jsonb").IsRequired();
        builder.Property(d => d.CreatedAt).HasColumnName("created_at");
        builder.Property(d => d.UpdatedAt).HasColumnName("updated_at");
        builder.Property(d => d.DeletedAt).HasColumnName("deleted_at");

        builder.HasQueryFilter(d => !d.DeletedAt.HasValue);

        builder.HasOne(p => p.Presentation).WithMany(p => p.Slides).HasForeignKey(p => p.PresentationId);
    }
}
