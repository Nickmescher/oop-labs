using Lab4.Application;
using Lab4.Output;

namespace Lab4.Commands.Handlers;

public class TreeGotoCommand : ICommand
{
    public string Path { get; }

    public TreeGotoCommand(string path)
    {
        Path = path;
    }

    public CommandResult Execute(FileSystemSession? session, IOutputWriter output)
    {
        if (session is null)
            return new CommandFailure("Not connected. Use 'connect' first.");

        NavigateResult result = session.TryNavigate(Path);

        if (result is NavigateFailure failure)
            return new CommandFailure(failure.Reason);

        output.WriteLine($"Current path: {session.CurrentAbsolutePath}");
        return new CommandSuccess();
    }
}
