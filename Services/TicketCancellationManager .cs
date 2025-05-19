using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Train_Reservation_System_CLI.Models;
using Train_Reservation_System_CLI.Services.IOHandlers;
using Train_Reservation_System_CLI.Execptions;

namespace Train_Reservation_System_CLI.Services
{
    internal class TicketCancellationManager
    {
        private TicketManager ticketManager;
        private TrainManager trainManager;

        public TicketCancellationManager(TicketManager ticketManager, TrainManager trainManager)
        {
            this.ticketManager = ticketManager;
            this.trainManager = trainManager;
        }

        public void GetCancellationDetails()
        {
            string[] cancellationDetails = InputHandler.ReadString("Enter PNR Number & Number of Seats To Cancel Booking: (eg. 10000002 5)", 2);
            TicketCancellation(cancellationDetails);
        }

        public void TicketCancellation(string[] cancellationDetails)
        {
            var cancelledInfo = GetCancelledSeats(cancellationDetails);

            if (cancelledInfo.ConfirmedCancelledSeats.Count == 0)
            {
                OutputHandler.PrintMessage($"Your {cancelledInfo.WaitingCancelledSeats} Waiting Seats Are Cancelled Successfully");
            }
            else
            {
                OutputHandler.PrintMessage($"Your {cancelledInfo.ConfirmedCancelledSeats.Count} Confirmed Seats & {cancelledInfo.WaitingCancelledSeats} Waiting Seats Are Cancelled Successfully");
            }

            AssignCancelledSeats(cancelledInfo);
        }

        public void AssignCancelledSeats(CancalledTicketInfo info)
        {
            Train train = trainManager.Trains.FirstOrDefault(t => t.TrainNumber == info.TrainNumber);
            if (train == null) return;

            if (!train.Waitlist.Waitlists.TryGetValue(info.CoachType, out List<Ticket> waitlist))
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
                    string seatList = string.Join(", ", assignedSeats);
                    OutputHandler.PrintMessage($"Ticket with PNR {waitTicket.PNR} has been assigned seats: {seatList} from the waitlist.");
                }
            }

        }

        public CancalledTicketInfo GetCancelledSeats(string[] details)
        {
            int PNR = int.Parse(details[0]);
            int seatsToCancel = int.Parse(details[1]);

            if (!ticketManager.Tickets.TryGetValue(PNR, out Ticket ticket))
                throw new InvalidInputExecption(OutputHandler.ErrorInvalidPNR(PNR));

            if (ticket.TotalNoOfSeats < seatsToCancel)
                throw new InvalidInputExecption(OutputHandler.ErrorInvalidSeatCount(seatsToCancel));

            Train train = trainManager.Trains.FirstOrDefault(t => t.TrainNumber == ticket.TrainNumber);
            var cancelledSeats = new List<Seat>();

            int waitingToCancel = Math.Min(ticket.WaitingSeats, seatsToCancel);
            ticket.WaitingSeats -= waitingToCancel;

            if (ticket.WaitingSeats == 0)
            {
                train.Waitlist.Waitlists[ticket.CoachType].Remove(ticket);
            }

            int confirmedToCancel = seatsToCancel - waitingToCancel;
            for (int i = 0; i < confirmedToCancel; i++)
            {
                var seat = ticket.BookedSeats[^1];
                seat.IsBooked = false;
                cancelledSeats.Add(seat);
                ticket.BookedSeats.RemoveAt(ticket.BookedSeats.Count - 1);
            }

            ticket.TotalNoOfSeats -= seatsToCancel;
            ticket.Fare = FareCalculator.CalculateFare(train.Route.GetDistance(ticket.From, ticket.To), ticket.CoachType, ticket.TotalNoOfSeats);

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

}
