using Api.Features.Presentations;
using Api.Features.Presentations.Commands;
using Api.Features.Presentations.Domain.Entities;
using Api.Features.Slides;
using Api.Features.Slides.Domain.Entities;
using Api.Features.Slides.Domain.Enums;
using Api.Shared.Exceptions;
using FluentValidation.Results;
using NSubstitute;
using System.Linq.Expressions;
using System.Text.Json;
using static Api.Features.Presentations.Commands.AddSlideCommand;

namespace Features.Tests.Features.Presentations.Commands;

public class AddSlideCommandHandlerTests
{
    private readonly ISlideRepository _slideRepository;
    private readonly IPresentationRepository _presentationRepository;
    private readonly AddSlideCommandHandler _handler;
    private readonly AddSlideCommandValidator _addSlideCommandValidator;

    public AddSlideCommandHandlerTests()
    {
        _slideRepository = Substitute.For<ISlideRepository>();
        _presentationRepository = Substitute.For<IPresentationRepository>();
        _addSlideCommandValidator = new AddSlideCommandValidator();

        _handler = new AddSlideCommandHandler(_slideRepository, _presentationRepository);
    }

    [Fact]
    public async Task Handle_Should_Add_Slide_When_Presentation_Exists()
    {
        // Arrange
        var presentationId = Guid.NewGuid().ToString();
        var addSlideDto = new AddSlideDto(SlideType.MultipleChoice, 1, JsonDocument.Parse("{\"question\": \"What is your favorite color?\"}"));

        var command = new AddSlideCommand
        {
            PresentationId = presentationId,
            Slide = addSlideDto
        };

        var presentation = new Presentation { Id = Guid.Parse(presentationId), Name = "My First Presentation" };

        _presentationRepository.GetAsync(
            Arg.Any<Expression<Func<Presentation, bool>>>(),
            null, false, true, Arg.Any<CancellationToken>()).Returns(presentation);

        // Act
        await _handler.Handle(command, CancellationToken.None);

        // Assert
        await _slideRepository.Received(1).AddAsync(Arg.Is<Slide>(s =>
            s.PresentationId == presentation.Id &&
            s.Type == SlideType.MultipleChoice &&
            s.Order == 1 &&
            s.Visible &&
            s.Content == addSlideDto.Content
        ));
    }

    [Fact]
    public async Task Handle_Should_Throw_NotFoundException_When_Presentation_Does_Not_Exist()
    {
        // Arrange
        var presentationId = Guid.NewGuid().ToString();
        var addSlideDto = new AddSlideDto(SlideType.MultipleChoice, 1, JsonDocument.Parse("{\"question\": \"What is your favorite color?\"}"));

        var command = new AddSlideCommand
        {
            PresentationId = presentationId,
            Slide = addSlideDto
        };

        _presentationRepository.GetAsync(
            Arg.Any<Expression<Func<Presentation, bool>>>(),
            null, false, true, Arg.Any<CancellationToken>()).Returns((Presentation?)null);

        // Act & Assert
        var exception = await Assert.ThrowsAsync<NotFoundException>(() => _handler.Handle(command, CancellationToken.None));
        Assert.Equal("Presentation not found.", exception.Message);

        // Ensure slide is not added
        await _slideRepository.DidNotReceive().AddAsync(Arg.Any<Slide>());
    }

    [Fact]
    public void Handle_ShouldThrowException_WhenInvalidTitle()
    {
        // Arrange
        var presentationId = Guid.NewGuid().ToString();
        var addSlideDto = new AddSlideDto(default, 1, JsonDocument.Parse("{\"question\": \"What is your favorite color?\"}"));

        var command = new AddSlideCommand
        {
            PresentationId = presentationId,
            Slide = addSlideDto
        };

        // Act
        ValidationFailure? result = _addSlideCommandValidator
            .Validate(command)
            .Errors
            .FirstOrDefault(x => x.PropertyName == "Slide.Type" && x.ErrorCode == "NotEmptyValidator");

        // Assert
        Assert.Equal("NotEmptyValidator", result?.ErrorCode);
    }
}
