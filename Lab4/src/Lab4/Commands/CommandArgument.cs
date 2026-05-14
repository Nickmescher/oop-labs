namespace Lab4.Commands;

public abstract record CommandArgument;

public record PositionalArgument(string Value) : CommandArgument;

public record FlagArgument(string Flag, string? Value) : CommandArgument;
