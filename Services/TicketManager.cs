using Train_Reservation_System_CLI.IOHandlers;
using Train_Reservation_System_CLI.Models;
using Train_Reservation_System_CLI.Validators;
using static Train_Reservation_System_CLI.Utils.InputUtils;

namespace Train_Reservation_System_CLI.Services;

public class TicketManager
{
    private int PNR = 10000000;

    private readonly List<Ticket> _tickets = new();

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

        _tickets.Add(ticket);

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
        BookingValidator.ValidatePNR(_tickets, pnr);
        var ticket = GetTicketByPNR(pnr);
        OutputHandler.PrintMessage(ticket.ToString());
    }

    public void AddTicketInTrainWaitingList(Ticket ticket, Train train)
    {
        train.WaitingTickets.Add(ticket);
    }

    public Ticket GetTicketByPNR(int pnr)
    {
        BookingValidator.ValidatePNR(_tickets, pnr);
        return _tickets.Single(t => t.PNR == pnr);
    }

    public void GenerateBookingReport()
    {
        OutputHandler.DisplayReport(_tickets);
    }
}