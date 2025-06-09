using Train_Reservation_System_CLI.Execptions;
using Train_Reservation_System_CLI.Models;

namespace Train_Reservation_System_CLI.Validators;

public static class BookingValidator
{
    public static void ValidateTrainsByRoute(List<Train> trains, BookingRequest request)
    {
        if (trains == null || trains.Count == 0)
        {
            throw new InvalidOperationException($"No trains found from {request.From} to {request.To}.");
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
        if (!availableTrains.Any(train => train.TrainNumber == inputTrainNumber))
        {
            throw new InvalidInputExecption(
                $"Train selection {inputTrainNumber} is invalid. Enter Any Above Given Number");
        }
    }

    public static void ValidatePNR(List<Ticket> tickets, int pnr)
    {
        bool isValid = tickets.Any(t => t.PNR == pnr);
        if (!isValid)
        {
            throw new InvalidInputExecption($"Invalid PNR Number ({pnr}) For Getting Booking Details");
        }
    }
}