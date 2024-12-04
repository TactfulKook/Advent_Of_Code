using AC.Services.DataReaders;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AC.Problems;

public partial class DayThree(ITextFileReader textFileReader) : IProblem
{
    private readonly ITextFileReader _textFileReader = textFileReader;
    private const string fileName = "day_three.txt";
    private const string AnswerMessage = "Result: {0}";
    private const string WrongHalfNumber = "No problem with given half {0}.";
    private const string DoString = "do()";
    private const string DontString = "don't()";

    public void Solve(int half)
    {
        var fileContent = _textFileReader.ReadFile(fileName);

        if (half == 1)
        {
            FirstHalf(fileContent);
        }
        else if (half == 2)
        {
            SecondHalf(fileContent);
        }
        else
        {
            Console.WriteLine(WrongHalfNumber, half);
        }
    }

    private static void FirstHalf(string fileContent)
    {
        long result = 0;

        var multiplications = MulRegex().Matches(fileContent);

        if(multiplications != null && multiplications.Count > 0)
        {
            foreach (Match multiplication in multiplications)
            {
                var numbers = NumberRegex().Replace(multiplication.Value, "");

                var numberPair = numbers.Split(",");

                if (long.TryParse(numberPair[0], out long number1) && long.TryParse(numberPair[1], out long number2))
                {
                    result += (number1 * number2);
                }
            }
        }

        Console.WriteLine(AnswerMessage, result);
    }

    private void SecondHalf(string fileContent)
    {
        long result = 0;

        var multiplications = MulDoRegex().Matches(fileContent);

        if (multiplications != null && multiplications.Count > 0)
        {
            var trimmedMultiplications = TrimMultiplications(multiplications);

            foreach (var multiplication in trimmedMultiplications)
            {
                var numbers = NumberRegex().Replace(multiplication, "");

                var numberPair = numbers.Split(",");

                if (long.TryParse(numberPair[0], out long number1) && long.TryParse(numberPair[1], out long number2))
                {
                    result += (number1 * number2);
                }
            }
        }

        Console.WriteLine(AnswerMessage, result);
    }

    private List<string> TrimMultiplications(MatchCollection multiplications)
    {
        var list = new List<string>();

        bool doFlag = true;

        foreach (Match line in multiplications)
        {
            if (line.Value.Equals(DoString))
            {
                doFlag = true;
            }
            else if (line.Value.Equals(DontString))
            {
                doFlag = false;
            }
            else if (doFlag)
            {
                list.Add(line.Value);
            }
        }

        return list;
    }

    [GeneratedRegex(@"mul\(\d+,\d+\)")]
    private static partial Regex MulRegex();
    [GeneratedRegex(@"(mul\(\d+,\d+\))|do\(\)|don't\(\)")]
    private static partial Regex MulDoRegex();
    [GeneratedRegex(@"\)?(mul\()?")]
    private static partial Regex NumberRegex();
}
