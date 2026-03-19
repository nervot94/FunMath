using System.Diagnostics;
using FunMath.Models;

namespace FunMath.Services;

public class MathProblemGenerator(GameState gameState)
{
    private readonly Random _random = new();
    
    public string GenerateNewProblem()
    {
        int number1;
        int number2;
        int correctAnswer;
        MathOperation mathOperation;
        
        // Generate a problem until we have a valid problem.
        // I hate this code.
        do
        {
            number1 = _random.Next(1, 11);
            number2 = _random.Next(1, 11);
            mathOperation = GetRandomMathOperation();

            correctAnswer = mathOperation switch
            {
                MathOperation.Addition => number1 + number2,
                MathOperation.Substraction => number1 - number2,
                MathOperation.Multiplication => number1 * number2,
                MathOperation.Division => number1 / number2,
                _ => throw new UnreachableException("This should not be reachable"),
            };
        } while (correctAnswer % 2 != 0 || // Answer must be even
                 correctAnswer < 0 || // No negative answers
                 (mathOperation == MathOperation.Division && number1 % number2 != 0)); // Division must be exact
        
        gameState.CorrectAnswer = correctAnswer;

        return GetFormatString(mathOperation, number1, number2);
    }

    private static string GetFormatString(MathOperation operation, int number1, int number2)
    {
        return operation switch
        {
            MathOperation.Addition => $"{number1} + {number2} = ?", 
            MathOperation.Substraction => $"{number1} - {number2} = ?",
            MathOperation.Multiplication => $"{number1} x {number2} = ?",
            MathOperation.Division => $"{number1} : {number2} = ?",

            // C# SHUTUP
            _ => throw new UnreachableException("This should not be reachable")
        };
    }
    
    /// <summary>
    /// Give a random math operation to be used for creating random math problems.
    /// </summary>
    /// <returns>A random math operation such as addition and subtraction.</returns>
    private MathOperation GetRandomMathOperation()
    {
        var values = Enum.GetValues<MathOperation>();
        return values[_random.Next(values.Length)];
    }
}

public enum MathOperation
{
    Addition,
    Substraction,
    Multiplication,
    Division,
}