using Train_Reservation_System_CLI.Models;

namespace Train_Reservation_System_CLI.IOHandlers;

public static class OutputHandler
{
    public static string Separator => new('=', 50);

    public static void PrintWrapped(string message)
    {
        Console.WriteLine(Separator);
        Console.WriteLine(message);
        Console.WriteLine(Separator);
    }

    public static void PrintBanner(string heading)
    {
        Console.ForegroundColor = ConsoleColor.DarkYellow;
        Console.WriteLine();
        Console.WriteLine(Separator);
        Console.WriteLine(heading);
        Console.WriteLine(Separator);
        Console.ResetColor();
    }

    public static void PrintMessage(string message)
    {
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine(message);
        Console.ResetColor();
    }

    public static void PrintError(string message)
    {
        Console.ForegroundColor = ConsoleColor.Red;
        PrintWrapped("ERROR: " + message);
        Console.ResetColor();
    }

    public static void ShowTrainsForBookingRequest(List<Train> trains)
    {
        PrintBanner("Available Trains");
        if (trains.Count == 0)
        {
            PrintError("No trains found.");
            return;
        }

        Console.WriteLine(string.Join(" ", trains.Select(t => t.TrainNumber)));
    }

    public static void PrintAllTrains(List<Train> trains)
    {
        if (trains == null || trains.Count == 0)
        {
            PrintError("No trains available.");
            return;
        }

        PrintBanner("Train List");
        foreach (var train in trains) Console.WriteLine(train);
    }

    // Error Message Helpers
    public static string ErrorInvalidPNR(int pnr)
    {
        return $"Invalid PNR: {pnr}. Please check and try again.";
    }

    public static string ErrorInvalidSeatCount(int seatCount)
    {
        return $"Select Seats Between 1 to 24. You selected: {seatCount}";
    }

    public static string ErrorInvalidDate(DateOnly date)
    {
        return $"Invalid Date: {date}. Please select a date that is today or in the future.";
    }

    public static string ErrorCoachUnavailable(CoachType coachType)
    {
        return $"No {coachType} coach available on any train for the selected route.";
    }

    public static string ErrorInsufficientSeats(CoachType coachType, int available)
    {
        return $"Only {available} seats available in {coachType}.";
    }

    public static string ErrorNoTrainAvailable()
    {
        return "No Train Available for the given route.";
    }
    public static void DisplayCancellationMessage(CancellationRecord record)
    {
        string message = record.ConfirmedCancelledSeats.Count == 0
            ? $"Your {record.WaitingCancelledSeats} Waiting Seats Are Cancelled Successfully"
            : $"Your {record.ConfirmedCancelledSeats.Count} Confirmed Seats & {record.WaitingCancelledSeats} Waiting Seats Are Cancelled Successfully";

        OutputHandler.PrintMessage(message);
    }
}