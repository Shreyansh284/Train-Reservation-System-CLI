namespace Train_Reservation_System_CLI.Models;

public class CancalledTicketInfo
{
    public CoachType CoachType;
    public List<Seat> ConfirmedCancelledSeats = new();
    public DateOnly Date;
    public string From;
    public string To;
    public int TrainNumber;
    public int WaitingCancelledSeats;

    public CancalledTicketInfo(int trainNumber, string from, string to, CoachType coachType, DateOnly date,
        List<Seat> confirmedCancelledSeats, int waitingCancelledSeats)
    {
        TrainNumber = trainNumber;
        From = from;
        To = to;
        CoachType = coachType;
        Date = date;
        ConfirmedCancelledSeats = confirmedCancelledSeats;
        WaitingCancelledSeats = waitingCancelledSeats;
    }
}