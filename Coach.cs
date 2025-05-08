namespace Train_Reservation_System_CLI;

public class Coach
{
    public string CoachID;
    public CoachType CoachType;
    public int TotalSeats;
    public Dictionary<DateOnly, List<Seat>> SeatsByDate = new Dictionary<DateOnly, List<Seat>>();

    public Coach(string coachID, CoachType coachType, int totalSeats)
    {

        CoachID = coachID;
        CoachType = coachType;
        TotalSeats = totalSeats;
    }

    public int GetAvailableSeats(DateOnly date)
    {
        if (SeatsByDate.ContainsKey(date))
        {
            return TotalSeats - SeatsByDate[date].Count;
        }
        else
        {
            SeatsByDate.Add(date, new List<Seat>() );
            return TotalSeats - SeatsByDate[date].Count;
                ;
        }
    }
}
