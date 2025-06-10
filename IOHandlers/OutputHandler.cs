using Train_Reservation_System_CLI.Models;

namespace Train_Reservation_System_CLI.IOHandlers;

public static class OutputHandler
{
    public static string Separator => new('=', 50);

    private static void PrintWrapped(string message)
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

        PrintMessage(string.Join(" ", trains.Select(t => t.TrainNumber)));
    }

    public static void PrintAllTrains(List<Train> trains)
    {
        if (trains == null || trains.Count == 0)
        {
            PrintError("No trains available.");
            return;
        }

        PrintBanner("Train List");
        foreach (var train in trains) PrintMessage(train.ToString());
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

    public static void DisplayCancellationMessage(CancellationRecord record)
    {
        string message = record.ConfirmedCancelledSeats.Count == 0
            ? $"Your {record.WaitingCancelledSeats} Waiting Seats Are Cancelled Successfully"
            : $"Your {record.ConfirmedCancelledSeats.Count} Confirmed Seats & {record.WaitingCancelledSeats} Waiting Seats Are Cancelled Successfully";
        PrintMessage(message);
    }

    public static void DisplayReport(List<Ticket> tickets)
    {
        if (tickets.Count == 0)
        {
            OutputHandler.PrintError("No bookings found.");
            return;
        }

        var sortedTickets = tickets.OrderBy(t => t.Date).ThenBy(t => t.PNR);

        const int chunkSize = 6;

        var header =
            $"{"PNR",-10} | {"TrainNo",-8} | {"From",-10} | {"To",-10} | {"Date",-12} | {"CoachType",-10} | {"Confirmed Seats",-40} | {"Waiting",-8} | {"Fare",-6}";
        var separator = new string('-', header.Length);

        Console.WriteLine("=========== BOOKING REPORT ===========");
        Console.WriteLine(header);
        Console.WriteLine(separator);

        foreach (var ticket in sortedTickets)
        {
            var seatChunks = FormatSeatChunks(ticket.BookedSeats, chunkSize);

            Console.WriteLine(
                $"{ticket.PNR,-10} | {ticket.TrainNumber,-8} | {ticket.From,-10} | {ticket.To,-10} | {ticket.Date.ToString("yyyy-MM-dd"),-12} | {ticket.CoachType,-10} | {seatChunks[0],-40} | {ticket.WaitingSeats,-8} | INR {ticket.Fare,-6:F2}"
            );

            for (var i = 1; i < seatChunks.Count; i++)
                Console.WriteLine(
                    $"{string.Empty,-10} | {string.Empty,-8} | {string.Empty,-10} | {string.Empty,-10} | {string.Empty,-12} | {string.Empty,-10} | {seatChunks[i],-40} | {string.Empty,-8} | {string.Empty,-6}"
                );
        }
    }
    private static List<string> FormatSeatChunks(List<Seat> seats, int chunkSize)
    {
        if (seats == null || seats.Count == 0)
            return ["-"];

        return seats
            .Select((seat, index) => new { seat.SeatNumber, index })
            .GroupBy(x => x.index / chunkSize)
            .Select(group => string.Join(", ", group.Select(x => x.SeatNumber)))
            .ToList();
    }
}