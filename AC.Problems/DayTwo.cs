using AC.Services.DataReaders;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AC.Problems;

public class DayTwo(ITextFileReader textFileReader) : IProblem
{
    private readonly ITextFileReader _textFileReader = textFileReader;
    private const string fileName = "day_two.txt";
    private const string AnswerMessage = "Safe Reports: {0}";
    private const string WrongHalfNumber = "No problem with given half {0}.";

    private List<List<int>> reports = new List<List<int>>();

    public string Solve(int half)
    {
        var fileContent = _textFileReader.ReadFile(fileName);

        InitializeReports(fileContent);

        if (half == 1)
        {
            return FirstHalf();
        }
        else if (half == 2)
        {
            return SecondHalf();
        }
        else
        {
            return string.Format(WrongHalfNumber, half);
        }
    }

    private string FirstHalf()
    {
        int safeReports = 0;

        foreach (var report in reports)
        {
            if (report == null || report.Count == 0)
            {
                continue;
            }

            (var safeReport, var unsafeIndex) = CheckIfSafe(report);

            if (safeReport)
            {
                safeReports++;
            }
        }

        return string.Format(AnswerMessage, safeReports);
    }

    private string SecondHalf()
    {
        int safeReports = 0;

        foreach (var report in reports)
        {
            if (report == null || report.Count == 0)
            {
                continue;
            }

            (var safeReport, var unsafeIndex) = CheckIfSafe(report);

            while (!safeReport && unsafeIndex >= 0)
            {
                var tempReport = new List<int>();

                tempReport.AddRange(report);

                tempReport.RemoveAt(unsafeIndex);

                (safeReport, var temp) = CheckIfSafe(tempReport);

                unsafeIndex--;
            }

            if (safeReport)
            {
                safeReports++;
            }
        }

        return string.Format(AnswerMessage, safeReports);
    }

    private static (bool, int) CheckIfSafe(List<int> report)
    {
        bool safeReport = true;
        var unsafeIndex = -1;
        int i = 0;

        int previousNumber = report[i];
        int increasing = 1;

        while (safeReport && i < (report.Count - 1))
        {
            i++;

            var currentNumber = report[i];

            if (i == 1 && previousNumber > currentNumber)
            {
                increasing = -1;
            }

            var distance = increasing * (currentNumber - previousNumber);

            if (distance < 1 || distance > 3)
            {
                safeReport = false;
                unsafeIndex = i;
            }
            else
            {
                previousNumber = currentNumber;
            }
        }

        return (safeReport, unsafeIndex);
    }

    private void InitializeReports(string fileContent)
    {
        List<string> rows = [.. fileContent.Split(new char[] { '\n', })];

        foreach (var row in rows)
        {
            var numbers = row.Split(' ', StringSplitOptions.RemoveEmptyEntries);

            var numberRow = new List<int>();

            if (numbers != null && numbers.Length > 0)
            {
                foreach (var number in numbers)
                {
                    if (int.TryParse(number, out var numberValue))
                    {
                        numberRow.Add(numberValue);
                    }
                    else
                    {
                        throw new Exception("Non-number found!");
                    }
                }
            }

            reports.Add(numberRow);
        }
    }

}
