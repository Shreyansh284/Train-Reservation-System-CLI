namespace Train_Reservation_System_CLI.Models;

public class Coach(string coachId, CoachType coachType, int totalSeats)
{
    public readonly string CoachId = coachId;
    public readonly CoachType CoachType = coachType;
    public readonly List<Seat> Seats = new();

    public List<Seat> GetReservedSeats(DateOnly date)
    {
        return Seats.Where(s => s.Reservations.Any(r => r.Date == date)).ToList();
    }

    public int AvailableSeatsCount(DateOnly date)
    {
        int reservedSeats = GetReservedSeats(date).Count;
        return totalSeats - reservedSeats;
    }
}