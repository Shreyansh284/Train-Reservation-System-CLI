namespace Train_Reservation_System_CLI.Models;

public class BookedRoute(string fromStation, string toStation)
{
    public string FromStation { get; set; } = fromStation;
    public string ToStation { get; set; } = toStation;
}