using Api.Features.Slides.Domain.Entities;
using Api.Shared.Persistence.Contexts;

namespace Api.Features.Slides.Persistence.EntityFramework;

public class EfSlideRepository(BaseDbContext context) : EfRepository<Slide, Guid>(context), ISlideRepository
{
}
