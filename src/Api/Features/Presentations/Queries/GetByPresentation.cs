using Api.Features.Slides.Domain.Enums;
using Api.Shared;
using Api.Shared.Extensions;
using Api.Shared.Persistence.Contexts;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Api.Features.Presentations.Queries;

public partial class PresentationsController : ApiControllerBase
{
    [HttpGet("/api/presentations/{presentationId}/slides")]
    public async Task<IActionResult> GetByPresentation([FromRoute] string presentationId)
    {
        var res = await Mediator.Send(new GetByPresentationQuery(presentationId));
        return Ok(res);
    }
}

public record GetByPresentationQuery(string PresentationId) : IRequest<List<GetByPresentationResponse>>;

public class GetByPresentationQueryValidator : AbstractValidator<GetByPresentationQuery>
{
    public GetByPresentationQueryValidator()
    {
        RuleFor(v => v.PresentationId).NotNull().NotEmpty().IsGuid();
    }
}

internal sealed class GetByPresentationQueryHandler(BaseDbContext baseDbContext) : IRequestHandler<GetByPresentationQuery, List<GetByPresentationResponse>>
{
    public async Task<List<GetByPresentationResponse>> Handle(GetByPresentationQuery request, CancellationToken cancellationToken)
    {
        return await baseDbContext.Presentations
            .Where(p => p.Id == Guid.Parse(request.PresentationId))
            .SelectMany(p => p.Slides!)
            .Select(s => new GetByPresentationResponse
            {
                Type = s.Type,
                Video = s.Type == SlideType.Video ? new GetByPresentationVideoDto
                {
                    Title = s.Video!.Title,
                    Url = s.Video.Url
                } : null,
                MultipleChoice = s.Type == SlideType.MultipleChoice ? new GetByPresentationMultipleChoiceDto
                {
                    Title = s.MultipleChoice!.Title,
                    Choices = s.MultipleChoice.Options!.Select(o => o.Value)
                } : null
            })
            .ToListAsync(cancellationToken);
    }
}

public class GetByPresentationResponse
{
    public required string Type { get; set; }
    public GetByPresentationVideoDto? Video { get; set; }
    public GetByPresentationMultipleChoiceDto? MultipleChoice { get; set; }
}

public class GetByPresentationVideoDto
{
    public string? Title { get; set; }
    public string? Url { get; set; }
}

public class GetByPresentationMultipleChoiceDto
{
    public string? Title { get; set; }
    public IEnumerable<string>? Choices { get; set; }
}
