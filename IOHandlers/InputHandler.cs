using static Train_Reservation_System_CLI.Utils.InputUtils;

namespace Train_Reservation_System_CLI.IOHandlers;

public static class InputHandler
{
    public static string ReadInput(string message)
    {
        OutputHandler.PrintBanner(message);
        string input = Console.ReadLine();
        return input;
    }

    public static int GetChoiceFromMenu()
    {
        Console.WriteLine(OutputHandler.Separator);
        Console.WriteLine("Enter 1 to Add Trains");
        Console.WriteLine("Enter 2 to Book Ticket");
        Console.WriteLine("Enter 3 to View All Trains");
        Console.WriteLine("Enter 4 to Get Booking Details By PNR Number");
        Console.WriteLine("Enter 5 to Generate Bookings Report");
        Console.WriteLine("Enter 6 for Booking Cancellation");
        Console.WriteLine("Enter 7 to Exit");
        Console.WriteLine(OutputHandler.Separator);

        return ParseInt(Console.ReadLine());
    }
}