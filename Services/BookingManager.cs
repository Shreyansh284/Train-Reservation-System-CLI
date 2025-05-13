using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Train_Reservation_System_CLI.Models;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Train_Reservation_System_CLI.Services
{
    class BookingManager
    {
        private TrainManager trainManager;

        public BookingManager(TrainManager trainManager)
        {
            this.trainManager = trainManager;
        }
        public int PNR = 10000000;

        public Dictionary<int, Ticket> Tickets = new();

        public void HandleBookingFlow(string from, string to, DateOnly date, CoachType coachType, int noOfSeats)
        {

            List<Train> trains = FetchMatchingTrain(from, to, date, coachType, noOfSeats);
            if (trains.Count == 0)
                throw new InvalidInputExecption(OutputHandler.ErrorNoTrainAvailable());
            if (noOfSeats <= 0 || noOfSeats > 24)
                throw new InvalidInputExecption(OutputHandler.ErrorInvalidSeatCount(noOfSeats));

            if (!(date >= DateOnly.FromDateTime(DateTime.Today)))
                throw new InvalidInputExecption(OutputHandler.ErrorInvalidDate(date));

            OutputHandler.ShowTrainsForBookingRequest(trains);
            OutputHandler.PrintMessage("Select Train Number");
            int selectedTrainNumber = InputHandler.GetInputForTrainNumber();

            Train selectedTrain = trains.FirstOrDefault(t => t.TrainNumber == selectedTrainNumber);
            if (selectedTrain == null)
                throw new InvalidInputExecption(OutputHandler.ErrorNoTrainAvailable());


            BookTicket(selectedTrain, from, to, date, coachType, noOfSeats);

        }
        public void GenerateBookingReport()
        {
            if (Tickets.Count == 0)
            {
                OutputHandler.PrintMessage("No bookings found.");
                return;
            }

            var sortedTickets = Tickets.Values.OrderBy(t => t.Date).ThenBy(t => t.PNR);
            // Updated header with Waiting Seats column
            string header = string.Format(
                "{0,-10} | {1,-10} | {2,-10} | {3,-12} | {4,-10} | {5,-40} | {6,-8} | {7,-6}",
                "PNR", "From", "To", "Date", "CoachType", "Confirmed Seats", "Waiting", "Fare"
            );
            string separator = new string('-', header.Length);

            Console.WriteLine("=========== BOOKING REPORT ===========");
            Console.WriteLine(header);
            Console.WriteLine(separator);

            foreach (var ticket in sortedTickets)
            {
                List<string> seatChunks = ticket.BookedSeats
                    .Select((s, i) => new { Seat = s, Index = i })
                    .GroupBy(x => x.Index / 6)
                    .Select(g => string.Join(", ", g.Select(x => x.Seat.SeatNumber))) // .SeatNumber assumed
                    .ToList();
                if (seatChunks.Count == 0)
                {
                    seatChunks.Add("-");
                }

                // First line
                string row = string.Format(
                    "{0,-10} | {1,-10} | {2,-10} | {3,-12} | {4,-10} | {5,-40} | {6,-8} | INR {7,-6:F2}",
                    ticket.PNR,
                    ticket.From,
                    ticket.To,
                    ticket.Date.ToString("yyyy-MM-dd"),
                    ticket.CoachType,
                    seatChunks[0],
                    ticket.WaitingSeats,
                    ticket.Fare
                );
                Console.WriteLine(row);

                // Continuation lines for remaining seat chunks
                for (int i = 1; i < seatChunks.Count; i++)
                {
                    string continuation = string.Format(
                        "{0,-10} | {1,-10} | {2,-10} | {3,-12} | {4,-10} | {5,-40} | {6,-6} | {7,-6}",
                        "", "", "", "", "", seatChunks[i], "", ""
                    );
                    Console.WriteLine(continuation);
                }
            }

        }

        public void BookTicket(Train train, string from, string to, DateOnly date, CoachType coachType, int noOfSeats)
        {
            var coaches = train.Coaches.Where(c => c.CoachType == coachType).ToList();
            var (bookedSeats, WaitingSeats) = BookSeatsAcrossCoachesWithWaiting(coaches, date, noOfSeats);

            double fare = CalculateFare(train.Route.GetDistance(from, to), coachType, noOfSeats);
            Ticket ticket = GenerateTicket(train.TrainNumber, from, to, date, coachType, WaitingSeats, bookedSeats, noOfSeats, fare);
            if (WaitingSeats > 0)
            {
                if (ticket.CoachType == CoachType.SL)
                {
                    train.Waitlist.SLWaitlist.Add(ticket);
                }
                if (ticket.CoachType == CoachType.A3)
                {
                    train.Waitlist.A3Waitlist.Add(ticket);
                }
                if (ticket.CoachType == CoachType.A2)
                {
                    train.Waitlist.A2Waitlist.Add(ticket);
                }
                if (ticket.CoachType == CoachType.A1)
                {
                    train.Waitlist.A1Waitlist.Add(ticket);
                }
            }
            OutputHandler.PrintMessage(OutputHandler.SuccessBooking(ticket.PNR, ticket.Fare, ticket.WaitingSeats, ticket.BookedSeats.Count()));
        }

        public void GetBookingDetailsByPNR(int pnr)
        {
            if (Tickets.ContainsKey(pnr))
            {
                Ticket ticket = Tickets[pnr];
                OutputHandler.PrintMessage(ticket.ToString());
            }
            else
            {
                throw new InvalidInputExecption(OutputHandler.ErrorInvalidPNR(pnr));
            }
        }

        private (List<Seat> bookedSeats, int WaitingSeats) BookSeatsAcrossCoachesWithWaiting(List<Coach> coaches, DateOnly date, int seatsToBook)
        {
            List<Seat> bookedSeats = new List<Seat>();
            List<Seat> unassignedSeats = coaches
                                        .Where(c => c.SeatsByDate.ContainsKey(date))
                                        .SelectMany(c => c.SeatsByDate[date])
                                        .Where(s => !s.IsBooked)
                                        .ToList();
            if (unassignedSeats.Any())
            {
                foreach (var seat in unassignedSeats)
                {
                    if (seatsToBook > 0)
                    {
                        seat.IsBooked = true;
                        bookedSeats.Add(seat);
                        seatsToBook--;
                    }
                }
            }
            foreach (var coach in coaches)
            {
                int available = coach.GetAvailableSeats(date);
                int bookedCount = coach.SeatsByDate[date].Count;
                int toBook = Math.Min(seatsToBook, available);

                for (int i = 1; i <= toBook; i++)
                {
                    Seat seat = new Seat();
                    seat.SeatNumber = $"{coach.CoachID}-{bookedCount + i}";
                    seat.IsBooked = true;
                    coach.SeatsByDate[date].Add(seat);

                    bookedSeats.Add(seat);
                }

                seatsToBook -= toBook;

                if (seatsToBook == 0)
                    break;
            }
            return (bookedSeats, seatsToBook);
        }

        public Ticket GenerateTicket(int trainNumber, string from, string to, DateOnly date, CoachType coachType, int seatsInWaiting, List<Seat> bookedSeats, int totalSeats, double fare)
        {
            int currentPNR = ++PNR;

            Ticket ticket = new Ticket
            (
               currentPNR,
                trainNumber,
                from,
                 to,
                date,
                coachType,
                seatsInWaiting,
                bookedSeats,
                totalSeats,
                fare
            );

            Tickets.Add(currentPNR, ticket);


            return ticket;
        }

        private static double CalculateFare(int distance, CoachType coachType, int noOfSeats)
        {
            return distance * (int)coachType * noOfSeats;
        }

        public List<Train> FetchMatchingTrain(string from, string to, DateOnly date, CoachType coachType, int noOfSeats)
        {
            List<Train> matchingTrains = new List<Train>();

            List<Train> availableTrainsByRoute = trainManager.GetTrainsByRoute(from, to);

            if (availableTrainsByRoute.Count != 0)
            {
                foreach (var train in availableTrainsByRoute)
                {
                    if (train.HasCoach(coachType) == null)
                        continue;

                    matchingTrains.Add(train);
                }
            }
            return matchingTrains;
        }

        public void TicketCancellation(string cancellationDetails)
        {
            CancalledTicketInfo cancalledTicketInfo = GetCancelledSeats(cancellationDetails);
            if (cancalledTicketInfo.ConfirmedCancelledSeats.Count == 0)
                OutputHandler.PrintMessage($"Your {cancalledTicketInfo.WaitingCancelledSeats} Waiting Seats Are Cancelled Successfully");
            else
                OutputHandler.PrintMessage($"Your {cancalledTicketInfo.ConfirmedCancelledSeats.Count} Confirmed Seats  & {cancalledTicketInfo.WaitingCancelledSeats} Waiting Seats Are Cancelled Successfully");
            AssignCancelledSeats(cancalledTicketInfo);
        }
        public void AssignCancelledSeats(CancalledTicketInfo cancalledTicketInfo)
        {
            Train train = trainManager.Trains.FirstOrDefault(t => t.TrainNumber == cancalledTicketInfo.TrainNumber);
            if (train == null) return;

            List<Ticket> waitlist = GetWaitlistByCoachType(train, cancalledTicketInfo.CoachType)
                ?.FindAll(w => w.CoachType == cancalledTicketInfo.CoachType && w.Date == cancalledTicketInfo.Date);

            if (waitlist == null) return;

            foreach (Ticket wait in waitlist)
            {
                while (wait.WaitingSeats > 0 && cancalledTicketInfo.ConfirmedCancelledSeats.Count > 0)
                {
                    wait.WaitingSeats--;
                    Seat seat = cancalledTicketInfo.ConfirmedCancelledSeats[cancalledTicketInfo.ConfirmedCancelledSeats.Count - 1];
                    seat.IsBooked = true;
                    wait.BookedSeats.Add(seat);
                    OutputHandler.PrintMessage($"Ticket with PNR {wait.PNR} has been assigned a seat - ({seat.SeatNumber}) from the waitlist.");
                    cancalledTicketInfo.ConfirmedCancelledSeats.RemoveAt(cancalledTicketInfo.ConfirmedCancelledSeats.Count - 1);
                }
            }
            return;
        }

        private List<Ticket> GetWaitlistByCoachType(Train train, CoachType coachType)
        {
            return coachType switch
            {
                CoachType.SL => train.Waitlist.SLWaitlist,
                CoachType.A3 => train.Waitlist.A3Waitlist,
                CoachType.A2 => train.Waitlist.A2Waitlist,
                CoachType.A1 => train.Waitlist.A1Waitlist,
            };
        }

        public CancalledTicketInfo GetCancelledSeats(string cancellationDetails)
        {
            List<Seat> cancelledSeats = new List<Seat>();
            string[] splitDetails = cancellationDetails.Split(' ');
            int PNR = int.Parse(splitDetails[0]);
            int noOfSeatsToCancel = int.Parse(splitDetails[1]);
            Ticket ticket = Tickets[PNR];
            if (ticket == null)
                throw new InvalidInputExecption(OutputHandler.ErrorInvalidPNR(PNR));

            if (ticket.TotalNoOfSeats < noOfSeatsToCancel)
            {
                throw new InvalidInputExecption(OutputHandler.ErrorInvalidSeatCount(noOfSeatsToCancel));
            }

            Train train = trainManager.Trains.FirstOrDefault(t => t.TrainNumber == ticket.TrainNumber);


            if (ticket.WaitingSeats >= noOfSeatsToCancel)
            {
                ticket.WaitingSeats -= noOfSeatsToCancel;
                if (ticket.WaitingSeats == 0)
                {
                    switch (ticket.CoachType)
                    {
                        case CoachType.SL:
                            train.Waitlist.SLWaitlist.Remove(ticket);
                            break;
                        case CoachType.A3:
                            train.Waitlist.A3Waitlist.Remove(ticket);
                            break;
                        case CoachType.A2:
                            train.Waitlist.A2Waitlist.Remove(ticket);
                            break;
                        case CoachType.A1:
                            train.Waitlist.A1Waitlist.Remove(ticket);
                            break;
                    }
                    ticket.TotalNoOfSeats -= noOfSeatsToCancel;
                    ticket.Fare = CalculateFare(train.Route.GetDistance(ticket.From, ticket.To), ticket.CoachType, ticket.TotalNoOfSeats);
                    return new CancalledTicketInfo(ticket.TrainNumber, ticket.From, ticket.To, ticket.CoachType, ticket.Date, cancelledSeats, noOfSeatsToCancel);


                }
            }

            int toCancelFromConfirmed = noOfSeatsToCancel - ticket.WaitingSeats;
            if (ticket.WaitingSeats != 0)
            {
                ticket.WaitingSeats = 0;
                if (ticket.WaitingSeats == 0)
                {
                    switch (ticket.CoachType)
                    {
                        case CoachType.SL:
                            train.Waitlist.SLWaitlist.Remove(ticket);
                            break;
                        case CoachType.A3:
                            train.Waitlist.A3Waitlist.Remove(ticket);
                            break;
                        case CoachType.A2:
                            train.Waitlist.A2Waitlist.Remove(ticket);
                            break;
                        case CoachType.A1:
                            train.Waitlist.A1Waitlist.Remove(ticket);
                            break;
                    }

                }
            }
            for (int i = 0; i < toCancelFromConfirmed; i++)
            {
                Seat seat = ticket.BookedSeats[ticket.BookedSeats.Count - 1];
                seat.IsBooked = false;
                ticket.BookedSeats.RemoveAt(ticket.BookedSeats.Count - 1);
                cancelledSeats.Add(seat);
            }
            ticket.TotalNoOfSeats -= noOfSeatsToCancel;
            ticket.Fare = CalculateFare(train.Route.GetDistance(ticket.From, ticket.To), ticket.CoachType, ticket.TotalNoOfSeats);

            return new CancalledTicketInfo(ticket.TrainNumber, ticket.From, ticket.To, ticket.CoachType, ticket.Date, cancelledSeats, noOfSeatsToCancel - toCancelFromConfirmed);

        }

    }
}
