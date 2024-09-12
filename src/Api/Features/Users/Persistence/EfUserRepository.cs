using Api.Features.Users.Domain.Entities;
using Api.Shared.Persistence.Contexts;

namespace Api.Features.Users.Persistence;

public class EfUserRepository(BaseDbContext context) : EfRepository<User, Guid>(context), IUserRepository
{
}
