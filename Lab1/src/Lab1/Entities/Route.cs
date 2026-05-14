using Lab1.Entities.Segments;
using Lab1.Models;

namespace Lab1.Entities;

public class Route
{
    private readonly IReadOnlyList<IRouteSegment> _segments;
    public double SpeedLimit { get; }

    public Route(IReadOnlyList<IRouteSegment> segments, double speedLimit)
    {
        _segments = segments;
        SpeedLimit = speedLimit;
    }

    public RouteResult TryRun(Train train)
    {
        double totalTime = 0;

        foreach (IRouteSegment segment in _segments)
        {
            SegmentPassResult result = segment.TryPass(train);

            if (result is SegmentPassFailure)
                return new RouteFailure();

            totalTime += ((SegmentPassSuccess)result).Time;
        }

        if (train.Speed > SpeedLimit)
            return new RouteFailure();

        return new RouteSuccess(totalTime);
    }
}
