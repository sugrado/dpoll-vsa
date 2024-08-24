using Api.Features.Slides.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Api.Features.Slides.Infrastructure.Persistence;

public class MultipleChoiceOptionConfiguration : IEntityTypeConfiguration<MultipleChoiceOption>
{
    public void Configure(EntityTypeBuilder<MultipleChoiceOption> builder)
    {
        builder.ToTable("multiple_choice_options").HasKey(d => d.Id);

        builder.Property(d => d.Id).HasColumnName("id");
        builder.Property(d => d.Value).HasColumnName("value");
        builder.Property(d => d.MultipleChoiceId).HasColumnName("multiple_choice_id");
        builder.Property(d => d.CreatedAt).HasColumnName("created_at");
        builder.Property(d => d.UpdatedAt).HasColumnName("updated_at");
        builder.Property(d => d.DeletedAt).HasColumnName("deleted_at");

        builder.HasQueryFilter(d => !d.DeletedAt.HasValue);

        builder.HasOne(d => d.MultipleChoice).WithMany(d => d.Options).HasForeignKey(d => d.MultipleChoiceId);
    }
}
