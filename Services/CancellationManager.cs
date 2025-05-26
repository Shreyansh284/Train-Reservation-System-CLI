using Train_Reservation_System_CLI.Execptions;
using Train_Reservation_System_CLI.IOHandlers;
using Train_Reservation_System_CLI.Models;
using static Train_Reservation_System_CLI.Utils.InputUtils;
using static Train_Reservation_System_CLI.Validators.RouteValidator;

namespace Train_Reservation_System_CLI.Services;

internal class CancellationManager
{
    private readonly TicketManager ticketManager;

    public CancellationManager(TicketManager ticketManager)
    {
        this.ticketManager = ticketManager;
    }

    public void HandleCancellation()
    {
        var input = InputHandler.ReadInput("Enter PNR Number & Number of Seats To Cancel Booking: (e.g., 10000002 5)");
        var details = SplitInput(input);

        var record = ProcessCancellation(details);

        DisplayCancellationMessage(record);
        ReassignCancelledSeatsToWaitlist(record);
    }

    private CancellationRecord ProcessCancellation(string[] details)
    {
        int pnr = ParseInt(details[0]);
        int seatsToCancel = ParseInt(details[1]);

        var ticket = ticketManager.GetTicketByPNR(pnr)
                     ?? throw new InvalidInputExecption(OutputHandler.ErrorInvalidPNR(pnr));

        if (seatsToCancel > ticket.TotalNoOfSeats)
            throw new InvalidInputExecption(OutputHandler.ErrorInvalidSeatCount(seatsToCancel));

        var train = TrainManager.Trains.First(t => t.TrainNumber == ticket.TrainNumber);

        // Cancel waiting seats first, then confirmed ones
        int waitingToCancel = CancelWaitingSeats(ticket, train, seatsToCancel);
        int remainingSeatsToCancel = seatsToCancel - waitingToCancel;

        var confirmedCancelledSeats = CancelConfirmedSeats(ticket, remainingSeatsToCancel);

        ticket.TotalNoOfSeats -= seatsToCancel;
        ticket.Fare = FareCalculator.CalculateFare(
            train.Route.GetDistance(ticket.From, ticket.To),
            ticket.CoachType,
            ticket.TotalNoOfSeats
        );

        return new CancellationRecord(
            ticket.TrainNumber,
            ticket.From,
            ticket.To,
            ticket.CoachType,
            ticket.Date,
            confirmedCancelledSeats,
            waitingToCancel
        );
    }

    private int CancelWaitingSeats(Ticket ticket, Train train, int seatsToCancel)
    {
        int waitingToCancel = Math.Min(ticket.WaitingSeats, seatsToCancel);
        ticket.WaitingSeats -= waitingToCancel;

        if (ticket.WaitingSeats == 0)
            train.WaitingTickets.Remove(ticket);

        return waitingToCancel;
    }

    private List<Seat> CancelConfirmedSeats(Ticket ticket, int seatsToCancel)
    {
        var cancelledSeats = new List<Seat>();

        for (int i = 0; i < seatsToCancel && ticket.BookedSeats.Count > 0; i++)
        {
            int lastIndex = ticket.BookedSeats.Count - 1;
            var seat = ticket.BookedSeats[lastIndex];
            ticket.BookedSeats.RemoveAt(lastIndex);

            var reservation = seat.Reservations.Single(r => r.Date == ticket.Date);
            reservation.RemoveRequestedRoute(reservation.GetBookedRoute(ticket.From, ticket.To));

            cancelledSeats.Add(seat);
        }

        return cancelledSeats;
    }

    private void DisplayCancellationMessage(CancellationRecord record)
    {
        string message = record.ConfirmedCancelledSeats.Count == 0
            ? $"Your {record.WaitingCancelledSeats} Waiting Seats Are Cancelled Successfully"
            : $"Your {record.ConfirmedCancelledSeats.Count} Confirmed Seats & {record.WaitingCancelledSeats} Waiting Seats Are Cancelled Successfully";

        OutputHandler.PrintMessage(message);
    }

    private void ReassignCancelledSeatsToWaitlist(CancellationRecord record)
    {
        var train = TrainManager.Trains.FirstOrDefault(t => t.TrainNumber == record.TrainNumber);
        if (train == null) return;

        var stations = train.Route.GetStations();
        var waitlist = train.GetWaitlistByCoachTypeAndDate(record.CoachType, record.Date);
        var availableSeats = new List<Seat>(record.ConfirmedCancelledSeats);

        foreach (var ticket in waitlist)
        {
            List<string> assignedSeats = new();

            foreach (var seat in availableSeats)
            {
                if (ticket.WaitingSeats <= 0) break;

                var reservation = seat.Reservations.Single(r => r.Date == record.Date);
                if (!IsOverlapping(ticket.From, ticket.To, stations, reservation.BookedRoutes))
                {
                    reservation.AddRequestedRoute(new BookedRoute(ticket.From, ticket.To));
                    ticket.BookedSeats.Add(seat);
                    ticket.WaitingSeats--;
                    assignedSeats.Add(seat.SeatNumber);
                }
            }

            if (assignedSeats.Any())
            {
                OutputHandler.PrintMessage(
                    $"Ticket with PNR {ticket.PNR} has been assigned seats: {string.Join(", ", assignedSeats)} from the waitlist.");
            }
        }
    }
}