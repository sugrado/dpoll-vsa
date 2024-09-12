using Api.Features.Presentations.Domain.Entities;
using Api.Shared.Persistence;

namespace Api.Features.Presentations;

public interface IPresentationRepository : IRepository<Presentation, Guid>
{
}
