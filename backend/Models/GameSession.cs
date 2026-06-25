namespace backend.Models;

public class GameSession
{
    public int Id { get; set; }

    public string GameType { get; set; } = string.Empty;

    public DateTime StartedAt { get; set; } = DateTime.UtcNow;

    public int UserId { get; set; }

    public User? User { get; set; }
}