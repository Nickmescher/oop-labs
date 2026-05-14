using Lab4.Application;
using Lab4.Output;

namespace Lab4.Commands.Handlers;

public class DisconnectCommand : ICommand
{
    public CommandResult Execute(FileSystemSession? session, IOutputWriter output)
    {
        if (session is null)
            return new CommandFailure("Not connected.");

        output.WriteLine("Disconnected.");
        return new DisconnectSuccess();
    }
}

public record DisconnectSuccess : CommandResult;
