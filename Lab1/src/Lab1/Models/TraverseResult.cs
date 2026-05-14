namespace Lab1.Models;

public abstract record TraverseResult;

public record TraverseSuccess(double Time) : TraverseResult;

public record TraverseFailure : TraverseResult;
