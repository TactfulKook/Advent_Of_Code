using AC.Services.DataReaders;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AC.Problems;

public class Day05(ITextFileReader textFileReader) : IProblem
{
    private readonly ITextFileReader _textFileReader = textFileReader;
    private const string fileName = "day_five.txt";
    private const string AnswerMessage = "Result: {0}";
    private const string WrongHalfNumber = "No problem with given half {0}.";

    private Dictionary<int, List<int>> rules = new Dictionary<int, List<int>>();
    private List<List<int>> updates = new List<List<int>>();

    public string Solve(int half)
    {
        var fileContent = _textFileReader.ReadFile(fileName);

        InitializeLists(fileContent);

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
        var filteredUpdates = FilterUpdates(true);

        var result = filteredUpdates.Sum(u => u[(u.Count - 1) / 2]);

        return string.Format(AnswerMessage, result);
    }

    private string SecondHalf()
    {
        var filteredUpdates = FilterUpdates(false);

        filteredUpdates.ForEach(u => OrderUpdate(u));

        var result = filteredUpdates.Sum(u => u[(u.Count - 1) / 2]);

        return string.Format(AnswerMessage, result);

    }

    private List<List<int>> FilterUpdates(bool correct)
    {
        var filteredUpdates = new List<List<int>>();

        foreach (var update in updates)
        {
            (var rulesOK, var index) = IsCorrect(update);

            if ((rulesOK && correct) || (!rulesOK && !correct))
            {
                filteredUpdates.Add(update);
            }
        }

        return filteredUpdates;
    }

    private List<int> OrderUpdate(List<int> update)
    {
        (var correct, var index) = IsCorrect(update);

        if (correct)
            return update;

        (update[index - 1], update[index]) = (update[index], update[index - 1]);
        return OrderUpdate(update);
    }

    /// <summary>
    /// Check if update has correct rules and returns the index of the first incorrect item if not correct.
    /// </summary>
    /// <param name="update"></param>
    /// <returns></returns>
    private (bool, int) IsCorrect(List<int> update)
    {
        var previousItems = new List<int>();

        var rulesOK = true;
        var faultyIndex = -1;

        foreach (var item in update)
        {
            if (rules.TryGetValue(item, out var nonAllowedItems))
            {
                foreach (var previousItem in previousItems)
                {
                    if (nonAllowedItems.Contains(previousItem))
                    {
                        rulesOK = false;
                        break;
                    }
                }

                if (!rulesOK)
                {
                    faultyIndex = update.IndexOf(item);
                    break;
                }

                previousItems.Add(item);
            }
        }

        return (rulesOK, faultyIndex);
    }

    private void InitializeLists(string fileContent)
    {
        var rows = fileContent.Split('\n');

        var i = 0;
        var row = rows[i];

        while (i < rows.Length && !row.Equals(string.Empty))
        {
            if (row == string.Empty) continue;

            var numbers = row.Split('|');
            if (int.TryParse(numbers[0], out int number1) && int.TryParse(numbers[1], out int number2))
            {
                if (rules.ContainsKey(number1))
                {
                    rules[number1].Add(number2);
                }
                else
                {
                    var list = new List<int>() { number2 };
                    rules.Add(number1, list);
                }
            }

            row = rows[++i];
        }

        row = rows[++i];

        while (i < rows.Length && !row.Equals(string.Empty))
        {
            var content = row.Split(',');

            var list = new List<int>();

            foreach (var entry in content)
            {

                if (int.TryParse(entry, out int number))
                {
                    list.Add(number);
                }
            }

            updates.Add(list);

            row = rows[++i];
        }
    }
}
