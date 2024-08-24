using Api.Features.Presentations.Domain.Entities;
using Api.Features.Slides.Domain.Entities;
using Api.Features.Users.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Reflection;
namespace Api.Shared.Persistence.Contexts;

public partial class BaseDbContext(DbContextOptions<BaseDbContext> options) : DbContext(options)
{
    public DbSet<Presentation> Presentations { get; set; }
    public DbSet<Slide> Slides { get; set; }
    public DbSet<MultipleChoice> MultipleChoices { get; set; }
    public DbSet<MultipleChoiceOption> MultipleChoiceOptions { get; set; }
    public DbSet<Video> Videos { get; set; }
    public DbSet<User> Users { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }
}
