using Api.Features.Presentations.Domain.Entities;
using Api.Shared.Persistence.Contexts;

namespace Api.Features.Presentations.Infrastructure.Persistence.Implementations;

public class EfPresentationRepository(BaseDbContext context) : EfRepository<Presentation, Guid>(context), IPresentationRepository
{
}
