using AC.Problems;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Diagnostics;

namespace Advent_of_Code;

public class Worker(IServiceProvider serviceProvider) : IHostedService
{

    private const string PromptAsk = "\nWhich days problem would you like to solve? \n" +
        "Type a number to select day, type 'end' to stop program \n";
    private const string EndPrompt = "end";
    private const string ExitText = "\nProgram will now exit, thank you for playing. \n";
    private const string FaultyPromptText = "\nFaulty Prompt given: {0}. Try again. \n";
    private const string NoPromptFound = "\nNo Problem number {0} found. Try again. \n";
    private const string DayPrompt = "\nSolving {0} Problem, which half would you like to solve? \n";
    private const string ExecutionTime = "\nExecution took {0} ticks";

    private readonly Dictionary<int, Type> Problems = new Dictionary<int, Type>()
    {
        { 1, typeof(DayOne) },
        { 2, typeof(DayTwo) },
        { 3, typeof(DayThree) },
        { 4, typeof(DayFour) },
        { 5, typeof(Day05) },
    };

    private readonly IServiceProvider ServiceProvider = serviceProvider;

    public Task StartAsync(CancellationToken cancellationToken)
    {
        string? prompt;
        do
        {
            Console.WriteLine(PromptAsk);
            prompt = Console.ReadLine();
            try
            {
                if (int.TryParse(prompt, out int problemId))
                {
                    var problemType = GetProblemType(problemId);

                    if (problemType != null)
                    {
                        Console.WriteLine(DayPrompt, problemType.Name);

                        var halfPrompt = Console.ReadLine();

                        if (int.TryParse(halfPrompt, out int half) && (half == 1 || half == 2))
                        {
                            var problem = (IProblem)ActivatorUtilities.CreateInstance(ServiceProvider, problemType);

                            Stopwatch stopWatch = new();
                            stopWatch.Start();
                            var answerMessage = problem.Solve(half);
                            stopWatch.Stop();
                            Console.WriteLine(ExecutionTime, stopWatch.ElapsedTicks);
                            Console.WriteLine(answerMessage);

                        }
                        else
                        {
                            Console.Write(FaultyPromptText);
                        }
                    }
                    else
                    {
                        Console.WriteLine(NoPromptFound, prompt);
                    }
                }
                else if (prompt == null || !prompt.Equals(EndPrompt))
                {
                    Console.WriteLine(FaultyPromptText, prompt);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Error in execution with prompt: {0}. Given error: {1}", prompt, e.Message);
            }
        }
        while (prompt != null && !prompt.Equals(EndPrompt));

        Console.WriteLine(ExitText);

        return Task.CompletedTask;
    }

    private Type? GetProblemType(int problemId)
    {
        if (Problems.TryGetValue(problemId, out Type? problemType))
        {
            return problemType;
        }
        return null;
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
