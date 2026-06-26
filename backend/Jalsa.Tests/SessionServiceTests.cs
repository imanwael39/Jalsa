using AutoMapper;
using FluentAssertions;
using Jalsa.Application.DTOs.Session;
using Jalsa.Application.Interfaces.Repositores;
using Jalsa.Application.Interfaces.Services;
using Jalsa.Application.Mappings;
using Jalsa.Application.Services;
using Jalsa.Domain.Models.Session;
using Moq;
using Xunit;

namespace Jalsa.Tests.Services;

public class SessionServiceTests
{
    private readonly Mock<ISessionRepository> _sessionRepositoryMock;
    private readonly Mock<IGenericRepository<SessionNote>> _sessionNoteRepositoryMock;
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly IMapper _mapper;
    private readonly SessionService _sessionService;

    public SessionServiceTests()
    {
        _sessionRepositoryMock = new Mock<ISessionRepository>();
        _sessionNoteRepositoryMock = new Mock<IGenericRepository<SessionNote>>();
        _unitOfWorkMock = new Mock<IUnitOfWork>();

        var configuration = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile<SessionMappingProfile>();
        });
        _mapper = configuration.CreateMapper();

        _sessionService = new SessionService(
            _sessionRepositoryMock.Object,
            _sessionNoteRepositoryMock.Object,
            _unitOfWorkMock.Object,
            _mapper);
    }

    [Fact]
    public async Task GetByIdAsync_WhenSessionExists_ReturnsSessionViewDto()
    {
        var sessionId = Guid.NewGuid();
        var session = new Session
        {
            Id = sessionId,
            PatientId = Guid.NewGuid(),
            SessionNumber = 1,
            SessionDate = DateOnly.FromDateTime(DateTime.UtcNow.AddDays(-1)),
            Status = "Draft",
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow,
            Patient = new Domain.Models.Patient.Patient { FullName = "Test Patient" },
            SessionNote = new SessionNote
            {
                Id = Guid.NewGuid(),
                SessionId = sessionId,
                Observations = "Test observations"
            }
        };

        _sessionRepositoryMock
            .Setup(x => x.GetWithDetailsAsync(sessionId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(session);

        var result = await _sessionService.GetByIdAsync(sessionId);

        result.Should().NotBeNull();
        result!.Id.Should().Be(sessionId);
        result.PatientName.Should().Be("Test Patient");
        result.SessionNote.Should().NotBeNull();
        result.SessionNote!.Observations.Should().Be("Test observations");
    }

    [Fact]
    public async Task GetByIdAsync_WhenSessionDoesNotExist_ReturnsNull()
    {
        var sessionId = Guid.NewGuid();

        _sessionRepositoryMock
            .Setup(x => x.GetWithDetailsAsync(sessionId, It.IsAny<CancellationToken>()))
            .ReturnsAsync((Session?)null);

        var result = await _sessionService.GetByIdAsync(sessionId);

        result.Should().BeNull();
    }

    [Fact]
    public async Task CreateAsync_WhenValidDto_CreatesSession()
    {
        var patientId = Guid.NewGuid();
        var dto = new SessionCreateDto
        {
            PatientId = patientId,
            SessionDate = DateOnly.FromDateTime(DateTime.UtcNow.AddDays(-1)),
            SessionType = "Individual",
            Status = "Draft",
            SessionNote = new SessionNoteCreateDto
            {
                Observations = "Test observations"
            }
        };

        _sessionRepositoryMock
            .Setup(x => x.GetNextSessionNumberAsync(patientId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(1);

        _sessionRepositoryMock
            .Setup(x => x.GetWithDetailsAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((Guid id, CancellationToken ct) => new Session
            {
                Id = id,
                PatientId = patientId,
                SessionNumber = 1,
                SessionDate = dto.SessionDate,
                Status = dto.Status!,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            });

        var result = await _sessionService.CreateAsync(dto);

        result.Should().NotBeNull();
        result.PatientId.Should().Be(patientId);
        result.SessionNumber.Should().Be(1);
        result.Status.Should().Be("Draft");

        _sessionRepositoryMock.Verify(x => x.AddAsync(It.IsAny<Session>(), It.IsAny<CancellationToken>()), Times.Once);
        _sessionNoteRepositoryMock.Verify(x => x.AddAsync(It.IsAny<SessionNote>(), It.IsAny<CancellationToken>()), Times.Once);
        _unitOfWorkMock.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task DeleteAsync_WhenSessionExists_ReturnsTrue()
    {
        var sessionId = Guid.NewGuid();
        var session = new Session { Id = sessionId };

        _sessionRepositoryMock
            .Setup(x => x.GetWithDetailsAsync(sessionId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(session);

        var result = await _sessionService.DeleteAsync(sessionId);

        result.Should().BeTrue();
        _sessionRepositoryMock.Verify(x => x.Remove(session), Times.Once);
        _unitOfWorkMock.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task DeleteAsync_WhenSessionDoesNotExist_ReturnsFalse()
    {
        var sessionId = Guid.NewGuid();

        _sessionRepositoryMock
            .Setup(x => x.GetWithDetailsAsync(sessionId, It.IsAny<CancellationToken>()))
            .ReturnsAsync((Session?)null);

        var result = await _sessionService.DeleteAsync(sessionId);

        result.Should().BeFalse();
    }
}
