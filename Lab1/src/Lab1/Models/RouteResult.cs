namespace Lab1.Models;

public abstract record RouteResult;

public record RouteSuccess(double TotalTime) : RouteResult;

public record RouteFailure : RouteResult;
