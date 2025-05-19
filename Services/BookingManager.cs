using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Train_Reservation_System_CLI.Models;
using Train_Reservation_System_CLI.Services.IOHandlers;
using Train_Reservation_System_CLI.Services.Parsers;
using Train_Reservation_System_CLI.Execptions;


namespace Train_Reservation_System_CLI.Services
{
    class BookingManager
    {
        private TrainManager trainManager;
        private TicketManager ticketManager;

        public BookingManager(TrainManager trainManager, TicketManager ticketManager)
        {
            this.trainManager = trainManager;
            this.ticketManager = ticketManager;
        }

        public void GetBookingDetails()
        {
            string[] bookingDetails = InputHandler.ReadString("Enter Booking Details : ( e.g.,Ahmedabad Surat 2023-03-15 SL 3)",5,5);

            BookingRequest bookingRequest = BookingParser.ParseBookingDetails(bookingDetails);

            HandleBookingFlow(bookingRequest);
        }

        public void HandleBookingFlow(BookingRequest bookingRequest)
        {
            if (bookingRequest.NoOfSeats <= 0 || bookingRequest.NoOfSeats > 24)
                throw new InvalidInputExecption(OutputHandler.ErrorInvalidSeatCount(bookingRequest.NoOfSeats));
            if (!(bookingRequest.Date >= DateOnly.FromDateTime(DateTime.Today)))
                throw new InvalidInputExecption(OutputHandler.ErrorInvalidDate(bookingRequest.Date));

            List<Train> trains = FetchMatchingTrain(bookingRequest);
            if (trains.Count == 0)
                throw new InvalidInputExecption(OutputHandler.ErrorNoTrainAvailable());

            OutputHandler.ShowTrainsForBookingRequest(trains);

            int selectedTrainNumber = InputHandler.ReadInt("Select/Enter Train Number To Proceed Booking");

            Train selectedTrain = trains.FirstOrDefault(t => t.TrainNumber == selectedTrainNumber);

            if (selectedTrain == null) throw new InvalidInputExecption(OutputHandler.ErrorNoTrainAvailable());

            BookTicket(selectedTrain, bookingRequest);

        }

        public void BookTicket(Train train, BookingRequest bookingRequest)
        {
            List<Coach> coaches = train.Coaches.Where(c => c.CoachType == bookingRequest.CoachType).ToList();
            SeatAllocationResult seatAllocationResult = BookSeatsAcrossCoachesWithWaiting(coaches, bookingRequest.Date, bookingRequest.NoOfSeats);

            double fare = FareCalculator.CalculateFare(train.Route.GetDistance(bookingRequest.From, bookingRequest.To), bookingRequest.CoachType, bookingRequest.NoOfSeats);
            Ticket ticket = ticketManager.GenerateTicket(bookingRequest, train.TrainNumber, seatAllocationResult.WaitingSeats, seatAllocationResult.BookedSeats, fare);

            if (seatAllocationResult.WaitingSeats > 0) ticketManager.AddTicketInWaitingList(ticket, train);

            ticketManager.DisplayTicket(ticket);
        }

        private SeatAllocationResult BookSeatsAcrossCoachesWithWaiting(List<Coach> coaches, DateOnly date, int seatsToBook)
        {
            SeatAllocationResult seatAllocationResult = new();

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
                        seatAllocationResult.BookedSeats.Add(seat);
                        seatsToBook--;
                    }
                }
            }
            foreach (var coach in coaches)
            {
                int available = coach.GetAvailableSeats(date);
                int currentSeatCount = coach.SeatsByDate[date].Count;
                int toBook = Math.Min(seatsToBook, available);

                for (int i = 1; i <= toBook; i++)
                {
                    Seat seat = new Seat();
                    seat.SeatNumber = $"{coach.CoachID}-{currentSeatCount + i}";
                    seat.IsBooked = true;
                    coach.SeatsByDate[date].Add(seat);

                    seatAllocationResult.BookedSeats.Add(seat);
                }

                seatsToBook -= toBook;

                if (seatsToBook == 0)
                    break;
            }
            seatAllocationResult.WaitingSeats = seatsToBook;
            return seatAllocationResult;
        }

        public List<Train> FetchMatchingTrain(BookingRequest bookingRequest)
        {
            List<Train> matchingTrains = new List<Train>();

            List<Train> availableTrainsByRoute = trainManager.GetTrainsByRoute(bookingRequest.From, bookingRequest.To);

            if (availableTrainsByRoute.Count != 0)
            {
                var trainsByCoachType = trainManager.GetTrainsByCoachType(availableTrainsByRoute, bookingRequest.CoachType);
                matchingTrains.AddRange(trainsByCoachType);
            }

            return matchingTrains;
        }
    }
}
