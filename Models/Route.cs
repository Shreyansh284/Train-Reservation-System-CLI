namespace Train_Reservation_System_CLI.Models;

public class Route
{
    public List<RouteStation> RouteStations { get; } = new();

    public void AddRouteStation(RouteStation routeStation)
    {
        RouteStations.Add(routeStation);
    }

    public bool IsValidRoute(string from, string to)
    {
        var stationNames = RouteStations.Select(s => s.StationName).ToList();

        var fromIndex = stationNames.IndexOf(from);
        var toIndex = stationNames.IndexOf(to);

        return fromIndex >= 0 && toIndex >= 0 && fromIndex < toIndex;
    }

    public List<string> GetStations()
    {
        return RouteStations.Select(s => s.StationName).ToList();
    }

    public int GetDistance(string from, string to)
    {
        var fromStation = RouteStations.FirstOrDefault(s => s.StationName == from);
        var toStation = RouteStations.FirstOrDefault(s => s.StationName == to);
        return toStation.DistanceFromStart - fromStation.DistanceFromStart;
    }
}