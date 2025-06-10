namespace Train_Reservation_System_CLI.Models;

public class CancellationRecord(
    int trainNumber,
    string from,
    string to,
    CoachType coachType,
    DateOnly date,
    List<Seat> confirmedCancelledSeats,
    int waitingCancelledSeats)
{
    public CoachType CoachType = coachType;
    public List<Seat> ConfirmedCancelledSeats = confirmedCancelledSeats;
    public DateOnly Date = date;
    public string From = from;
    public string To = to;
    public int TrainNumber = trainNumber;
    public int WaitingCancelledSeats = waitingCancelledSeats;
}