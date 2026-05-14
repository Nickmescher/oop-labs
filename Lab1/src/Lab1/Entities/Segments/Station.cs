using Lab1.Models;

namespace Lab1.Entities.Segments;

public class Station : IRouteSegment
{
    public double SpeedLimit { get; }
    public double BoardingFactor { get; }
    public double AlightingFactor { get; }

    public Station(double speedLimit, double boardingFactor, double alightingFactor)
    {
        SpeedLimit = speedLimit;
        BoardingFactor = boardingFactor;
        AlightingFactor = alightingFactor;
    }

    public SegmentPassResult TryPass(Train train)
    {
        if (train.Speed > SpeedLimit)
            return new SegmentPassFailure();

        double departureSpeed = train.Speed;
        train.Stop();
        train.SetDepartureSpeed(departureSpeed);

        double stationTime = BoardingFactor + AlightingFactor;
        return new SegmentPassSuccess(stationTime);
    }
}
