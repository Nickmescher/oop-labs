using Lab4.Application;
using Lab4.Output;

namespace Lab4.Commands.Handlers;

public class FileShowCommand : ICommand
{
    public string Path { get; }
    public string Mode { get; }

    public FileShowCommand(string path, string mode)
    {
        Path = path;
        Mode = mode;
    }

    public CommandResult Execute(FileSystemSession? session, IOutputWriter output)
    {
        if (session is null)
            return new CommandFailure("Not connected. Use 'connect' first.");

        string absolutePath = session.ResolveAbsolutePath(Path);

        if (!File.Exists(absolutePath))
            return new CommandFailure($"File not found: {absolutePath}");

        string content = File.ReadAllText(absolutePath);
        output.WriteLine(content);
        return new CommandSuccess();
    }
}
