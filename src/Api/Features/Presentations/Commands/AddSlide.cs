using Api.Features.Slides.Domain.Entities;
using Api.Features.Slides.Domain.Enums;
using Api.Features.Slides.Infrastructure.Persistence;
using Api.Shared;
using Api.Shared.Behaviors.Transaction;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Api.Features.Presentations.Commands;

public partial class PresentationsController : ApiControllerBase
{
    [HttpPost("/api/presentations/{presentationId}/slides")]
    public async Task<IActionResult> AddSlide([FromRoute] string presentationId, [FromBody] AddSlideDto addSlideDto)
    {
        AddSlideCommand command = new() { PresentationId = presentationId, Slide = addSlideDto };
        await Mediator.Send(command);
        return NoContent();
    }
}

public record AddSlideDto(SlideType Type, short Order, AddSlideMultipleChoiceDto? MultipleChoice);
public record AddSlideMultipleChoiceDto(string Title, string[] Choices);

public class AddSlideCommandValidator : AbstractValidator<AddSlideCommand>
{
    public AddSlideCommandValidator()
    {
        RuleFor(v => v.Slide.Type).NotNull().NotEmpty().IsInEnum();
        RuleFor(v => v.Slide.Order).NotNull().NotEmpty();
        When(p => p.Slide.Type == SlideType.MultipleChoice, () =>
        {
            RuleFor(v => v.Slide.MultipleChoice).NotNull().NotEmpty();
            RuleFor(v => v.Slide.MultipleChoice!.Title).NotNull().NotEmpty().MinimumLength(3).MaximumLength(256);
            RuleFor(v => v.Slide.MultipleChoice!.Choices).Must(p => p != null && p.Length != 0);
        });
    }
}

public class AddSlideCommand : IRequest, ITransactionalRequest
{
    public required string PresentationId { get; set; }
    public required AddSlideDto Slide { get; set; }

    public class AddSlideCommandHandler(ISlideRepository slideRepository, IMultipleChoiceRepository multipleChoiceRepository) : IRequestHandler<AddSlideCommand>
    {
        public async Task Handle(AddSlideCommand request, CancellationToken cancellationToken)
        {
            Slide slide = new()
            {
                PresentationId = Guid.Parse(request.PresentationId),
                Type = request.Slide.Type,
                Order = request.Slide.Order
            };

            await slideRepository.AddAsync(slide);

            switch (request.Slide.Type)
            {
                case SlideType.MultipleChoice:
                    await CreateMultipleChoiceSlide(slide, request.Slide.MultipleChoice!, cancellationToken);
                    break;
                case SlideType.Video:
                    //Video creation logic
                    break;
            }
        }

        private async Task CreateMultipleChoiceSlide(Slide slide, AddSlideMultipleChoiceDto multipleChoiceDto, CancellationToken cancellationToken)
        {
            List<MultipleChoiceOption> options = multipleChoiceDto
                .Choices
                .Select(p => new MultipleChoiceOption { Value = p })
                .ToList();

            MultipleChoice multipleChoice = new()
            {
                Slide = slide,
                Options = options,
                Title = multipleChoiceDto.Title
            };

            await multipleChoiceRepository.AddAsync(multipleChoice);
        }
    }
}
