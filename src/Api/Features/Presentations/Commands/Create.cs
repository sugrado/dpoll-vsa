using Api.Features.Presentations.Domain.Entities;
using Api.Features.Users;
using Api.Features.Users.Domain.Entities;
using Api.Shared;
using Api.Shared.Exceptions;
using Api.Shared.Extensions;
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

public class CreateCommandValidator : AbstractValidator<CreateCommand>
{
    public CreateCommandValidator()
    {
        RuleFor(v => v.UserId).NotNull().NotEmpty().IsGuid();
        RuleFor(v => v.Name).NotNull().NotEmpty();
    }
}

public sealed class CreateCommandHandler(IPresentationRepository presentationRepository, IUserRepository userRepository)
    : IRequestHandler<CreateCommand, CreatedPresentationResponse>
{
    public async Task<CreatedPresentationResponse> Handle(CreateCommand request, CancellationToken cancellationToken)
    {
        User? user = await userRepository.GetAsync(p => p.Id.Equals(Guid.Parse(request.UserId)), cancellationToken: cancellationToken);
        if (user is null)
        {
            throw new NotFoundException("User not found.");
        }

        Presentation presentation = new()
        {
            UserId = Guid.Parse(request.UserId),
            Name = request.Name
        };
        await presentationRepository.AddAsync(presentation);
        return new(presentation.Name, presentation.UserId.ToString());
    }
}