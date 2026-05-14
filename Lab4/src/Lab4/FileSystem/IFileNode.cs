namespace Lab4.FileSystem;

public interface IFileNode : IFileSystemNode
{
    string ReadContent();
}
