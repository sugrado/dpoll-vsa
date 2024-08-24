using Api.Features.Slides.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Api.Features.Slides.Infrastructure.Persistence;

public class MultipleChoiceConfiguration : IEntityTypeConfiguration<MultipleChoice>
{
    public void Configure(EntityTypeBuilder<MultipleChoice> builder)
    {
        builder.ToTable("multiple_choices").HasKey(d => d.Id);

        builder.Property(d => d.Id).HasColumnName("id");
        builder.Property(d => d.Title).HasColumnName("title");
        builder.Property(d => d.CreatedAt).HasColumnName("created_at");
        builder.Property(d => d.UpdatedAt).HasColumnName("updated_at");
        builder.Property(d => d.DeletedAt).HasColumnName("deleted_at");

        builder.HasQueryFilter(d => !d.DeletedAt.HasValue);

        builder.HasMany(d => d.Options).WithOne(d => d.MultipleChoice).HasForeignKey(d => d.MultipleChoiceId);
        builder.HasOne(p => p.Slide).WithOne(p => p.MultipleChoice).HasForeignKey<MultipleChoice>(p => p.Id);

    }
}
