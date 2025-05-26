namespace Train_Reservation_System_CLI.Models;

public class BookedRoute
{
    public string FromStation { get; set; }
   public string ToStation { get; set; }

    public BookedRoute(string fromStation, string toStation)
    {
        FromStation = fromStation;
        ToStation = toStation;
    }
}