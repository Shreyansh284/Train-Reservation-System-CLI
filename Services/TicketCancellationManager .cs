using Train_Reservation_System_CLI.Execptions;
using Train_Reservation_System_CLI.IOHandlers;
using Train_Reservation_System_CLI.Models;
using static Train_Reservation_System_CLI.Utils.InputUtils;

namespace Train_Reservation_System_CLI.Services;

internal class TicketCancellationManager
{
    private readonly TicketManager ticketManager;
    private TrainManager trainManager;

    public TicketCancellationManager(TicketManager ticketManager, TrainManager trainManager)
    {
        this.ticketManager = ticketManager;
        this.trainManager = trainManager;
    }

    public void GetCancellationDetails()
    {
        var input = InputHandler.ReadInput("Enter PNR Number & Number of Seats To Cancel Booking: (eg. 10000002 5)");
        var cancellationDetails = SplitInput(input);

        TicketCancellation(cancellationDetails);
    }

    public void TicketCancellation(string[] cancellationDetails)
    {
        var cancelledInfo = GetCancelledSeats(cancellationDetails);

        if (cancelledInfo.ConfirmedCancelledSeats.Count == 0)
            OutputHandler.PrintMessage(
                $"Your {cancelledInfo.WaitingCancelledSeats} Waiting Seats Are Cancelled Successfully");
        else
            OutputHandler.PrintMessage(
                $"Your {cancelledInfo.ConfirmedCancelledSeats.Count} Confirmed Seats & {cancelledInfo.WaitingCancelledSeats} Waiting Seats Are Cancelled Successfully");

        AssignCancelledSeats(cancelledInfo);
    }

    public void AssignCancelledSeats(CancalledTicketInfo info)
    {
        var train = TrainManager.Trains.FirstOrDefault(t => t.TrainNumber == info.TrainNumber);
        if (train == null) return;

        if (!train.Waitlist.Waitlists.TryGetValue(info.CoachType, out var waitlist))
            return;

        var matchingTickets = waitlist
            .Where(w => w.CoachType == info.CoachType && w.Date == info.Date)
            .ToList();

        foreach (var waitTicket in matchingTickets)
        {
            List<string> assignedSeats = new();

            while (waitTicket.WaitingSeats > 0 && info.ConfirmedCancelledSeats.Count > 0)
            {
                waitTicket.WaitingSeats--;
                var seat = info.ConfirmedCancelledSeats[^1];
                seat.IsBooked = true;
                waitTicket.BookedSeats.Add(seat);
                assignedSeats.Add(seat.SeatNumber);
                info.ConfirmedCancelledSeats.RemoveAt(info.ConfirmedCancelledSeats.Count - 1);
            }

            if (assignedSeats.Count > 0)
            {
                var seatList = string.Join(", ", assignedSeats);
                OutputHandler.PrintMessage(
                    $"Ticket with PNR {waitTicket.PNR} has been assigned seats: {seatList} from the waitlist.");
            }
        }
    }

    public CancalledTicketInfo GetCancelledSeats(string[] details)
    {
        var PNR = ParseInt(details[0]);
        var seatsToCancel = ParseInt(details[1]);

        if (!ticketManager.Tickets.TryGetValue(PNR, out var ticket))
            throw new InvalidInputExecption(OutputHandler.ErrorInvalidPNR(PNR));

        if (ticket.TotalNoOfSeats < seatsToCancel)
            throw new InvalidInputExecption(OutputHandler.ErrorInvalidSeatCount(seatsToCancel));

        var train = TrainManager.Trains.FirstOrDefault(t => t.TrainNumber == ticket.TrainNumber);
        var cancelledSeats = new List<Seat>();

        var waitingToCancel = Math.Min(ticket.WaitingSeats, seatsToCancel);
        ticket.WaitingSeats -= waitingToCancel;

        if (ticket.WaitingSeats == 0) train.Waitlist.Waitlists[ticket.CoachType].Remove(ticket);

        var confirmedToCancel = seatsToCancel - waitingToCancel;
        for (var i = 0; i < confirmedToCancel; i++)
        {
            var seat = ticket.BookedSeats[^1];
            seat.IsBooked = false;
            cancelledSeats.Add(seat);
            ticket.BookedSeats.RemoveAt(ticket.BookedSeats.Count - 1);
        }

        ticket.TotalNoOfSeats -= seatsToCancel;
        ticket.Fare = FareCalculator.CalculateFare(train.Route.GetDistance(ticket.From, ticket.To), ticket.CoachType,
            ticket.TotalNoOfSeats);

        return new CancalledTicketInfo(
            ticket.TrainNumber,
            ticket.From,
            ticket.To,
            ticket.CoachType,
            ticket.Date,
            cancelledSeats,
            waitingToCancel
        );
    }
}