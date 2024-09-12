using Api.Features.Users.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Api.Features.Users.Persistence;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable("users").HasKey(u => u.Id);

        builder.Property(u => u.Id).HasColumnName("id").IsRequired();
        builder.Property(u => u.FirstName).HasColumnName("first_name").IsRequired().HasMaxLength(64);
        builder.Property(u => u.LastName).HasColumnName("last_name").IsRequired().HasMaxLength(64);
        builder.Property(u => u.Email).HasColumnName("email").IsRequired().HasMaxLength(64);
        builder.Property(u => u.Status).HasColumnName("status");
        builder.Property(u => u.CreatedAt).HasColumnName("created_at").IsRequired();
        builder.Property(u => u.UpdatedAt).HasColumnName("updated_at");
        builder.Property(u => u.DeletedAt).HasColumnName("deleted_at");

        builder.HasQueryFilter(u => !u.DeletedAt.HasValue);

        builder.HasData(GetSeeds());
    }

    private IEnumerable<User> GetSeeds()
    {
        List<User> users =
            new()
            {
                new()
                {
                    Id = Guid.NewGuid(),
                    FirstName = "Görkem Rıdvan",
                    LastName = "ARIK",
                    Email = "admin@admin.com",
                    Status = true,
                    CreatedAt = new DateTime(2021, 8, 30, 20, 30, 00,DateTimeKind.Utc),
                }
            };
        return users;
    }
}
