using Api.Features.Presentations.Domain.Entities;
using Api.Features.Slides.Domain.Enums;
using Api.Shared.Entities;
using System.Text.Json;

namespace Api.Features.Slides.Domain.Entities;

public class Slide : Entity<Guid>
{
    public Guid PresentationId { get; set; }
    public SlideType Type { get; set; }
    public short Order { get; set; }
    public bool Visible { get; set; }
    public required JsonDocument Content { get; set; }
    public Presentation? Presentation { get; set; }
}
