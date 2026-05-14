namespace Lab4.Output;

public class ConsoleOutputWriter : IOutputWriter
{
    public void WriteLine(string text) => Console.WriteLine(text);
}
