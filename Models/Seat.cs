namespace Train_Reservation_System_CLI.Models;

public class Seat
{
    public string SeatNumber;
    public List<Reservation> Reservations ;

    public Seat(string seatNumber)
    {
        SeatNumber = seatNumber;
        Reservations = new List<Reservation>();
    }

    public void AddReservation(Reservation reservation)
    {
        Reservations.Add(reservation);
    }
}