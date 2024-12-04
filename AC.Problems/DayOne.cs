using AC.Services.DataReaders;

namespace AC.Problems;

public class DayOne(ITextFileReader textFileReader) : IProblem
{
    private readonly ITextFileReader _textFileReader = textFileReader;
    private const string fileName = "day_one.txt";
    private const string FirstAnswerMessage = "First half: Total lenght between the numbers is: {0}";
    private const string SecondAnswerMessage = "Second half: Total similarity score between the numbers is: {0}";
    private const string WrongHalfNumber = "No problem with given half {0}.";

    private List<long> firstColumn { get; set; } = new List<long>();
    private List<long> secondColumn { get; set; } = new List<long>();

    public void Solve(int half)
    {
        var fileContent = _textFileReader.ReadFile(fileName);

        InitializeLists(fileContent);

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

    private void FirstHalf()
    {
        long totalDistance = 0;

        for (var i = 0; i < firstColumn.Count; i++)
        {
            totalDistance += Math.Abs(firstColumn[i] - secondColumn[i]);
        }

        Console.WriteLine(FirstAnswerMessage, totalDistance);
    }

    private void SecondHalf()
    {
        long similarityScore = 0;

        for (var i = 0; i < firstColumn.Count; i++)
        {
            var appearances = secondColumn.Count(a => a == firstColumn[i]);

            similarityScore += (appearances * firstColumn[i]);
        }

        Console.WriteLine(SecondAnswerMessage, similarityScore);
    }

    private void InitializeLists(string fileContent)
    {
        List<string> rows = fileContent.Split(new char[] { '\n', }).ToList();

        foreach (var row in rows)
        {
            var numbers = row.Split(' ', StringSplitOptions.RemoveEmptyEntries);

            if (numbers.Length == 2)
            {
                if (long.TryParse(numbers[0], out long first) && long.TryParse(numbers[1], out long second))
                {
                    firstColumn.Add(first);
                    secondColumn.Add(second);
                }
                else
                {
                    throw new Exception("Faulty numbers");
                }
            }
        }

        firstColumn.Sort();
        secondColumn.Sort();
    }
}
