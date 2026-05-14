namespace Lab4.FileSystem;

public interface IFileSystem
{
    IDirectoryNode GetRoot(string absolutePath);
    void MoveFile(string sourcePath, string destinationPath);
    void CopyFile(string sourcePath, string destinationPath);
    void DeleteFile(string filePath);
    void RenameFile(string filePath, string newName);
}
