using Api.Shared.Entities;

namespace Api.Features.Slides.Domain.Entities;

public class MultipleChoice : Entity<Guid>
{
    public string? Title { get; set; }
    public ICollection<MultipleChoiceOption>? Options { get; set; }
    public Slide? Slide { get; set; }
}
