namespace Train_Reservation_System_CLI.Models;

public class BookingRequest
{
    public string From { get; set; }
    public string To { get; set; }
    public DateOnly Date { get; set; }
    public CoachType CoachType { get; set; }
    public int NoOfSeats { get; set; }
}