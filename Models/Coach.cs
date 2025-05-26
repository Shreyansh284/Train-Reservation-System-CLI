namespace Train_Reservation_System_CLI.Models;

public class Coach
{
    public string CoachID;
    public CoachType CoachType;
    public List<Seat> Seats;
    public int TotalSeats;

    public Coach(string coachID, CoachType coachType, int totalSeats)
    {
        CoachID = coachID;
        CoachType = coachType;
        TotalSeats = totalSeats;
        Seats = new List<Seat>();
    }

  public List<Seat> GetReservedSeats(DateOnly date)
    {
        return Seats.Where(s => s.Reservations.Any(r => r.Date == date)).ToList();
    }

    public int AvailableSeatsCount(DateOnly date)
    {
        int reservedSeats = GetReservedSeats(date).Count;
        return TotalSeats - reservedSeats;
    }
}