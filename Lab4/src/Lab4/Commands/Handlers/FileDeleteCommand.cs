using Lab4.Application;
using Lab4.Output;

namespace Lab4.Commands.Handlers;

public class FileDeleteCommand : ICommand
{
    public string Path { get; }

    public FileDeleteCommand(string path)
    {
        Path = path;
    }

    public CommandResult Execute(FileSystemSession? session, IOutputWriter output)
    {
        if (session is null)
            return new CommandFailure("Not connected. Use 'connect' first.");

        string absolutePath = session.ResolveAbsolutePath(Path);

        if (!File.Exists(absolutePath))
            return new CommandFailure($"File not found: {absolutePath}");

        session.FileSystem.DeleteFile(absolutePath);
        output.WriteLine($"Deleted '{absolutePath}'.");
        return new CommandSuccess();
    }
}
