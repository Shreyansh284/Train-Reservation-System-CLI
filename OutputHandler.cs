using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Train_Reservation_System_CLI
{
    public static class OutputHandler
    {

        public static void PrintMessage(string message)
        {
            
            Console.WriteLine($"{Seprater} {message} \n{Seprater}");
        }
        public static void ShowTrainsForBookingRequest(List<Train> trains)
        {
            Console.WriteLine($"{Seprater} Following Are Available Trains \n ");
            foreach (var train in trains)
            {

                Console.Write(train.TrainNumber + " ");
            }
            Console.WriteLine($"\n{Seprater}");    
        }
        
        public static string ErrorInvalidPNR(int pnr)
        {
            return $"{Seprater} Invalid PNR: {pnr}. Please check and try again. \n {Seprater}";
        }

        public static void PrintAllTrains(List<Train> trains)
        {
            foreach (var train in trains)
            {
                Console.WriteLine($"Train Number: {train.TrainNumber}");
                Console.WriteLine($"Route: {train.Route.Source} to {train.Route.Destination}");
                Console.WriteLine("Coaches:");
                foreach (var coach in train.Coaches)
                {
                    Console.WriteLine($"  Coach ID: {coach.CoachID}, Type: {coach.CoachType}, Total Seats: {coach.TotalSeats}");
                }
                Console.WriteLine("Routes:");
                foreach (var route in train.Route.RouteAndDistanceMap)
                {
                    Console.WriteLine($"  Route: {route.Key}, Distance: {route.Value}");
                }
                Console.WriteLine();
            }
        }

        public static string Seprater
            => new string('=', 50)+"\n";

        public static string ErrorInvalidSeatCount(int seatCount)
                => $"{Seprater} Select Seats Between 1 to 24. You selected: {seatCount} \n{Seprater}";
        public static string ErrorInvalidDate(DateOnly date)
                => $"{Seprater} Invalid Date: {date}. Please select a date that is today or in the future. \n{Seprater}";

        public static string ErrorCoachUnavailable(CoachType coachType)
                => $" {Seprater} No {coachType} coach available on any train for the selected route. \n{Seprater}";

        public static string ErrorInsufficientSeats(CoachType coachType, int available)
                => $"{Seprater} Only {available} seats available in {coachType}. \n{Seprater}";

        public static string SuccessBooking(int pnr, double fare,int waitingSeats,int confirmedSeats)
        {
            return string.Format(
                "Booking Confirmed!\n" +
                "-------------------\n" +
                "PNR   : {0}\n" +
                "Waiting Seats   : {1}\n" +
                "Confirmed Seats  : {2}\n"+
                "Fare  : INR {3:F2}\n",
                pnr,waitingSeats,confirmedSeats, fare
            );
        }
        public static string ErrorNoTrainAvailable()
            => $"{Seprater} No Train Available for the given route. \n {Seprater}";
    }
}
