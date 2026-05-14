namespace Lab4.FileSystem.Local;

public class LocalFileNode : IFileNode
{
    private readonly string _fullPath;

    public string Name { get; }

    public LocalFileNode(string fullPath)
    {
        _fullPath = fullPath;
        Name = Path.GetFileName(fullPath);
    }

    public string ReadContent() => File.ReadAllText(_fullPath);
}
