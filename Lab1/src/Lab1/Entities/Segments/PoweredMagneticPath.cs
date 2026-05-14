using Lab1.Models;

namespace Lab1.Entities.Segments;

public class PoweredMagneticPath : IRouteSegment
{
    public double Length { get; }
    public double Force { get; }

    public PoweredMagneticPath(double length, double force)
    {
        Length = length;
        Force = force;
    }

    public SegmentPassResult TryPass(Train train)
    {
        if (!train.TryApplyForce(Force))
            return new SegmentPassFailure();

        return train.TryTraverseDistance(Length) switch
        {
            TraverseSuccess s => new SegmentPassSuccess(s.Time),
            _ => new SegmentPassFailure(),
        };
    }
}
