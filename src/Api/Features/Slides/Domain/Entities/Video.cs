using Api.Shared.Entities;

namespace Api.Features.Slides.Domain.Entities;

public class Video : Entity<Guid>
{
    public string? Title { get; set; }
    public string? Url { get; set; }
    public Slide? Slide { get; set; }
}
