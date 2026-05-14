using Lab1.Models;

namespace Lab1.Entities.Segments;

public class MagneticPath : IRouteSegment
{
    public double Length { get; }

    public MagneticPath(double length)
    {
        Length = length;
    }

    public SegmentPassResult TryPass(Train train)
    {
        return train.TryTraverseDistance(Length) switch
        {
            TraverseSuccess s => new SegmentPassSuccess(s.Time),
            _ => new SegmentPassFailure(),
        };
    }
}
