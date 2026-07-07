using System.Linq.Expressions;
using FluentAssertions;
using Jalsa.Application.DTOs.Admin;
using Jalsa.Application.Interfaces.Repositories;
using Jalsa.Application.Interfaces.Services;
using Jalsa.Application.Services;
using Jalsa.Domain.Models.System;
using Moq;

namespace Jalsa.Tests;

public class SystemSettingsServiceTests
{
    private readonly List<SystemSetting> _settings = [];
    private readonly Mock<IGenericRepository<SystemSetting>> _settingsRepoMock;
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly Mock<IAuditLogService> _auditLogServiceMock;
    private readonly SystemSettingsService _sut;

    public SystemSettingsServiceTests()
    {
        _settingsRepoMock = new Mock<IGenericRepository<SystemSetting>>();
        _settingsRepoMock.Setup(r => r.Query()).Returns(() => _settings.AsQueryable());
        _settingsRepoMock
            .Setup(r => r.FindSingleAsync(It.IsAny<Expression<Func<SystemSetting, bool>>>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((Expression<Func<SystemSetting, bool>> predicate, CancellationToken _) =>
                _settings.AsQueryable().FirstOrDefault(predicate));
        _settingsRepoMock
            .Setup(r => r.AddAsync(It.IsAny<SystemSetting>(), It.IsAny<CancellationToken>()))
            .Callback<SystemSetting, CancellationToken>((s, _) => _settings.Add(s))
            .Returns(Task.CompletedTask);

        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _unitOfWorkMock.Setup(u => u.Repository<SystemSetting>()).Returns(_settingsRepoMock.Object);
        _unitOfWorkMock.Setup(u => u.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);

        _auditLogServiceMock = new Mock<IAuditLogService>();

        _sut = new SystemSettingsService(_unitOfWorkMock.Object, _auditLogServiceMock.Object);
    }

    [Fact]
    public async Task GetSettingsAsync_NoRowsExist_ReturnsDefaults()
    {
        var result = await _sut.GetSettingsAsync();

        result.SiteName.Should().Be("Jalsa");
        result.DefaultLanguage.Should().Be("ar");
        result.PasswordMinLength.Should().Be(8);
        result.SessionTimeoutMinutes.Should().Be(60);
        result.MaintenanceMode.Should().BeFalse();
    }

    [Fact]
    public async Task UpdateSettingsAsync_PersistsAllFieldsAndReadsBackCorrectly()
    {
        var dto = new SystemSettingsDto
        {
            SiteName = "My Clinic",
            DefaultLanguage = "en",
            PasswordMinLength = 10,
            SessionTimeoutMinutes = 30,
            MaintenanceMode = true
        };

        await _sut.UpdateSettingsAsync(dto);
        var result = await _sut.GetSettingsAsync();

        result.SiteName.Should().Be("My Clinic");
        result.DefaultLanguage.Should().Be("en");
        result.PasswordMinLength.Should().Be(10);
        result.SessionTimeoutMinutes.Should().Be(30);
        result.MaintenanceMode.Should().BeTrue();
    }

    [Fact]
    public async Task UpdateSettingsAsync_SecondUpdate_UpsertsExistingRowsInsteadOfDuplicating()
    {
        await _sut.UpdateSettingsAsync(new SystemSettingsDto { SiteName = "First" });
        await _sut.UpdateSettingsAsync(new SystemSettingsDto { SiteName = "Second" });

        _settings.Count(s => s.Key == "SiteName").Should().Be(1);
        (await _sut.GetSettingsAsync()).SiteName.Should().Be("Second");
    }

    [Fact]
    public async Task UpdateSettingsAsync_WritesAuditLogWithOldAndNewValues()
    {
        var dto = new SystemSettingsDto { SiteName = "Updated" };

        await _sut.UpdateSettingsAsync(dto, Guid.NewGuid(), "1.1.1.1", "Agent");

        _auditLogServiceMock.Verify(a => a.LogAsync(
            It.IsAny<Guid?>(), "SystemSettings", null, "Settings.Update",
            It.IsAny<object>(), It.IsAny<object>(), "1.1.1.1", "Agent"), Times.Once);
    }
}
