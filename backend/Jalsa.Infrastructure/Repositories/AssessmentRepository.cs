using Jalsa.Application.Interfaces.Repositories;
using Jalsa.Domain.Models.Assessment;
using Jalsa.Infrastructure.Data;

namespace Jalsa.Infrastructure.Repositories;

public class AssessmentRepository : GenericRepository<Assessment>, IAssessmentRepository
{
    public AssessmentRepository(Galsa_DBDbContext context) : base(context) { }
}
