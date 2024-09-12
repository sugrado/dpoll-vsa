using Api.Features.Presentations.Domain.Entities;
using Api.Shared.Persistence.Contexts;

namespace Api.Features.Presentations.Persistence.EntityFramework;

public class EfPresentationRepository(BaseDbContext context) : EfRepository<Presentation, Guid>(context), IPresentationRepository
{
}
