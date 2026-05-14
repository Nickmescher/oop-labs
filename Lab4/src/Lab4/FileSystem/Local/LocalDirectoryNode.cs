namespace Lab4.FileSystem.Local;

public class LocalDirectoryNode : IDirectoryNode
{
    private readonly string _fullPath;

    public string Name { get; }

    public LocalDirectoryNode(string fullPath)
    {
        _fullPath = fullPath;
        Name = Path.GetFileName(fullPath) is { Length: > 0 } name ? name : fullPath;
    }

    public IEnumerable<IFileSystemNode> GetChildren()
    {
        if (!Directory.Exists(_fullPath))
            yield break;

        foreach (string dir in Directory.EnumerateDirectories(_fullPath))
            yield return new LocalDirectoryNode(dir);

        foreach (string file in Directory.EnumerateFiles(_fullPath))
            yield return new LocalFileNode(file);
    }

    public IFileSystemNode? FindChild(string name)
    {
        string dirPath = Path.Combine(_fullPath, name);
        if (Directory.Exists(dirPath)) return new LocalDirectoryNode(dirPath);

        string filePath = Path.Combine(_fullPath, name);
        if (File.Exists(filePath)) return new LocalFileNode(filePath);

        return null;
    }
}
