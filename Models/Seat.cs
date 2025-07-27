namespace Train_Reservation_System_CLI.Models;

public class Seat(string seatNumber)
{
    public readonly string SeatNumber = seatNumber;
    public readonly List<Reservation> Reservations = new();

    public void AddReservation(Reservation reservation)
    {
        Reservations.Add(reservation);
    }
}