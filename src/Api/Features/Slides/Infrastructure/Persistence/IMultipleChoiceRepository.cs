using Api.Features.Slides.Domain.Entities;
using Api.Shared.Persistence;

namespace Api.Features.Slides.Infrastructure.Persistence;

public interface IMultipleChoiceRepository : IRepository<MultipleChoice, Guid>
{

}