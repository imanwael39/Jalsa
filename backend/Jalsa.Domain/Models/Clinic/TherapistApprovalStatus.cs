namespace Jalsa.Domain.Models.Clinic;

public static class TherapistApprovalStatus
{
    public const string Pending = "Pending";
    public const string Approved = "Approved";
    public const string Rejected = "Rejected";
    public const string Suspended = "Suspended";

    public static readonly string[] All = [Pending, Approved, Rejected, Suspended];
}
