using Lab4.Application;
using Lab4.Output;

namespace Lab4.Commands.Handlers;

public class FileRenameCommand : ICommand
{
    public string Path { get; }
    public string NewName { get; }

    public FileRenameCommand(string path, string newName)
    {
        Path = path;
        NewName = newName;
    }

    public CommandResult Execute(FileSystemSession? session, IOutputWriter output)
    {
        if (session is null)
            return new CommandFailure("Not connected. Use 'connect' first.");

        if (NewName.Contains('/') || NewName.Contains('\\'))
            return new CommandFailure("Name must not contain path separators.");

        string absolutePath = session.ResolveAbsolutePath(Path);

        if (!File.Exists(absolutePath))
            return new CommandFailure($"File not found: {absolutePath}");

        try
        {
            session.FileSystem.RenameFile(absolutePath, NewName);
            output.WriteLine($"Renamed to '{NewName}'.");
            return new CommandSuccess();
        }
        catch (IOException ex)
        {
            return new CommandFailure(ex.Message);
        }
    }
}
