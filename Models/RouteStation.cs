namespace Train_Reservation_System_CLI.Models;

public class RouteStation
{
    public RouteStation(string stationName, int distanceFromStart)
    {
        StationName = stationName;
        DistanceFromStart = distanceFromStart;
    }

    public string StationName { get; }
    public int DistanceFromStart { get; }
}