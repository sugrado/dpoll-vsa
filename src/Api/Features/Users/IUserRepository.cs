using Api.Features.Users.Domain.Entities;
using Api.Shared.Persistence;

namespace Api.Features.Users;

public interface IUserRepository : IRepository<User, Guid>
{
}
