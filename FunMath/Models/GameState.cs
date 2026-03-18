namespace FunMath.Models;

public class GameState
{
    public const int StartingLives = 3;

    public int Score { get; set; }
    public int Lives { get; set; }
    public int CorrectAnswer { get; set; }
    public bool IsGameOver { get; set; }

    public GameState() => Reset();
    
    public void Reset()
    {
        Score = 0;
        Lives = StartingLives;
        CorrectAnswer = 0;
        IsGameOver = false;
    }
}
