using Train_Reservation_System_CLI.Execptions;
using Train_Reservation_System_CLI.IOHandlers;
using Train_Reservation_System_CLI.Models;
using Train_Reservation_System_CLI.Parsers;
using Train_Reservation_System_CLI.Validators;
using static Train_Reservation_System_CLI.Utils.InputUtils;


namespace Train_Reservation_System_CLI.Services;

internal class BookingManager
{
    private readonly TicketManager ticketManager;
    private readonly TrainManager trainManager;

    public BookingManager(TrainManager trainManager, TicketManager ticketManager)
    {
        this.trainManager = trainManager;
        this.ticketManager = ticketManager;
    }

    public void GetBookingDetails()
    {
        var input = InputHandler.ReadInput("Enter Booking Details : ( e.g.,Ahmedabad Surat 2023-03-15 SL 3)");
        var bookingDetails = SplitInput(input);
        if (InputValidator.IsOutOfRange(5, 5, bookingDetails.Length))
            throw new InvalidInputExecption("Please Enter Details As Shown In Above Example");
        var bookingRequest = BookingParser.ParseBookingDetails(bookingDetails);

        HandleBookingFlow(bookingRequest);
    }

    public void HandleBookingFlow(BookingRequest bookingRequest)
    {
        if (bookingRequest.NoOfSeats <= 0 || bookingRequest.NoOfSeats > 24)
            throw new InvalidInputExecption(OutputHandler.ErrorInvalidSeatCount(bookingRequest.NoOfSeats));
        if (!(bookingRequest.Date >= DateOnly.FromDateTime(DateTime.Today)))
            throw new InvalidInputExecption(OutputHandler.ErrorInvalidDate(bookingRequest.Date));

        var trains = FetchMatchingTrain(bookingRequest);
        if (trains.Count == 0)
            throw new InvalidInputExecption(OutputHandler.ErrorNoTrainAvailable());

        OutputHandler.ShowTrainsForBookingRequest(trains);

        var input = InputHandler.ReadInput("Select/Enter Train Number To Proceed Booking");
        var selectedTrainNumber = ParseInt(input);
        var selectedTrain = trains.FirstOrDefault(t => t.TrainNumber == selectedTrainNumber);

        if (selectedTrain == null) throw new InvalidInputExecption(OutputHandler.ErrorNoTrainAvailable());

        BookTicket(selectedTrain, bookingRequest);
    }

    public void BookTicket(Train train, BookingRequest bookingRequest)
    {
        var coaches = train.Coaches.Where(c => c.CoachType == bookingRequest.CoachType).ToList();
        var seatAllocationResult =
            BookSeatsAcrossCoachesWithWaiting(coaches, bookingRequest.Date, bookingRequest.NoOfSeats);

        var fare = FareCalculator.CalculateFare(train.Route.GetDistance(bookingRequest.From, bookingRequest.To),
            bookingRequest.CoachType, bookingRequest.NoOfSeats);
        var ticket = ticketManager.GenerateTicket(bookingRequest, train.TrainNumber, seatAllocationResult.WaitingSeats,
            seatAllocationResult.BookedSeats, fare);

        if (seatAllocationResult.WaitingSeats > 0) ticketManager.AddTicketInWaitingList(ticket, train);

        ticketManager.DisplayTicket(ticket);
    }

    private SeatAllocationResult BookSeatsAcrossCoachesWithWaiting(List<Coach> coaches, DateOnly date, int seatsToBook)
    {
        SeatAllocationResult seatAllocationResult = new();

        var unassignedSeats = coaches
            .Where(c => c.SeatsByDate.ContainsKey(date))
            .SelectMany(c => c.SeatsByDate[date])
            .Where(s => !s.IsBooked)
            .ToList();
        if (unassignedSeats.Any())
            foreach (var seat in unassignedSeats)
                if (seatsToBook > 0)
                {
                    seat.IsBooked = true;
                    seatAllocationResult.BookedSeats.Add(seat);
                    seatsToBook--;
                }

        foreach (var coach in coaches)
        {
            var available = coach.GetAvailableSeats(date);
            var currentSeatCount = coach.SeatsByDate[date].Count;
            var toBook = Math.Min(seatsToBook, available);

            for (var i = 1; i <= toBook; i++)
            {
                var seat = new Seat();
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
        var matchingTrains = new List<Train>();

        var availableTrainsByRoute = trainManager.GetTrainsByRoute(bookingRequest.From, bookingRequest.To);

        if (availableTrainsByRoute.Count != 0)
        {
            var trainsByCoachType = trainManager.GetTrainsByCoachType(availableTrainsByRoute, bookingRequest.CoachType);
            matchingTrains.AddRange(trainsByCoachType);
        }

        return matchingTrains;
    }
}