using backend.Data;
using backend.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using backend.Dtos;

namespace backend.Controllers;

[ApiController]
[Route("casino")]
[Authorize]
public class CasinoController : ControllerBase
{
    private readonly AppDbContext _context;

    public CasinoController(AppDbContext context)
    {
        _context = context;
    }

    [HttpPost("spin")]
    public async Task<IActionResult> Spin(SpinRequest request)
    {
        var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

        var user = await _context.Users.FirstOrDefaultAsync(x => x.Id == userId);

        if (user == null)
            return NotFound("User not found");

        if (request.Bet <= 0)
            return BadRequest("Invalid bet");

        if (request.Bet > user.Balance)
            return BadRequest("Not enough balance");

        if (user.Balance <= 0)
        {
            return Ok(new
            {
                message = "Не вистачає балансу 💀",
                symbols = new[] { "❌", "❌", "❌" },
                winAmount = 0,
                balance = 0
            });
        }

        var random = new Random();

        var symbols = new[] { "🍒", "🍋", "⭐", "💎", "7️⃣" };

        var winPhrases = new[]
        {
        "🎉 Удача на вашому боці!",
        "🔥 Гарячий виграш!",
        "💰 Гроші йдуть до тебе!",
        "🍀 Удача усміхається тобі!",
        "⚡ Ти сьогодні в ударі!",
        "👑 Король удачі!"
    };

        var losePhrases = new[]
        {
        "💀 Не пощастило... спробуй ще!",
        "😅 Майже вийшло!",
        "🎲 Казино сьогодні сильніше!",
        "🙈 Удача відвернулася...",
        "💸 Мінус на цей раз!",
        "😈 Спробуй відігратися!"
    };

        int winType = random.Next(100);
        string r1, r2, r3;

        bool isWin = false;
        int winAmount = 0;

        if (winType < 60)
        {
            do
            {
                r1 = symbols[random.Next(symbols.Length)];
                r2 = symbols[random.Next(symbols.Length)];
                r3 = symbols[random.Next(symbols.Length)];
            }
            while (r1 == r2 || r2 == r3 || r1 == r3);

            isWin = false;
        }
        else if (winType < 90)
        {
            var s = symbols[random.Next(symbols.Length)];
            var other = symbols[random.Next(symbols.Length)];

            r1 = s;
            r2 = s;
            r3 = other;

            isWin = true;
            winAmount = request.Bet * 2;
        }
        else
        {
            var s = symbols[random.Next(symbols.Length)];

            r1 = r2 = r3 = s;

            isWin = true;
            winAmount = request.Bet * 10;
        }

        var message = isWin
            ? winPhrases[random.Next(winPhrases.Length)]
            : losePhrases[random.Next(losePhrases.Length)];

        user.Balance -= request.Bet;
        user.Balance += winAmount;

        var bet = new Bet
        {
            Amount = request.Bet,
            GameType = "Slots",
            WinAmount = winAmount,
            IsWin = isWin,
            UserId = user.Id,
            CreatedAt = DateTime.UtcNow
        };

        _context.Bets.Add(bet);

        await _context.SaveChangesAsync();

        return Ok(new
        {
            symbols = new[] { r1, r2, r3 },
            isWin,
            winAmount,
            balance = user.Balance,
            message
        });
    }
}