using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Train_Reservation_System_CLI.Execptions;

namespace Train_Reservation_System_CLI.Services.IOHandlers
{
    public static class InputHandler
    {

        public static int ReadInt(string message, int? max = null, int? min = null)
        {
            OutputHandler.PrintBanner(message);

            if (!int.TryParse(Console.ReadLine(), out int value))
            {

                throw new InvalidInputExecption("Invalid number format");
            }

            if (min.HasValue && value < min || max.HasValue && value > max)
            {
                throw new InvalidInputExecption($"Value must be between {min} and {max}");
            }

            return value;
        }

        public static string[] ReadString(string message, int? max = null,int? min =null)
        {
            OutputHandler.PrintBanner(message);
            var value = Console.ReadLine().Split(" ");
            if (value.Length < min || value.Length>max)
            {
                throw new InvalidInputExecption("Please Enter Detail As Given In Example");
            }
            return value;
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

            return int.Parse(Console.ReadLine());
        }

    }
}
