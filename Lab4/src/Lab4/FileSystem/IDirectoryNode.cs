namespace Lab4.FileSystem;

public interface IDirectoryNode : IFileSystemNode
{
    IEnumerable<IFileSystemNode> GetChildren();
    IFileSystemNode? FindChild(string name);
}
