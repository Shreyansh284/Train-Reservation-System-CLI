namespace Train_Reservation_System_CLI.Models;

public class Waitlist
{
    public Waitlist()
    {
        Waitlists[CoachType.SL] = new List<Ticket>();
        Waitlists[CoachType.A3] = new List<Ticket>();
        Waitlists[CoachType.A2] = new List<Ticket>();
        Waitlists[CoachType.A1] = new List<Ticket>();
    }

    public Dictionary<CoachType, List<Ticket>> Waitlists { get; set; } = new();
}