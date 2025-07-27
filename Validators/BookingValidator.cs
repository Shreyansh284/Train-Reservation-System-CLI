using Train_Reservation_System_CLI.Exceptions;
using Train_Reservation_System_CLI.IOHandlers;
using Train_Reservation_System_CLI.Models;


namespace Train_Reservation_System_CLI.Validators;

public static class BookingValidator
{
    private const int MaxSeatBook = 24;
    private const int MinSeatBook = 1;
    private static readonly DateOnly CurrentDate = DateOnly.FromDateTime(DateTime.Today);
    public static void ValidateTrainsByRoute(List<Train> trains, BookingRequest request)
    {
        if (trains == null || trains.Count == 0)
        {
            throw new InvalidOperationException($"No trains found from {request.From} to {request.To}.");
        }
    }

    public static void ValidateBookingInput(string[] bookingInput)
    {
        if (bookingInput.Length != 5)
        {
            throw new InvalidInputException(
                "Please Enter ( Source Destination Date CoachType And NumberOfSeat ) As Given Above");
        }

        int seats = Utils.InputUtils.ParseInt(bookingInput[4]);
        if (seats <= MinSeatBook || seats > MaxSeatBook)
        {
            throw new InvalidInputException(OutputHandler.ErrorInvalidSeatCount(seats));
        }

        DateOnly date = DateOnly.Parse(bookingInput[2]);
        if (date < CurrentDate)
        {
            throw new InvalidInputException(OutputHandler.ErrorInvalidDate(date));
        }
    }

    public static void ValidateTrainsByCoachType(List<Train> trains, BookingRequest request)
    {
        if (trains == null || trains.Count == 0)
        {
            throw new InvalidOperationException(
                $"No trains with coach type '{request.CoachType}' found between {request.From} and {request.To}.");
        }
    }

    public static void ValidateTrainSelection(List<Train> availableTrains, int inputTrainNumber)
    {
        if (availableTrains.All(train => train.TrainNumber != inputTrainNumber))
        {
            throw new InvalidInputException(
                $"Train selection {inputTrainNumber} is invalid. Enter Any Above Given Number");
        }
    }

    public static void ValidatePNR(List<Ticket> tickets, int pnr)
    {
        bool isValid = tickets.Any(t => t.PNR == pnr);
        if (!isValid)
        {
            throw new InvalidInputException($"Invalid PNR Number ({pnr}) For Getting Booking Details");
        }
    }
}