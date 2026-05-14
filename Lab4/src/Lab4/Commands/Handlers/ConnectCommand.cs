using Lab4.Application;
using Lab4.FileSystem.Local;
using Lab4.Output;

namespace Lab4.Commands.Handlers;

public class ConnectCommand : ICommand
{
    private readonly string _address;
    private readonly string _mode;

    public ConnectCommand(string address, string mode = "local")
    {
        _address = address;
        _mode = mode;
    }

    public CommandResult Execute(FileSystemSession? session, IOutputWriter output)
    {
        if (!Path.IsPathRooted(_address))
            return new CommandFailure("connect requires an absolute path.");

        if (!Directory.Exists(_address))
            return new CommandFailure($"Directory not found: {_address}");

        var fileSystem = _mode switch
        {
            "local" => (Lab4.FileSystem.IFileSystem)new LocalFileSystem(),
            _ => null,
        };

        if (fileSystem is null)
            return new CommandFailure($"Unknown mode: {_mode}");

        output.WriteLine($"Connected to {_address}");
        return new ConnectSuccess(new FileSystemSession(fileSystem, _address));
    }
}

public record ConnectSuccess(FileSystemSession Session) : CommandResult;
