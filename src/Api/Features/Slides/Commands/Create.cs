using Api.Features.Slides.Domain.Entities;
using Api.Features.Slides.Domain.Enums;
using Api.Shared;
using Api.Shared.Behaviors.Transaction;
using Api.Shared.Extensions;
using Api.Shared.Persistence.Contexts;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Api.Features.Slides.Commands;

public partial class SlidesController : ApiControllerBase
{
    [HttpPost("/api/slides")]
    public async Task<IActionResult> Create(CreateCommand command)
    {
        return Ok(await Mediator.Send(command));
    }
}

public record CreateCommand(SlideType Type, string PresentationId, short Order, string[] Choices) : IRequest<CreatedSlideResponse>, ITransactionalRequest;
public record CreatedSlideResponse(SlideType Type, string PresentationId, short Order);

internal sealed class CreateCommandValidator : AbstractValidator<CreateCommand>
{
    public CreateCommandValidator()
    {
        RuleFor(v => v.PresentationId).NotNull().NotEmpty().IsGuid();
        RuleFor(v => v.Type).NotNull().NotEmpty();
        RuleFor(v => v.Order).NotNull();
    }
}

internal sealed class CreateCommandHandler(BaseDbContext baseDbContext) : IRequestHandler<CreateCommand, CreatedSlideResponse>
{
    public async Task<CreatedSlideResponse> Handle(CreateCommand request, CancellationToken cancellationToken)
    {
        Slide slide = new()
        {
            PresentationId = Guid.Parse(request.PresentationId),
            Type = request.Type,
            Order = request.Order
        };

        await baseDbContext.Slides.AddAsync(slide, cancellationToken);

        switch (request.Type)
        {
            case SlideType.MultipleChoice:
                await CreateMultipleChoiceSlide(slide, request, cancellationToken);
                break;
            case SlideType.Video:
                await CreateVideoSlide(slide, request, cancellationToken);
                break;
        }
        return new(request.Type, request.PresentationId, request.Order);
    }

    private async Task CreateMultipleChoiceSlide(Slide slide, CreateCommand request, CancellationToken cancellationToken)
    {
        List<MultipleChoiceOption> options = request
            .Choices
            .Select(p => new MultipleChoiceOption { Value = p })
            .ToList();

        MultipleChoice multipleChoice = new()
        {
            Slide = slide,
            Options = options
        };

        await baseDbContext.MultipleChoices.AddAsync(multipleChoice, cancellationToken);
        await baseDbContext.SaveChangesAsync(cancellationToken);
    }

    private async Task CreateVideoSlide(Slide slide, CreateCommand request, CancellationToken cancellationToken)
    {
        Video video = new()
        {
            Slide = slide
        };

        await baseDbContext.Videos.AddAsync(video, cancellationToken);
        await baseDbContext.SaveChangesAsync(cancellationToken);
    }
}