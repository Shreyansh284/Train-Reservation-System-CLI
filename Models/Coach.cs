namespace Train_Reservation_System_CLI.Models;

public class Coach
{
    public string CoachID;
    public CoachType CoachType;
    public Dictionary<DateOnly, List<Seat>> SeatsByDate = new();
    public int TotalSeats;

    public Coach(string coachID, CoachType coachType, int totalSeats)
    {
        CoachID = coachID;
        CoachType = coachType;
        TotalSeats = totalSeats;
    }

    public int GetAvailableSeats(DateOnly date)
    {
        if (!SeatsByDate.ContainsKey(date)) SeatsByDate[date] = new List<Seat>();

        var bookedCount = SeatsByDate[date].Count(seat => seat.IsBooked);
        return TotalSeats - bookedCount;
    }
}