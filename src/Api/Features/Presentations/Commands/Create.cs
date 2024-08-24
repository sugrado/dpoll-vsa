using Api.Features.Presentations.Domain.Entities;
using Api.Shared;
using Api.Shared.Extensions;
using Api.Shared.Persistence.Contexts;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Api.Features.Presentations.Commands;

public partial class PresentationsController : ApiControllerBase
{
    [HttpPost("/api/presentations")]
    public async Task<IActionResult> Create(CreateCommand command)
    {
        return Ok(await Mediator.Send(command));
    }
}

public record CreateCommand(string UserId, string Name) : IRequest<CreatedPresentationResponse>;
public record CreatedPresentationResponse(string UserId, string Name);

internal sealed class CreateCommandValidator : AbstractValidator<CreateCommand>
{
    public CreateCommandValidator()
    {
        RuleFor(v => v.UserId).NotNull().NotEmpty().IsGuid();
        RuleFor(v => v.Name).NotNull().NotEmpty();
    }
}

internal sealed class CreateCommandHandler(BaseDbContext baseDbContext) : IRequestHandler<CreateCommand, CreatedPresentationResponse>
{
    public async Task<CreatedPresentationResponse> Handle(CreateCommand request, CancellationToken cancellationToken)
    {
        Presentation presentation = new()
        {
            UserId = Guid.Parse(request.UserId),
            Name = request.Name
        };
        await baseDbContext.Presentations.AddAsync(presentation, cancellationToken);
        await baseDbContext.SaveChangesAsync(cancellationToken);
        return new(presentation.Name, presentation.UserId.ToString());
    }
}