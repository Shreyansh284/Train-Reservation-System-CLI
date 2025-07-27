namespace Train_Reservation_System_CLI.Models;

public class RouteStation(string stationName, int distanceFromStart)
{
    public string StationName { get; } = stationName;
    public int DistanceFromStart { get; } = distanceFromStart;
}