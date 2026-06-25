using Jalsa.Application.Interfaces.Repositories;
using Jalsa.Domain.Models.Session;
using Jalsa.Infrastructure.Data;

namespace Jalsa.Infrastructure.Repositories;

public class SessionRepository : GenericRepository<Session>, ISessionRepository
{
    public SessionRepository(Galsa_DBDbContext context) : base(context) { }
}
