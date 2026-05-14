using Lab1.Models;

namespace Lab1.Entities;

public class Train
{
    public double Mass { get; }
    public double MaxForce { get; }
    public double Precision { get; }
    public double Speed { get; private set; }
    public double Acceleration { get; private set; }

    public Train(double mass, double maxForce, double precision)
    {
        Mass = mass;
        MaxForce = maxForce;
        Precision = precision;
        Speed = 0;
        Acceleration = 0;
    }

    public bool TryApplyForce(double force)
    {
        if (Math.Abs(force) > MaxForce) return false;
        Acceleration = force / Mass;
        return true;
    }

    public TraverseResult TryTraverseDistance(double distance)
    {
        double remaining = distance;
        double totalTime = 0;

        while (remaining > 0)
        {
            if (Speed == 0 && Acceleration == 0)
                return new TraverseFailure();

            double resultingSpeed = Speed + Acceleration * Precision;

            if (resultingSpeed <= 0)
                return new TraverseFailure();

            Speed = resultingSpeed;
            remaining -= Speed * Precision;
            totalTime += Precision;
        }

        return new TraverseSuccess(totalTime);
    }

    internal void Stop()
    {
        Speed = 0;
        Acceleration = 0;
    }

    internal void SetDepartureSpeed(double speed)
    {
        Speed = speed;
    }
}
