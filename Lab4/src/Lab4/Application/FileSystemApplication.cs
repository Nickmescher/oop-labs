using Lab4.Commands;
using Lab4.Commands.Handlers;
using Lab4.Output;

namespace Lab4.Application;

public class FileSystemApplication
{
    private readonly CommandParser _parser;
    private readonly IOutputWriter _output;
    private readonly ITreeRenderer _treeRenderer;
    private FileSystemSession? _session;

    public FileSystemApplication(CommandParser parser, IOutputWriter output, ITreeRenderer treeRenderer)
    {
        _parser = parser;
        _output = output;
        _treeRenderer = treeRenderer;
    }

    public void Run()
    {
        while (true)
        {
            string prompt = _session is null ? "> " : $"{_session.LocalPath}> ";
            Console.Write(prompt);

            string? input = Console.ReadLine();
            if (input is null) break;

            ICommand? command = BuildCommand(input);

            if (command is null)
            {
                _output.WriteLine("Unknown command.");
                continue;
            }

            CommandResult result = command.Execute(_session, _output);

            switch (result)
            {
                case ConnectSuccess connect:
                    _session = connect.Session;
                    break;
                case DisconnectSuccess:
                    _session = null;
                    break;
                case CommandFailure failure:
                    _output.WriteLine($"Error: {failure.Message}");
                    break;
            }
        }
    }

    private ICommand? BuildCommand(string input)
    {
        ParsedCommand? parsed = _parser.Parse(input);
        if (parsed is null) return null;

        return parsed.Name switch
        {
            "connect" => BuildConnectCommand(parsed),
            "disconnect" => new DisconnectCommand(),
            "tree" => BuildTreeCommand(parsed),
            "file" => BuildFileCommand(parsed),
            _ => null,
        };
    }

    private static ICommand BuildConnectCommand(ParsedCommand parsed)
    {
        string? address = parsed.Arguments.OfType<PositionalArgument>().FirstOrDefault()?.Value;
        string mode = parsed.Arguments.OfType<FlagArgument>()
            .FirstOrDefault(f => f.Flag == "-m")?.Value ?? "local";

        if (address is null) return new ErrorCommand("connect requires an address.");
        return new ConnectCommand(address, mode);
    }

    private ICommand BuildTreeCommand(ParsedCommand parsed)
    {
        return parsed.SubCommand switch
        {
            "goto" => BuildTreeGotoCommand(parsed),
            "list" => BuildTreeListCommand(parsed),
            _ => new ErrorCommand($"Unknown tree subcommand: {parsed.SubCommand}"),
        };
    }

    private static ICommand BuildTreeGotoCommand(ParsedCommand parsed)
    {
        string? path = parsed.Arguments.OfType<PositionalArgument>().FirstOrDefault()?.Value;
        if (path is null) return new ErrorCommand("tree goto requires a path.");
        return new TreeGotoCommand(path);
    }

    private ICommand BuildTreeListCommand(ParsedCommand parsed)
    {
        string? depthStr = parsed.Arguments.OfType<FlagArgument>()
            .FirstOrDefault(f => f.Flag == "-d")?.Value;

        int depth = depthStr is not null && int.TryParse(depthStr, out int d) ? d : 1;
        return new TreeListCommand(depth, _treeRenderer);
    }

    private static ICommand BuildFileCommand(ParsedCommand parsed)
    {
        return parsed.SubCommand switch
        {
            "show" => BuildFileShowCommand(parsed),
            "move" => BuildFileMoveCommand(parsed),
            "copy" => BuildFileCopyCommand(parsed),
            "delete" => BuildFileDeleteCommand(parsed),
            "rename" => BuildFileRenameCommand(parsed),
            _ => new ErrorCommand($"Unknown file subcommand: {parsed.SubCommand}"),
        };
    }

    private static ICommand BuildFileShowCommand(ParsedCommand parsed)
    {
        string? path = parsed.Arguments.OfType<PositionalArgument>().FirstOrDefault()?.Value;
        string mode = parsed.Arguments.OfType<FlagArgument>()
            .FirstOrDefault(f => f.Flag == "-m")?.Value ?? "console";
        if (path is null) return new ErrorCommand("file show requires a path.");
        return new FileShowCommand(path, mode);
    }

    private static ICommand BuildFileMoveCommand(ParsedCommand parsed)
    {
        var positionals = parsed.Arguments.OfType<PositionalArgument>().ToList();
        if (positionals.Count < 2) return new ErrorCommand("file move requires source and destination.");
        return new FileMoveCommand(positionals[0].Value, positionals[1].Value);
    }

    private static ICommand BuildFileCopyCommand(ParsedCommand parsed)
    {
        var positionals = parsed.Arguments.OfType<PositionalArgument>().ToList();
        if (positionals.Count < 2) return new ErrorCommand("file copy requires source and destination.");
        return new FileCopyCommand(positionals[0].Value, positionals[1].Value);
    }

    private static ICommand BuildFileDeleteCommand(ParsedCommand parsed)
    {
        string? path = parsed.Arguments.OfType<PositionalArgument>().FirstOrDefault()?.Value;
        if (path is null) return new ErrorCommand("file delete requires a path.");
        return new FileDeleteCommand(path);
    }

    private static ICommand BuildFileRenameCommand(ParsedCommand parsed)
    {
        var positionals = parsed.Arguments.OfType<PositionalArgument>().ToList();
        if (positionals.Count < 2) return new ErrorCommand("file rename requires a path and a name.");
        return new FileRenameCommand(positionals[0].Value, positionals[1].Value);
    }
}
