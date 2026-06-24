namespace Jalsa.Domain.Models.Identity;

public class RefreshToken
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public string Token { get; set; } = null!;
    public DateTime ExpiresAt { get; set; }
    public DateTime? RevokedAt { get; set; }
    public Guid? ReplacedByTokenId { get; set; }
    public DateTime CreatedAt { get; set; }

    public User User { get; set; } = null!;
    public RefreshToken? ReplacedByToken { get; set; }
    public ICollection<RefreshToken> ReplacedTokens { get; set; } = new List<RefreshToken>();
}
