using Train_Reservation_System_CLI.Execptions;
using Train_Reservation_System_CLI.IOHandlers;
using Train_Reservation_System_CLI.Models;
using static Train_Reservation_System_CLI.Utils.InputUtils;

namespace Train_Reservation_System_CLI.Services;

public class TicketManager
{
    private int PNR = 10000000;

    public Dictionary<int, Ticket> Tickets = new();

    public Ticket GenerateTicket(BookingRequest bookingRequest, int trainNumber, int seatsInWaiting,
        List<Seat> bookedSeats, double fare)
    {
        var currentPNR = ++PNR;

        var ticket = new Ticket
        (
            currentPNR,
            trainNumber,
            bookingRequest.From,
            bookingRequest.To,
            bookingRequest.Date,
            bookingRequest.CoachType,
            seatsInWaiting,
            bookedSeats,
            bookingRequest.NoOfSeats,
            fare
        );

        Tickets.Add(currentPNR, ticket);

        return ticket;
    }

    public void DisplayTicket(Ticket ticket)
    {
        OutputHandler.PrintMessage("Your Booking Is Done");
        OutputHandler.PrintMessage(ticket.ToString());
    }

    public void GetBookingDetailsByPNR()
    {
        var input = InputHandler.ReadInput("Enter PNR Number For Getting Booking Details");
        var pnr = ParseInt(input);
        if (!Tickets.ContainsKey(pnr)) throw new InvalidInputExecption(OutputHandler.ErrorInvalidPNR(pnr));

        var ticket = Tickets[pnr];
        OutputHandler.PrintMessage(ticket.ToString());
    }

    public void AddTicketInWaitingList(Ticket ticket, Train train)
    {
        if (train.Waitlist.Waitlists.TryGetValue(ticket.CoachType, out var waitlist)) waitlist.Add(ticket);
    }

    public void GenerateBookingReport()
    {
        if (Tickets.Count == 0)
        {
            OutputHandler.PrintError("No bookings found.");
            return;
        }

        var sortedTickets = Tickets.Values.OrderBy(t => t.Date).ThenBy(t => t.PNR);

        const int chunkSize = 6;

        var header =
            $"{"PNR",-10} | {"TrainNo",-8} | {"From",-10} | {"To",-10} | {"Date",-12} | {"CoachType",-10} | {"Confirmed Seats",-40} | {"Waiting",-8} | {"Fare",-6}";
        var separator = new string('-', header.Length);

        Console.WriteLine("=========== BOOKING REPORT ===========");
        Console.WriteLine(header);
        Console.WriteLine(separator);

        foreach (var ticket in sortedTickets)
        {
            var seatChunks = FormatSeatChunks(ticket.BookedSeats, chunkSize);

            Console.WriteLine(
                $"{ticket.PNR,-10} | {ticket.TrainNumber,-8} | {ticket.From,-10} | {ticket.To,-10} | {ticket.Date.ToString("yyyy-MM-dd"),-12} | {ticket.CoachType,-10} | {seatChunks[0],-40} | {ticket.WaitingSeats,-8} | INR {ticket.Fare,-6:F2}"
            );

            for (var i = 1; i < seatChunks.Count; i++)
                Console.WriteLine(
                    $"{string.Empty,-10} | {string.Empty,-8} | {string.Empty,-10} | {string.Empty,-10} | {string.Empty,-12} | {string.Empty,-10} | {seatChunks[i],-40} | {string.Empty,-8} | {string.Empty,-6}"
                );
        }
    }

    private List<string> FormatSeatChunks(List<Seat> seats, int chunkSize)
    {
        if (seats == null || seats.Count == 0)
            return new List<string> { "-" };

        return seats
            .Select((seat, index) => new { seat.SeatNumber, index })
            .GroupBy(x => x.index / chunkSize)
            .Select(group => string.Join(", ", group.Select(x => x.SeatNumber)))
            .ToList();
    }
}