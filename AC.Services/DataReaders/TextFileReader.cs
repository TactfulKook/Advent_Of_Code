using System.Reflection;

namespace AC.Services.DataReaders;

public class TextFileReader : ITextFileReader
{
    private const string InputFolder = "\\Input\\";

    public string ReadFile(string fileName)
    {
        try
        {
            var executingAssembly = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location); 

            // Open the text file using a stream reader.
            using StreamReader reader = new(executingAssembly + InputFolder + fileName);

            // Read the stream as a string.
            string text = reader.ReadToEnd();

            return text;
        }
        catch (IOException e)
        {
            Console.WriteLine("The file could not be read:");
            Console.WriteLine(e.Message);

            throw new IOException(e.StackTrace);
        }
    }
}
