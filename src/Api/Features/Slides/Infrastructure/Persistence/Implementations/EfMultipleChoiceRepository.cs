using Api.Features.Slides.Domain.Entities;
using Api.Shared.Persistence.Contexts;

namespace Api.Features.Slides.Infrastructure.Persistence.Implementations;

public class EfMultipleChoiceRepository(BaseDbContext context) : EfRepository<MultipleChoice, Guid>(context), IMultipleChoiceRepository
{
}
