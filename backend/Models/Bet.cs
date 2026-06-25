namespace backend.Models;

public class Bet
{
    public int Id { get; set; }

    public int Amount { get; set; }

    public string GameType { get; set; } = string.Empty;

    public int WinAmount { get; set; }

    public bool IsWin { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public int UserId { get; set; }
    public User? User { get; set; }
}