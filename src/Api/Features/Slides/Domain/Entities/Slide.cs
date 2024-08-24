using Api.Features.Presentations.Domain.Entities;
using Api.Features.Slides.Domain.Enums;
using Api.Shared.Entities;

namespace Api.Features.Slides.Domain.Entities;

public class Slide : Entity<Guid>
{
    public Guid PresentationId { get; set; }
    public SlideType Type { get; set; }
    public short Order { get; set; }
    public Presentation? Presentation { get; set; }
    public MultipleChoice? MultipleChoice { get; set; }
    public Video? Video { get; set; }
}
