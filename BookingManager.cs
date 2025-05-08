using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Train_Reservation_System_CLI
{
    class BookingManager
    {
        private TrainManager trainManager;

        public BookingManager(TrainManager trainManager)
        {
            this.trainManager = trainManager;
        }
        public int PNR = 10000000;

        public Dictionary<int,Ticket> Tickets=new();

        public void HandleBookingFlow(string from, string to, DateOnly date, CoachType coachType, int noOfSeats)
        {
            if (noOfSeats <= 0 || noOfSeats > 24)
                throw new InvalidInputExecption(OutputHandler.ErrorInvalidSeatCount(noOfSeats));

            if (!(date >= DateOnly.FromDateTime(DateTime.Today)))
                throw new InvalidInputExecption(OutputHandler.ErrorInvalidDate(date));

            List<Train> trains = FetchMatchingTrain(from,to,date,coachType,noOfSeats);
            if (trains.Count == 0)
                throw new InvalidInputExecption(OutputHandler.ErrorNoTrainAvailable());

            OutputHandler.ShowTrainsForBookingRequest(trains);
            OutputHandler.PrintMessage("Select Train Number");
            int selectedTrainNumber= InputHandler.GetInputForTrainNumber();

            Train selectedTrain = trains.FirstOrDefault(t => t.TrainNumber == selectedTrainNumber);
            if (selectedTrain == null)
                throw new InvalidInputExecption(OutputHandler.ErrorNoTrainAvailable());


            BookTicket(selectedTrain,from,to,date,coachType,noOfSeats);


        }
        public void GenerateBookingReport()
        {
            if (Tickets.Count == 0)
            {
                OutputHandler.PrintMessage("No bookings found.");
                return;
            }

            var sortedTickets = Tickets.Values.OrderBy(t => t.Date).ThenBy(t => t.PNR);

            // Header without Seats column
            string header = string.Format(
                "{0,-10} | {1,-10} | {2,-10} | {3,-12} | {4,-10} | {5,-40} | {6,-8}",
                "PNR", "From", "To", "Date", "CoachType", "SeatNumbers", "Fare"
            );
            string separator = new string('-', header.Length);

            Console.WriteLine("=========== BOOKING REPORT ===========");
            Console.WriteLine(header);
            Console.WriteLine(separator);


            foreach (var ticket in sortedTickets)
            {
                List<string> seatChunks = ticket.SeatNumbers
                    .Select((s, i) => new { Seat = s, Index = i })
                    .GroupBy(x => x.Index / 6)
                    .Select(g => string.Join(", ", g.Select(x => x.Seat)))
                    .ToList();

                // First line: full data
                string row = string.Format(
                    "{0,-10} | {1,-10} | {2,-10} | {3,-12} | {4,-10} | {5,-40} | INR {6,-6:F2}",
                    ticket.PNR,
                    ticket.From,
                    ticket.To,
                    ticket.Date.ToString("yyyy-MM-dd"),
                    ticket.CoachType,
                    seatChunks[0],
                    ticket.Fare
                );
                Console.WriteLine(row);

                // Remaining seat chunks: only seat numbers column
                for (int i = 1; i < seatChunks.Count; i++)
                {
                    string continuation = string.Format(
                        "{0,-10} | {1,-10} | {2,-10} | {3,-12} | {4,-10} | {5,-40} | {6,-8}",
                        "", "", "", "", "", seatChunks[i], ""
                    );
                    Console.WriteLine(continuation);
                }
            }

        }

        public void BookTicket(Train train, string from, string to, DateOnly date, CoachType coachType, int noOfSeats)
        {
            var coaches = train.Coaches.Where(c => c.CoachType == coachType).ToList();
            List<string> bookedSeats= BookSeatsAcrossCoaches(coaches, date, noOfSeats);

            double fare = CalculateFare(train.Route.GetDistance(from, to), coachType, noOfSeats);
            Ticket ticket = GenerateTicket(train.TrainNumber, from, to, date, coachType, bookedSeats, fare);
            OutputHandler.PrintMessage(OutputHandler.SuccessBooking(ticket.PNR, ticket.Fare));
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


        private List<string> BookSeatsAcrossCoaches(List<Coach> coaches, DateOnly date, int seatsToBook)
        {
            List<string> bookedSeats = new List<string>();
            foreach (var coach in coaches)
            {
              
                int bookedCount = coach.SeatsByDate[date].Count;
                int available = coach.TotalSeats - bookedCount;
                int toBook = Math.Min(seatsToBook, available);

                for (int i = 1; i <= toBook; i++)
                {
                    coach.SeatsByDate[date].Add(new Seat
                    {
                        SeatNumber = $"{coach.CoachID}-{bookedCount + i}",
                        IsBooked = true
                    });

                    bookedSeats.Add($"{coach.CoachID}-{bookedCount + i}");
                }

                seatsToBook -= toBook;

                if (seatsToBook == 0)
                    break;
            }
            return bookedSeats;
        }

        public Ticket GenerateTicket( int trainNumber ,string from, string to, DateOnly date, CoachType coachType, List<string> bookedSeats, double fare)
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
                bookedSeats,
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

            if(availableTrainsByRoute.Count!=0)
            {
                foreach (var train in availableTrainsByRoute)
                {
                    if (train.HasCoach(coachType) == null)
                        continue;
                    var coaches = train.Coaches.Where(c => c.CoachType == coachType).ToList();
                    int totalAvailableSeats = coaches.Sum(c => c.GetAvailableSeats(date));
                    if (totalAvailableSeats >= noOfSeats)
                        matchingTrains.Add(train);
                }
            }
            return matchingTrains;

        }
    }
}
