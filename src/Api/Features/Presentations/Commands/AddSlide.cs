using Api.Features.Presentations.Domain.Entities;
using Api.Features.Slides;
using Api.Features.Slides.Domain.Entities;
using Api.Features.Slides.Domain.Enums;
using Api.Shared;
using Api.Shared.Behaviors.Transaction;
using Api.Shared.Exceptions;
using Api.Shared.Extensions;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace Api.Features.Presentations.Commands;

public partial class PresentationsController : ApiControllerBase
{
    [HttpPost("api/presentations/{presentationId}/slides")]
    public async Task<IActionResult> AddSlide([FromRoute] string presentationId, [FromBody] AddSlideDto addSlideDto)
    {
        AddSlideCommand command = new() { PresentationId = presentationId, Slide = addSlideDto };
        await Mediator.Send(command);
        return NoContent();
    }
}

public record AddSlideDto(SlideType Type, short Order, JsonDocument? Content);

public class AddSlideCommandValidator : AbstractValidator<AddSlideCommand>
{
    public AddSlideCommandValidator()
    {
        RuleFor(v => v.Slide.Type).NotNull().NotEmpty().IsInEnum();
        RuleFor(v => v.Slide.Order).NotNull().NotEmpty();
        RuleFor(v => v.Slide.Content).NotNull().NotEmpty();
        RuleFor(v => v.PresentationId).NotNull().NotEmpty().IsGuid();
    }
}

public class AddSlideCommand : IRequest, ITransactionalRequest
{
    public required string PresentationId { get; set; }
    public required AddSlideDto Slide { get; set; }

    public class AddSlideCommandHandler(
        ISlideRepository slideRepository,
        IPresentationRepository presentationRepository) : IRequestHandler<AddSlideCommand>
    {
        public async Task Handle(AddSlideCommand request, CancellationToken cancellationToken)
        {
            Presentation? presentation = await presentationRepository.GetAsync(p => p.Id.Equals(Guid.Parse(request.PresentationId)), cancellationToken: cancellationToken);
            if (presentation is null)
            {
                throw new NotFoundException("Presentation not found.");
            }

            Slide slide = new()
            {
                PresentationId = presentation.Id,
                Type = request.Slide.Type,
                Order = request.Slide.Order,
                Visible = true,
                Content = request.Slide.Content!
            };

            await slideRepository.AddAsync(slide);
        }
    }
}
