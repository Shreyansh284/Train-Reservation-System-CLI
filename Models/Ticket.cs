namespace Train_Reservation_System_CLI.Models;

public class Ticket(
    int pnr,
    int trainNumber,
    string from,
    string to,
    DateOnly date,
    CoachType coachType,
    int seatsInWaiting,
    List<Seat> bookedSeats,
    int totalSeats,
    double fare)
{
    public readonly List<Seat> BookedSeats = bookedSeats;
    public readonly CoachType CoachType = coachType;
    public DateOnly Date = date;
    public double Fare = fare;
    public readonly string From = from;
    public readonly int PNR = pnr;
    public readonly string To = to;
    public int TotalNoOfSeats = totalSeats;
    public readonly int TrainNumber = trainNumber;
    public int WaitingSeats = seatsInWaiting;


    public override string ToString()
    {
        return
            $"========== TICKET DETAILS ==========\n" +
            $"PNR              : {PNR}\n" +
            $"From             : {From}\n" +
            $"To               : {To}\n" +
            $"Coach Type       : {CoachType}\n" +
            $"Date             : {Date}\n" +
            $"Seats In Waiting : {WaitingSeats}\n" +
            $"Booked Seats     : {string.Join(", ", BookedSeats.Select(s => s.SeatNumber))}\n" +
            $"Total Seats      : {TotalNoOfSeats}\n" +
            $"Fare             : INR {Fare:F2}\n" +
            $"====================================";
    }
}