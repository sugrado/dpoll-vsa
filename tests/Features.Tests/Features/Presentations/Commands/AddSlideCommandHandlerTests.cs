using Api.Features.Presentations.Commands;
using Api.Features.Slides.Domain.Entities;
using Api.Features.Slides.Domain.Enums;
using Api.Features.Slides.Infrastructure.Persistence;
using FluentValidation.Results;
using Moq;
using static Api.Features.Presentations.Commands.AddSlideCommand;

namespace Features.Tests.Features.Presentations.Commands;

public class AddSlideCommandHandlerTests
{
    private readonly Mock<ISlideRepository> _slideRepositoryMock;
    private readonly Mock<IMultipleChoiceRepository> _multipleChoiceRepositoryMock;
    private readonly AddSlideCommandHandler _handler;
    private readonly AddSlideCommandValidator _addSlideCommandValidator;

    public AddSlideCommandHandlerTests()
    {
        _slideRepositoryMock = new Mock<ISlideRepository>();
        _multipleChoiceRepositoryMock = new Mock<IMultipleChoiceRepository>();
        _addSlideCommandValidator = new AddSlideCommandValidator();
        _handler = new AddSlideCommandHandler(_slideRepositoryMock.Object, _multipleChoiceRepositoryMock.Object);
    }

    [Fact]
    public async Task Handle_ShouldCreateMultipleChoiceSlide_WhenMultipleChoiceSlideIsGiven()
    {
        // Arrange
        var command = new AddSlideCommand
        {
            PresentationId = Guid.NewGuid().ToString(),
            Slide = new AddSlideDto(
                SlideType.MultipleChoice,
                1,
                new AddSlideMultipleChoiceDto("Test Title", ["Choice1", "Choice2"])
            )
        };

        // Act
        await _handler.Handle(command, CancellationToken.None);

        // Assert
        _slideRepositoryMock.Verify(r => r.AddAsync(It.IsAny<Slide>()), Times.Once);
        _multipleChoiceRepositoryMock.Verify(r => r.AddAsync(It.Is<MultipleChoice>(mc =>
            mc.Title == command.Slide.MultipleChoice!.Title &&
            mc.Options!.Count == 2 &&
            mc.Options.All(o => o.Value == "Choice1" || o.Value == "Choice2")
        )), Times.Once);
    }

    [Fact]
    public void Handle_ShouldThrowException_WhenInvalidTitle()
    {
        // Arrange
        var presentationId = Guid.NewGuid().ToString();
        var addSlideDto = new AddSlideDto(SlideType.MultipleChoice, 1, new AddSlideMultipleChoiceDto("a", ["Choice1", "Choice2"]));
        var command = new AddSlideCommand
        {
            PresentationId = presentationId,
            Slide = addSlideDto
        };

        // Act
        ValidationFailure? result = _addSlideCommandValidator
            .Validate(command)
            .Errors
            .FirstOrDefault(x => x.PropertyName == "Slide.MultipleChoice.Title" && x.ErrorCode == "MinimumLengthValidator");

        // Assert
        Assert.Equal("MinimumLengthValidator", result?.ErrorCode);
    }
}
