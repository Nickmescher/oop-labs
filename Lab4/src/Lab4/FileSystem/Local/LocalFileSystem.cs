namespace Lab4.FileSystem.Local;

public class LocalFileSystem : IFileSystem
{
    public IDirectoryNode GetRoot(string absolutePath)
        => new LocalDirectoryNode(absolutePath);

    public void MoveFile(string sourcePath, string destinationPath)
    {
        string destination = Directory.Exists(destinationPath)
            ? Path.Combine(destinationPath, Path.GetFileName(sourcePath))
            : destinationPath;

        if (File.Exists(destination))
            throw new IOException($"File already exists at destination: {destination}");

        File.Move(sourcePath, destination);
    }

    public void CopyFile(string sourcePath, string destinationPath)
    {
        string destination = Directory.Exists(destinationPath)
            ? Path.Combine(destinationPath, Path.GetFileName(sourcePath))
            : destinationPath;

        if (File.Exists(destination))
            throw new IOException($"File already exists at destination: {destination}");

        File.Copy(sourcePath, destination);
    }

    public void DeleteFile(string filePath)
        => File.Delete(filePath);

    public void RenameFile(string filePath, string newName)
    {
        string directory = Path.GetDirectoryName(filePath)!;
        string newPath = Path.Combine(directory, newName);

        if (File.Exists(newPath))
            throw new IOException($"File with name '{newName}' already exists.");

        File.Move(filePath, newPath);
    }
}
