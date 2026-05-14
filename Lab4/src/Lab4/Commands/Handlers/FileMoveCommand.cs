using Lab4.Application;
using Lab4.Output;

namespace Lab4.Commands.Handlers;

public class FileMoveCommand : ICommand
{
    public string SourcePath { get; }
    public string DestinationPath { get; }

    public FileMoveCommand(string sourcePath, string destinationPath)
    {
        SourcePath = sourcePath;
        DestinationPath = destinationPath;
    }

    public CommandResult Execute(FileSystemSession? session, IOutputWriter output)
    {
        if (session is null)
            return new CommandFailure("Not connected. Use 'connect' first.");

        string source = session.ResolveAbsolutePath(SourcePath);
        string destination = session.ResolveAbsolutePath(DestinationPath);

        if (!File.Exists(source))
            return new CommandFailure($"File not found: {source}");

        try
        {
            session.FileSystem.MoveFile(source, destination);
            output.WriteLine($"Moved '{source}' to '{destination}'.");
            return new CommandSuccess();
        }
        catch (IOException ex)
        {
            return new CommandFailure(ex.Message);
        }
    }
}
