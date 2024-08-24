using Api.Shared.Entities;

namespace Api.Features.Users.Domain.Entities;

public class User : Entity<Guid>
{
    public required string FirstName { get; set; }
    public required string LastName { get; set; }
    public required string Email { get; set; }
    public bool Status { get; set; }
}
