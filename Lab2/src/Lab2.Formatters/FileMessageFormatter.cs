namespace Lab2.Formatters;

public class FileMessageFormatter : IMessageFormatter
{
    private readonly string _filePath;

    public FileMessageFormatter(string filePath)
    {
        _filePath = filePath;
    }

    public void WriteTitle(string title)
    {
        File.AppendAllText(_filePath, $"# {title}{Environment.NewLine}");
    }

    public void WriteBody(string body)
    {
        File.AppendAllText(_filePath, $"{Environment.NewLine}{body}{Environment.NewLine}");
    }
}
