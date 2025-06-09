using Train_Reservation_System_CLI.Execptions;
using Train_Reservation_System_CLI.IOHandlers;
using Train_Reservation_System_CLI.Models;
using Train_Reservation_System_CLI.Parsers;
using Train_Reservation_System_CLI.Validators;
using static Train_Reservation_System_CLI.Services.SeatAllocator;
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

    public void HandleBookingFlow()
    {
        var request = ReadAndValidateBookingRequest();
        var availableTrains = GetTrainsForRequest(request);
        OutputHandler.ShowTrainsForBookingRequest(availableTrains);

        var selectedTrain = GetSelectedTrain(availableTrains);
        ConfirmBooking(selectedTrain, request);
    }

    private BookingRequest ReadAndValidateBookingRequest()
    {
        try
        {
            var input = InputHandler.ReadInput("Enter Booking Details: (e.g., Ahmedabad Surat 2023-03-15 SL 3)");
            var parts = SplitInput(input);

            if (parts.Length != 5)
                throw new InvalidInputExecption("Please enter details in the correct format.");

            var request = BookingParser.ParseBookingDetails(parts);

            if (request.NoOfSeats <= 0 || request.NoOfSeats > 24)
                throw new InvalidInputExecption(OutputHandler.ErrorInvalidSeatCount(request.NoOfSeats));

            if (request.Date < DateOnly.FromDateTime(DateTime.Today))
                throw new InvalidInputExecption(OutputHandler.ErrorInvalidDate(request.Date));

            return request;
        }
        catch (Exception e)
        {
            OutputHandler.PrintError(e.Message);
            return ReadAndValidateBookingRequest();
        }
    }

    private List<Train> GetTrainsForRequest(BookingRequest request)
    {
        var trainsByRoute = trainManager.GetTrainsByRoute(request.From, request.To);
        BookingValidator.ValidateTrainsByRoute(trainsByRoute, request);

        var filteredTrains = trainManager.FilterTrainsByCoachType(trainsByRoute, request.CoachType);
        BookingValidator.ValidateTrainsByCoachType(filteredTrains, request);

        return filteredTrains;
    }

    private Train GetSelectedTrain(List<Train> trains)
    {
        try
        {
            var input = InputHandler.ReadInput("Enter Train Number For Booking :");
            var trainNo = ParseInt(input);
            BookingValidator.ValidateTrainSelection(trains, trainNo);
            return trains.First(t => t.TrainNumber == trainNo);
        }
        catch (Exception e)
        {
            OutputHandler.PrintError(e.Message);
            return GetSelectedTrain(trains);
        }
    }

    private void ConfirmBooking(Train train, BookingRequest request)
    {
        var result = AllocateSeats(train, request);

        var fare = FareCalculator.CalculateFare(
            train.Route.GetDistance(request.From, request.To),
            request.CoachType,
            request.NoOfSeats
        );

        var ticket = ticketManager.GenerateTicket(request, train.TrainNumber,
            result.WaitingSeats, result.BookedSeats, fare);

        if (result.WaitingSeats > 0)
            ticketManager.AddTicketInTrainWaitingList(ticket, train);

        ticketManager.DisplayTicket(ticket);
    }
}