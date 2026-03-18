using FunMath.Models;

namespace FunMath.Services;

/// <summary>
/// Manages the remaining lives for the current game.
/// </summary>
public class LifeController(GameState gameState)
{
    public int GetRemainingLives() => gameState.Lives;

    public void LoseLife()
    {
        if (gameState.IsGameOver || gameState.Lives <= 0)
        {
            return;
        }

        gameState.Lives--;

        if (gameState.Lives <= 0)
        {
            gameState.Lives = 0;
            gameState.IsGameOver = true;
        }
    }

    public string GetLivesDisplayText() => $"Lives: {gameState.Lives}/{GameState.StartingLives}";
}
