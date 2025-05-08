using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Train_Reservation_System_CLI
{
    public static class InputHandler
    {
        public static int GetChoiceFromMenu()
        {

            Console.WriteLine(OutputHandler.Seprater);
            Console.WriteLine("Enter 1 to Add Trains");
            Console.WriteLine("Enter 2 to Book Ticket");
            Console.WriteLine("Enter 3 to View All Trains");
            Console.WriteLine("Enter 4 to Get Booking Details By PNR Number");
            Console.WriteLine("Enter 5 to Generate Bookings Report");
            Console.WriteLine("Enter 6 to Exit");
            Console.WriteLine(OutputHandler.Seprater);

            return int.Parse(Console.ReadLine());
        }
        
        public static int GetInputForPNRNumber()
        {
            Console.WriteLine(OutputHandler.Seprater);
            Console.WriteLine("Enter PNR Number: ");
            var pnrNumber = Int32.Parse(Console.ReadLine());
            return pnrNumber;
        }
        public static int GetInputForTrainNumber()
        {
            Console.WriteLine(OutputHandler.Seprater);
            Console.WriteLine("Enter Train Number: ");
            var trainNumber = Int32.Parse(Console.ReadLine());

            return trainNumber;
        }

        public static string[] GetBookingDetails()
        {
            Console.WriteLine(OutputHandler.Seprater);
            Console.WriteLine("Enter Booking Details : ( e.g.,Ahmedabad Surat 2023-03-15 SL 3)");
            var trainBooking = Console.ReadLine().Split(" ");
            if (trainBooking.Length<5)
            {
                throw new InvalidInputExecption("Please Enter Detail As Given In Example");
            }
            return trainBooking;
        }

        public static int GetNumberOfTrainsToAdd()
        {
            Console.WriteLine(OutputHandler.Seprater);
            Console.WriteLine("Enter Number of Trains to be Added: ");

            int numberOfTrains = int.Parse(Console.ReadLine()); ;
            if (numberOfTrains > 50 || numberOfTrains < 1)
            {
                throw new InvalidInputExecption("Trains Must Be Between 1 To 50");
            }
            return numberOfTrains;
        }

        public static string[] GetInputForTrainRoute()
        {
            Console.WriteLine(OutputHandler.Seprater);
            Console.WriteLine("Enter Train Route: (e.g., 29772 Vadodara-0 Dahod-150 Indore-350 ) ");
            var trainRoute = Console.ReadLine().Split(" ");
            if (trainRoute.Length < 3) 
            {
                throw new InvalidInputExecption("Please Enter Detail As Given In Example");
            }
            return trainRoute;
        }

        public static string[] GetInputForTrainCoaches()
        {
            Console.WriteLine(OutputHandler.Seprater);
            Console.WriteLine("Enter Train Coaches: ( e.g., 29772 S1-72 S2-72 B1-72 A1-48) ");
            var trainCoach = Console.ReadLine().Split(" ");
            if (trainCoach.Length < 2)
            {
                throw new InvalidInputExecption("Please Enter Detail As Given In Example");
            }
            return trainCoach;
        }

    }
}
