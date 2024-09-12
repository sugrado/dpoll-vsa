using Api.Features.Slides.Domain.Entities;
using Api.Shared.Persistence;

namespace Api.Features.Slides;

public interface ISlideRepository : IRepository<Slide, Guid>
{
}
