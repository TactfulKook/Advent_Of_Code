using AC.Services.DataReaders;

namespace AC.Problems;

public class DayFour(ITextFileReader textFileReader) : IProblem
{
    private readonly ITextFileReader _textFileReader = textFileReader;
    private const string fileName = "day_four.txt";
    private const string AnswerMessage = "Result: {0}";
    private const string WrongHalfNumber = "No problem with given half {0}.";

    private List<string> LetterTable { get; set; } = [];

    private readonly List<(int, int)> XmasDirections = [(-1, -1), (-1, 0), (-1, 1), (0, -1), (0, 0), (0, 1), (1, -1), (1, 0), (1, 1)];
    private readonly List<(int, int)> MasDirections = [(-1, -1), (-1, 1), (1, -1), (1, 1)];

    private void FirstHalf()
    {
        long result = 0;

        for (int i = 0; i < LetterTable.Count; i++)
        {
            var j = LetterTable[i].IndexOf('X');

            while (j >= 0 && j < LetterTable[i].Length)
            {
                foreach (var (directionI, directionJ) in XmasDirections)
                {
                    if (i + 3 * directionI >= 0 && i + 3 * directionI < LetterTable[i].Length && j + 3 * directionJ >= 0 && j + 3 * directionJ < LetterTable.Count)
                    {
                        if (LetterTable[i + directionI][j + directionJ] != 'M')
                        {
                            continue;
                        }
                        if (LetterTable[i + 2 * directionI][j + 2 * directionJ] != 'A')
                        {
                            continue;
                        }
                        if (LetterTable[i + 3 * directionI][j + 3 * directionJ] != 'S')
                        {
                            continue;
                        }
                        result++;
                    }
                }

                j = LetterTable[i].IndexOf('X', j + 1);
            }
        }

        Console.WriteLine(AnswerMessage, result);
    }

    private void SecondHalf()
    {
        var result = 0;

        for (int i = 1; i < LetterTable.Count - 1; i++)
        {
            var j = LetterTable[i].IndexOf('A', 1);

            while (j >= 1 && j < LetterTable[i].Length - 1)
            {
                var masCount = 0;

                foreach (var (directionI, directionJ) in MasDirections)
                {
                    if (LetterTable[i + -1 * directionI][j + -1 * directionJ] != 'M')
                    {
                        continue;
                    }
                    if (LetterTable[i + directionI][j + directionJ] != 'S')
                    {
                        continue;
                    }
                    masCount++;
                }

                if (masCount == 1)
                {
                    Console.WriteLine(LetterTable[i - 1].Substring(j - 1, 3));
                    Console.WriteLine(LetterTable[i].Substring(j - 1, 3));
                    Console.WriteLine(LetterTable[i + 1].Substring(j - 1, 3) + "\n");
                }
                if (masCount == 2)
                {
                    result++;
                }
                if (masCount > 2)
                {
                    Console.WriteLine("WTF");
                }

                j = LetterTable[i].IndexOf('A', j + 1);
            }
        }

        Console.WriteLine(AnswerMessage, result);
    }

    public void Solve(int half)
    {
        var fileContent = _textFileReader.ReadFile(fileName);

        InitializeLetterTable(fileContent);

        if (half == 1)
        {
            FirstHalf();
        }
        else if (half == 2)
        {
            SecondHalf();
        }
        else
        {
            Console.WriteLine(WrongHalfNumber, half);
        }
    }

    private void InitializeLetterTable(string fileContent)
    {
        LetterTable = [.. fileContent.Split('\n', StringSplitOptions.RemoveEmptyEntries)];
    }
}
