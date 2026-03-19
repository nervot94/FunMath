using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using FunMath.Models;
using FunMath.Services;

namespace FunMath;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    private readonly GameState _gameState = new();
    private readonly ScoreController _scoreController;
    private readonly LifeController _lifeController;
    private readonly MathProblemGenerator _problemGenerator;
    
    public MainWindow()
    {
        InitializeComponent();

        _scoreController = new ScoreController(_gameState);
        _lifeController = new LifeController(_gameState);
        _problemGenerator = new MathProblemGenerator(_gameState);

        GenerateNewProblem();
        UpdateUi();

        AnswerTextBox.Focus();
    }
    
    private void GenerateNewProblem()
    {
        var problem = _problemGenerator.GenerateNewProblem();
        MathProblemTextBlock.Text = problem;
        AnswerTextBox.Clear();
        AnswerTextBox.Focus();
    }
    
    private void UpdateUi()
    {
        ScoreTextBlock.Text = _scoreController.GetScoreDisplayText();
        LiveTextBlock.Text = _lifeController.GetLivesDisplayText();
    }

    /// <summary>
    /// Checks the user's answer against the correct answer.
    /// </summary>
    private void CheckAnswer()
    {
        if (string.IsNullOrWhiteSpace(AnswerTextBox.Text))
        {
            return;
        }

        // We do not want to have invalid answers crash the entire game.
        if (!int.TryParse(AnswerTextBox.Text, out var userAnswer))
        {
            return;
        }

        // Check if the answer is correct
        if (userAnswer == _gameState.CorrectAnswer)
        {
            _scoreController.IncrementScore();
            FlashBackground("#40a02b"); // Green for correct
            GenerateNewProblem();
            UpdateUi();
        }
        else
        {
            _lifeController.LoseLife();
            FlashBackground("#d20f39"); // Red for incorrect
            UpdateUi();

            if (_gameState.IsGameOver)
            {
                MessageBox.Show("Game over! :(", "Game Over", MessageBoxButton.OK, MessageBoxImage.Information);
                ResetGame();
                return;
            }

            GenerateNewProblem();
        }
    }
    
    /// <summary>
    /// Flashes the card background with the specified color.
    /// </summary>
    /// <param name="flashColor">The color to flash (e.g., green for correct, red for incorrect).</param>
    private void FlashBackground(string flashColor)
    {
        var originalColor = (Color)ColorConverter.ConvertFromString("#dce0e8");
        var targetColor = (Color)ColorConverter.ConvertFromString(flashColor);
        
        var colorAnimation = new ColorAnimation
        {
            From = targetColor,
            To = originalColor,
            Duration = TimeSpan.FromSeconds(1),
            AutoReverse = false
        };
        
        var brush = new SolidColorBrush(originalColor);
        MainGrid.Background = brush;
        brush.BeginAnimation(SolidColorBrush.ColorProperty, colorAnimation);
    }
    
    private void ResetGame()
    {
        _gameState.Reset();

        GenerateNewProblem();
        UpdateUi();
    }

    private void CheckButton_Click(object sender, RoutedEventArgs e) => CheckAnswer();
    
    private void NewProblemButton_Click(object sender, RoutedEventArgs e) => GenerateNewProblem();
    
    private void NewGameButton_Click(object sender, RoutedEventArgs e) => ResetGame();

    private void RestartButton_Click(object sender, RoutedEventArgs e)
    {
        GenerateNewProblem();
        UpdateUi();
    }
    
    private void AnswerTextBox_KeyDown(object sender, KeyEventArgs e)
    {
        if (e.Key == Key.Enter)
        {
            CheckAnswer();
        }
    }
}
