using Jalsa.Application.Interfaces.Repositories;
using Jalsa.Domain.Models.Patient;
using Jalsa.Infrastructure.Data;

namespace Jalsa.Infrastructure.Repositories;

public class PatientRepository : GenericRepository<Patient>, IPatientRepository
{
    public PatientRepository(Galsa_DBDbContext context) : base(context) { }
}
