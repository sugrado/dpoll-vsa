using Api.Features.Presentations.Domain.Entities;
using Api.Shared.Persistence;

namespace Api.Features.Presentations.Infrastructure.Persistence;

public interface IPresentationRepository : IRepository<Presentation, Guid>
{
}
