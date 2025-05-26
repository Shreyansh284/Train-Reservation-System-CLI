namespace Train_Reservation_System_CLI.Models;

public class Reservation
{
    public DateOnly Date { get; set; }
    public List<BookedRoute> BookedRoutes { get; set; }

    public Reservation(DateOnly date)
    {
        Date = date;
        BookedRoutes = new List<BookedRoute>();
    }

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