namespace Train_Reservation_System_CLI;

public class Coach
{
    public string CoachID;
    public CoachType CoachType;
    public int TotalSeats;
    public Dictionary<DateOnly, int> SeatsByDate = new Dictionary<DateOnly, int>();

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
            return TotalSeats - SeatsByDate[date];
        }
        else
        {
            SeatsByDate.Add(date, 0);
            return TotalSeats - SeatsByDate[date];
        }
    }
}
