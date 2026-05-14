using Lab4.FileSystem;

namespace Lab4.Application;

public class FileSystemSession
{
    private readonly List<string> _localPathSegments = new();

    public IFileSystem FileSystem { get; }
    public string ConnectionPath { get; }

    public FileSystemSession(IFileSystem fileSystem, string connectionPath)
    {
        FileSystem = fileSystem;
        ConnectionPath = connectionPath;
    }

    public string LocalPath
        => _localPathSegments.Count == 0
            ? string.Empty
            : string.Join(Path.DirectorySeparatorChar, _localPathSegments);

    public string CurrentAbsolutePath
        => LocalPath.Length == 0
            ? ConnectionPath
            : Path.Combine(ConnectionPath, LocalPath);

    public string ResolveAbsolutePath(string path)
    {
        if (Path.IsPathRooted(path))
            return Path.GetFullPath(Path.Combine(ConnectionPath, path.TrimStart('/', '\\')));

        return Path.GetFullPath(Path.Combine(CurrentAbsolutePath, path));
    }

    public NavigateResult TryNavigate(string path)
    {
        string targetAbsolute = ResolveAbsolutePath(path);

        if (!targetAbsolute.StartsWith(ConnectionPath, StringComparison.OrdinalIgnoreCase))
            return new NavigateFailure("Cannot navigate outside the connection root.");

        if (!Directory.Exists(targetAbsolute))
            return new NavigateFailure($"Directory not found: {targetAbsolute}");

        string relative = Path.GetRelativePath(ConnectionPath, targetAbsolute);
        _localPathSegments.Clear();

        if (relative != ".")
            _localPathSegments.AddRange(relative.Split(Path.DirectorySeparatorChar));

        return new NavigateSuccess();
    }
}

public abstract record NavigateResult;
public record NavigateSuccess : NavigateResult;
public record NavigateFailure(string Reason) : NavigateResult;
