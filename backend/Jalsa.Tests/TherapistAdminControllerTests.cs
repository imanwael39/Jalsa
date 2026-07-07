using System.Security.Claims;
using FluentAssertions;
using Jalsa.API.Controllers;
using Jalsa.Application.DTOs.Admin;
using Jalsa.Application.Interfaces.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace Jalsa.Tests;

public class TherapistAdminControllerTests
{
    private readonly Mock<ITherapistAdminService> _therapistAdminServiceMock;
    private readonly AdminDoctorsController _sut;
    private readonly Guid _adminUserId;

    public TherapistAdminControllerTests()
    {
        _therapistAdminServiceMock = new Mock<ITherapistAdminService>();
        _sut = new AdminDoctorsController(_therapistAdminServiceMock.Object);

        _adminUserId = Guid.NewGuid();
        var claims = new List<Claim> { new(ClaimTypes.NameIdentifier, _adminUserId.ToString()) };
        var principal = new ClaimsPrincipal(new ClaimsIdentity(claims, "Test"));
        _sut.ControllerContext = new ControllerContext { HttpContext = new DefaultHttpContext { User = principal } };
    }

    [Fact]
    public async Task GetDoctors_ReturnsPagedResultFromService()
    {
        var expected = new PagedResultDto<TherapistAdminViewDto> { TotalCount = 2 };
        _therapistAdminServiceMock.Setup(s => s.GetTherapistsAsync(It.IsAny<TherapistFilterDto>())).ReturnsAsync(expected);

        var result = await _sut.GetDoctors(new TherapistFilterDto());

        result.Should().BeOfType<OkObjectResult>().Which.Value.Should().Be(expected);
    }

    [Fact]
    public async Task GetDoctorDetail_NotFound_Returns404()
    {
        _therapistAdminServiceMock.Setup(s => s.GetTherapistDetailAsync(It.IsAny<Guid>())).ReturnsAsync((TherapistAdminDetailDto?)null);

        var result = await _sut.GetDoctorDetail(Guid.NewGuid());

        result.Should().BeOfType<NotFoundObjectResult>();
    }

    [Fact]
    public async Task GetDoctorDetail_Found_ReturnsOk()
    {
        var id = Guid.NewGuid();
        _therapistAdminServiceMock.Setup(s => s.GetTherapistDetailAsync(id)).ReturnsAsync(new TherapistAdminDetailDto { Id = id });

        var result = await _sut.GetDoctorDetail(id);

        result.Should().BeOfType<OkObjectResult>();
    }

    [Fact]
    public async Task UpdateStatus_NotFound_Returns404()
    {
        _therapistAdminServiceMock
            .Setup(s => s.UpdateApprovalStatusAsync(It.IsAny<Guid>(), It.IsAny<string>(), It.IsAny<Guid?>(), It.IsAny<string?>(), It.IsAny<string?>()))
            .ReturnsAsync((TherapistAdminViewDto?)null);

        var result = await _sut.UpdateStatus(Guid.NewGuid(), new UpdateTherapistStatusDto { NewStatus = "Approved" });

        result.Should().BeOfType<NotFoundObjectResult>();
    }

    [Fact]
    public async Task UpdateStatus_Found_PassesActingAdminUserIdFromClaims()
    {
        var doctorId = Guid.NewGuid();
        _therapistAdminServiceMock
            .Setup(s => s.UpdateApprovalStatusAsync(doctorId, "Suspended", _adminUserId, It.IsAny<string?>(), It.IsAny<string?>()))
            .ReturnsAsync(new TherapistAdminViewDto { Id = doctorId, ApprovalStatus = "Suspended" });

        var result = await _sut.UpdateStatus(doctorId, new UpdateTherapistStatusDto { NewStatus = "Suspended" });

        result.Should().BeOfType<OkObjectResult>();
        _therapistAdminServiceMock.Verify(s => s.UpdateApprovalStatusAsync(doctorId, "Suspended", _adminUserId, It.IsAny<string?>(), It.IsAny<string?>()), Times.Once);
    }
}
