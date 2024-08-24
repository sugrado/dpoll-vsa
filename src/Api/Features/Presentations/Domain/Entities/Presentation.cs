using Api.Features.Slides.Domain.Entities;
using Api.Features.Users.Domain.Entities;
using Api.Shared.Entities;

namespace Api.Features.Presentations.Domain.Entities;

public class Presentation : Entity<Guid>
{
    public Guid UserId { get; set; }
    public required string Name { get; set; }
    public User? User { get; set; }
    public ICollection<Slide>? Slides { get; set; }
}
