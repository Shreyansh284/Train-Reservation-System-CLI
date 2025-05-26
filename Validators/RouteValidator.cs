using Train_Reservation_System_CLI.Models;

namespace Train_Reservation_System_CLI.Validators;

public static class RouteValidator
{
    public static bool IsOverlapping(string fromStation, string toStation, List<string> stations,
        List<BookedRoute> bookedRoutes)
    {
        int fromIndex = stations.IndexOf(fromStation);
        int toIndex = stations.IndexOf(toStation);

        foreach (var bookedRoute in bookedRoutes)
        {
            int fromRouteIndex = stations.IndexOf(bookedRoute.FromStation);
            int toRouteIndex = stations.IndexOf(bookedRoute.ToStation);

            if (fromRouteIndex < toIndex && toRouteIndex > fromIndex)
            {
                return true;
            }
        }

        return false;
    }
}