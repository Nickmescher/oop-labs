namespace Lab4.Commands;

public abstract record CommandResult;

public record CommandSuccess(string? Message = null) : CommandResult;

public record CommandFailure(string Message) : CommandResult;
