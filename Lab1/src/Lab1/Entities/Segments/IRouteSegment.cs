using Lab1.Models;

namespace Lab1.Entities.Segments;

public interface IRouteSegment
{
    SegmentPassResult TryPass(Train train);
}
