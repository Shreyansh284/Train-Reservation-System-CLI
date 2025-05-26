namespace Train_Reservation_System_CLI.Models;

public class Waitlist
{
    public Ticket Ticket { get; set; }

    public Waitlist(Ticket ticket)
    {
        Ticket = ticket;
    }
}