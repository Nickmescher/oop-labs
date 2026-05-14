using Lab1.Entities;
using Lab1.Entities.Segments;
using Lab1.Models;
using Xunit;

namespace Lab1.Tests;

public class RouteTests
{
    private static Train CreateTrain(double mass = 1000, double maxForce = 5000, double precision = 0.1)
        => new Train(mass, maxForce, precision);

    // Scenario 1: powered path (accelerates to route speed limit) + magnetic path -> success
    [Fact]
    public void Scenario1_AccelerationWithinLimit_ReturnsSuccess()
    {
        // Arrange
        var train = CreateTrain(mass: 1000, maxForce: 5000, precision: 0.1);
        var route = new Route(
            new List<IRouteSegment>
            {
                new PoweredMagneticPath(length: 100, force: 3000),
                new MagneticPath(length: 50),
            },
            speedLimit: 100);

        // Act
        RouteResult result = route.TryRun(train);

        // Assert
        Assert.IsType<RouteSuccess>(result);
    }

    // Scenario 2: powered path accelerates train above route speed limit -> failure
    [Fact]
    public void Scenario2_AccelerationExceedsRouteLimit_ReturnsFailure()
    {
        // Arrange
        var train = CreateTrain(mass: 1000, maxForce: 5000, precision: 0.1);
        var route = new Route(
            new List<IRouteSegment>
            {
                new PoweredMagneticPath(length: 200, force: 5000),
                new MagneticPath(length: 50),
            },
            speedLimit: 5); // very low limit

        // Act
        RouteResult result = route.TryRun(train);

        // Assert
        Assert.IsType<RouteFailure>(result);
    }

    // Scenario 3: powered path + magnetic + station (within speed limit) + magnetic -> success
    [Fact]
    public void Scenario3_StationWithinSpeedLimit_ReturnsSuccess()
    {
        // Arrange
        var train = CreateTrain(mass: 1000, maxForce: 5000, precision: 0.1);
        var station = new Station(speedLimit: 100, boardingFactor: 2, alightingFactor: 3);
        var route = new Route(
            new List<IRouteSegment>
            {
                new PoweredMagneticPath(length: 100, force: 2000),
                new MagneticPath(length: 50),
                station,
                new MagneticPath(length: 50),
            },
            speedLimit: 100);

        // Act
        RouteResult result = route.TryRun(train);

        // Assert
        Assert.IsType<RouteSuccess>(result);
    }

    // Scenario 4: powered path accelerates above station speed limit -> failure at station
    [Fact]
    public void Scenario4_SpeedExceedsStationLimit_ReturnsFailure()
    {
        // Arrange
        var train = CreateTrain(mass: 100, maxForce: 5000, precision: 0.1);
        var station = new Station(speedLimit: 5, boardingFactor: 1, alightingFactor: 1);
        var route = new Route(
            new List<IRouteSegment>
            {
                new PoweredMagneticPath(length: 100, force: 5000),
                station,
                new MagneticPath(length: 50),
            },
            speedLimit: 1000);

        // Act
        RouteResult result = route.TryRun(train);

        // Assert
        Assert.IsType<RouteFailure>(result);
    }

    // Scenario 5: accelerates above route limit but within station limit -> failure (route limit exceeded at end)
    [Fact]
    public void Scenario5_ExceedsRouteLimitAfterStation_ReturnsFailure()
    {
        // Arrange
        var train = CreateTrain(mass: 100, maxForce: 5000, precision: 0.1);
        var station = new Station(speedLimit: 200, boardingFactor: 1, alightingFactor: 1);
        var route = new Route(
            new List<IRouteSegment>
            {
                new PoweredMagneticPath(length: 100, force: 5000),
                new MagneticPath(length: 50),
                station,
                new MagneticPath(length: 50),
            },
            speedLimit: 3); // very low route limit at end

        // Act
        RouteResult result = route.TryRun(train);

        // Assert
        Assert.IsType<RouteFailure>(result);
    }

    // Scenario 6: complex route with braking -> success
    [Fact]
    public void Scenario6_ComplexRouteWithBraking_ReturnsSuccess()
    {
        // Arrange
        var train = CreateTrain(mass: 1000, maxForce: 5000, precision: 0.1);
        double stationSpeedLimit = 30;
        double routeSpeedLimit = 30;
        var station = new Station(speedLimit: stationSpeedLimit, boardingFactor: 1, alightingFactor: 1);

        var route = new Route(
            new List<IRouteSegment>
            {
                new PoweredMagneticPath(length: 50, force: 4000),  // accelerate above station limit
                new MagneticPath(length: 20),
                new PoweredMagneticPath(length: 50, force: -4000), // brake to within station limit
                station,
                new MagneticPath(length: 20),
                new PoweredMagneticPath(length: 50, force: 4000),  // accelerate above route limit
                new MagneticPath(length: 20),
                new PoweredMagneticPath(length: 50, force: -4000), // brake to within route limit
            },
            speedLimit: routeSpeedLimit);

        // Act
        RouteResult result = route.TryRun(train);

        // Assert
        Assert.IsType<RouteSuccess>(result);
    }

    // Scenario 7: only magnetic path, train has no speed/acceleration -> failure
    [Fact]
    public void Scenario7_NoForceApplied_ReturnsFailure()
    {
        // Arrange
        var train = CreateTrain();
        var route = new Route(
            new List<IRouteSegment>
            {
                new MagneticPath(length: 100),
            },
            speedLimit: 1000);

        // Act
        RouteResult result = route.TryRun(train);

        // Assert
        Assert.IsType<RouteFailure>(result);
    }

    // Scenario 8: force Y then force -2Y where 2Y exceeds maxForce -> failure
    [Fact]
    public void Scenario8_BrakingForceExceedsMaxForce_ReturnsFailure()
    {
        // Arrange: maxForce = Y, so applying -2Y will fail
        double forceY = 3000;
        var train = CreateTrain(mass: 1000, maxForce: forceY, precision: 0.1);
        var route = new Route(
            new List<IRouteSegment>
            {
                new PoweredMagneticPath(length: 100, force: forceY),
                new PoweredMagneticPath(length: 100, force: -2 * forceY),
            },
            speedLimit: 1000);

        // Act
        RouteResult result = route.TryRun(train);

        // Assert
        Assert.IsType<RouteFailure>(result);
    }

    [Fact]
    public void RouteSuccess_ContainsTotalTime()
    {
        // Arrange
        var train = CreateTrain(mass: 1000, maxForce: 5000, precision: 0.1);
        var route = new Route(
            new List<IRouteSegment>
            {
                new PoweredMagneticPath(length: 50, force: 3000),
            },
            speedLimit: 1000);

        // Act
        RouteResult result = route.TryRun(train);

        // Assert
        var success = Assert.IsType<RouteSuccess>(result);
        Assert.True(success.TotalTime > 0);
    }

    [Fact]
    public void Station_StopsAndReacceleratesToArrivalSpeed()
    {
        // Arrange
        var train = CreateTrain(mass: 1000, maxForce: 5000, precision: 0.1);
        var station = new Station(speedLimit: 1000, boardingFactor: 2, alightingFactor: 3);

        // First apply some force so train has speed
        train.TryApplyForce(3000);
        train.TryTraverseDistance(50);
        double speedBeforeStation = train.Speed;

        // Act
        SegmentPassResult result = station.TryPass(train);

        // Assert
        Assert.IsType<SegmentPassSuccess>(result);
        Assert.Equal(5.0, ((SegmentPassSuccess)result).Time);
        Assert.Equal(speedBeforeStation, train.Speed);
    }
}
