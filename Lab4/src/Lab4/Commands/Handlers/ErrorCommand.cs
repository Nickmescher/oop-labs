using Lab4.Application;
using Lab4.Output;

namespace Lab4.Commands.Handlers;

public class ErrorCommand : ICommand
{
    private readonly string _message;

    public ErrorCommand(string message)
    {
        _message = message;
    }

    public CommandResult Execute(FileSystemSession? session, IOutputWriter output)
        => new CommandFailure(_message);
}
