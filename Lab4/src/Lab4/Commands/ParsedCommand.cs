namespace Lab4.Commands;

public record ParsedCommand(
    string Name,
    string? SubCommand,
    IReadOnlyList<CommandArgument> Arguments);
