using Api.Shared.Entities;

namespace Api.Features.Slides.Domain.Entities;

public class MultipleChoiceOption : Entity<Guid>
{
    public Guid MultipleChoiceId { get; set; }
    public required string Value { get; set; }
    public MultipleChoice? MultipleChoice { get; set; }
}