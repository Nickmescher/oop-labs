namespace Lab2;

public abstract record MarkReadResult;

public record MarkReadSuccess : MarkReadResult;

public record MarkReadFailure(string Reason) : MarkReadResult;
