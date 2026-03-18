using FunMath.Models;

namespace FunMath.Services;

/// <summary>
/// Manages all score-related logic for the math game.
/// </summary>
public class ScoreController(GameState gameState)
{
    public void IncrementScore() => gameState.Score++;
    
    public string GetScoreDisplayText() => $"Score: {gameState.Score}";
}
