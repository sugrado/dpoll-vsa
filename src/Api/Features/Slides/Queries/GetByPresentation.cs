using Api.Features.Presentations;
using Api.Features.Presentations.Domain.Entities;
using Api.Features.Slides.Domain.Entities;
using Api.Features.Slides.Domain.Enums;
using Api.Shared;
using Api.Shared.Exceptions;
using Api.Shared.Extensions;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace Api.Features.Slides.Queries;

public partial class PresentationsController : ApiControllerBase
{
    [HttpGet("/api/presentations/{presentationId}/slides")]
    public async Task<IActionResult> GetByPresentation([FromRoute] string presentationId)
    {
        var res = await Mediator.Send(new GetByPresentationQuery(presentationId));
        return Ok(res);
    }
}

public record GetByPresentationQuery(string PresentationId) : IRequest<IEnumerable<GetByPresentationResponse>>;

public class GetByPresentationQueryValidator : AbstractValidator<GetByPresentationQuery>
{
    public GetByPresentationQueryValidator()
    {
        RuleFor(v => v.PresentationId).NotNull().NotEmpty().IsGuid();
    }
}

internal sealed class GetByPresentationQueryHandler(ISlideRepository slideRepository, IPresentationRepository presentationRepository)
    : IRequestHandler<GetByPresentationQuery, IEnumerable<GetByPresentationResponse>>
{
    public async Task<IEnumerable<GetByPresentationResponse>> Handle(GetByPresentationQuery request, CancellationToken cancellationToken)
    {
        Presentation? presentation = await presentationRepository.GetAsync(p => p.Id.Equals(Guid.Parse(request.PresentationId)), cancellationToken: cancellationToken);
        if (presentation is null)
        {
            throw new NotFoundException($"Presentation with id {request.PresentationId} not found.");
        }

        IEnumerable<Slide> slides = await slideRepository.GetListAsync(p => p.Id == Guid.Parse(request.PresentationId), cancellationToken: cancellationToken);
        return slides.Select(s => new GetByPresentationResponse
        {
            Type = s.Type,
            Order = s.Order,
            Visible = s.Visible,
            Content = s.Content
        });
    }
}

public class GetByPresentationResponse
{
    public SlideType Type { get; set; }
    public short Order { get; set; }
    public bool Visible { get; set; }
    public required JsonDocument Content { get; set; }
}