namespace Lab1.Models;

public abstract record SegmentPassResult;

public record SegmentPassSuccess(double Time) : SegmentPassResult;

public record SegmentPassFailure : SegmentPassResult;
