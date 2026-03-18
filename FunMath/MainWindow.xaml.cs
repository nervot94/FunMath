using System.Windows;
using System.Windows.Input;
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
    private readonly MathProblemGenerator _problemGenerator;
    
    public MainWindow()
    {
        InitializeComponent();

        _scoreController = new ScoreController(_gameState);
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

        // TODO: Check if we have a game over state.
        //       If so, reset game
    }

    /// <summary>
    /// Checks the user's answer against the correct answer.
    /// </summary>
    private void CheckAnswer()
    {
        if (string.IsNullOrWhiteSpace(AnswerTextBox.Text))
        {
            MessageBox.Show("Voer een antwoord in!", "Geen antwoord", MessageBoxButton.OK, MessageBoxImage.Warning);
            return;
        }

        // We do not want to have invalid answers crash the entire game.
        if (!int.TryParse(AnswerTextBox.Text, out var userAnswer))
        {
            MessageBox.Show("Voer een geldig getal in!", "Ongeldige invoer", MessageBoxButton.OK, MessageBoxImage.Warning);
            return;
        }

        // Check if the answer is correct
        if (userAnswer == _gameState.CorrectAnswer)
        {
            _scoreController.IncrementScore();
            MessageBox.Show("Correct! Goed gedaan!", "Juist", MessageBoxButton.OK, MessageBoxImage.Information);
            GenerateNewProblem();
            UpdateUi();
        }
        else
        {
            // TODO: Decrement lives by 1
            
            MessageBox.Show($"Fout! Het juiste antwoord was {_gameState.CorrectAnswer}", "Fout", MessageBoxButton.OK, MessageBoxImage.Error);
            UpdateUi();

            // TODO: Only generate a new problem if we are in a game over state
            GenerateNewProblem();
        }
    }
    
    private void ResetGame()
    {
        _gameState.Reset();
        
        AnswerTextBox.IsEnabled = true;
        CheckButton.IsEnabled = true;
        NewProblemButton.IsEnabled = true;
        GenerateNewProblem();
        UpdateUi();
    }
    
    private void CheckButton_Click(object sender, RoutedEventArgs e)
    {
        CheckAnswer();
    }
    
    private void NewProblemButton_Click(object sender, RoutedEventArgs e)
    {
        GenerateNewProblem();
    }
    
    private void AnswerTextBox_KeyDown(object sender, KeyEventArgs e)
    {
        // TODO: Check game over state
        if (e.Key == Key.Enter)
        {
            CheckAnswer();
        }
    }
    
    private void RestartButton_Click(object sender, RoutedEventArgs e)
    {
        ResetGame();
    }
    
    private void NewGameButton_Click(object sender, RoutedEventArgs e)
    {
        ResetGame();
    }
}