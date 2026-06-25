namespace backend.Models;
using System.Text.Json.Serialization;

public class User
{
	public int Id { get; set; }

	public string Name { get; set; } = string.Empty;

	public string PasswordHash { get; set; } = string.Empty;

	public string Role { get; set; } = "Player";

	public decimal Balance { get; set; } = 1000;

	[JsonIgnore]
	public List<Bet> Bets { get; set; } = new();

	public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

}