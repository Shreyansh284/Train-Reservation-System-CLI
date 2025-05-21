namespace Train_Reservation_System_CLI.Models;

public class Ticket
{
    public List<Seat> BookedSeats;
    public CoachType CoachType;
    public DateOnly Date;
    public double Fare;
    public string From;
    public int PNR;
    public string To;
    public int TotalNoOfSeats;
    public int TrainNumber;
    public int WaitingSeats;

    public Ticket(int pnr, int trainNumber, string from, string to, DateOnly date, CoachType coachType,
        int seatsInWaiting, List<Seat> bookedSeats, int totalSeats, double fare)
    {
        PNR = pnr;
        TrainNumber = trainNumber;
        From = from;
        To = to;
        Date = date;
        CoachType = coachType;
        WaitingSeats = seatsInWaiting;
        BookedSeats = bookedSeats;
        TotalNoOfSeats = totalSeats;
        Fare = fare;
    }

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