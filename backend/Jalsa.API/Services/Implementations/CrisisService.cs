using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Jalsa.Application.DTOs.Crisis;
using Jalsa.Application.Interfaces.Services;
using Jalsa.Infrastructure.Data;
using Jalsa.Domain.Models.Crisis;

namespace Jalsa.API.Services.Implementations;

public class CrisisService : ICrisisService
{
    private readonly Galsa_DBDbContext _context;
    private readonly IMapper _mapper;

    public CrisisService(Galsa_DBDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<CrisisAlertViewDto> CreateManualAlertAsync(Guid patientId, CancellationToken cancellationToken = default)
    {
        var alert = new CrisisAlert
        {
            Id = Guid.NewGuid(),
            PatientId = patientId,
            Severity = "High",
            Status = "Open",
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        _context.CrisisAlerts.Add(alert);
        await _context.SaveChangesAsync(cancellationToken);

        return _mapper.Map<CrisisAlertViewDto>(alert);
    }
}
