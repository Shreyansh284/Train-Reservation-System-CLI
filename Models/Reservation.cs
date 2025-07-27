namespace Train_Reservation_System_CLI.Models;

public class Reservation(DateOnly date)

{
    public DateOnly Date { get; set; } = date;
    public List<BookedRoute> BookedRoutes { get; set; } = new();

    public void AddRequestedRoute(BookedRoute bookedRoute)
    {
        BookedRoutes.Add(bookedRoute);
    }

    public void RemoveRequestedRoute(BookedRoute bookedRoute)
    {
        BookedRoutes.Remove(bookedRoute);
    }

    public BookedRoute GetBookedRoute(string from, string to)
    {
        return BookedRoutes.Find(r => r.FromStation == from && r.ToStation == to);
    }
}