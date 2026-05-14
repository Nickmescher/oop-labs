namespace Lab2.Formatters;

public class ConsoleMessageFormatter : IMessageFormatter
{
    public void WriteTitle(string title)
    {
        Console.WriteLine($"# {title}");
    }

    public void WriteBody(string body)
    {
        Console.WriteLine();
        Console.WriteLine(body);
    }
}
