using System.Linq;
using FluentAssertions;
using Jalsa.API.Services.Implementations.AI;
using Jalsa.API.Services.Interfaces.AI;

namespace Jalsa.Tests;

/// <summary>
/// Enforces the "no clinical context" rule structurally, not just by convention: the
/// patient support AI service must never be able to reach IPatientContextBuilder (intake,
/// assessments, session notes, RAG), because it never receives that dependency at all. If
/// someone later adds an IPatientContextBuilder constructor parameter to
/// PatientSupportAiService to "improve" the patient chat, this test fails immediately.
/// </summary>
public class PatientSupportAiServiceTests
{
    [Fact]
    public void Constructor_NeverDependsOnPatientContextBuilder()
    {
        var constructor = typeof(PatientSupportAiService).GetConstructors().Single();
        var parameterTypes = constructor.GetParameters().Select(p => p.ParameterType);

        parameterTypes.Should().NotContain(typeof(IPatientContextBuilder));
    }
}
