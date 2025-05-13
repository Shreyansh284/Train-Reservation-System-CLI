using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Train_Reservation_System_CLI
{
    class TrainConfiguration
    {
        TrainManager trainManager;
        BookingManager bookingManager;

        public TrainConfiguration()
        {
            this.trainManager = new TrainManager();
            this.bookingManager = new BookingManager(trainManager);
        }

        public void RunTrainOperationsMenu()
        {

            bool exit = false;

            while (!exit)
            {
                try
                {
                    int choice = InputHandler.GetChoiceFromMenu();
                    HandleMenuChoice(choice, ref exit);
                }
                catch (InvalidInputExecption ex)
                {
                    Console.WriteLine(OutputHandler.Seprater);
                    Console.WriteLine(ex.Message);
                    Console.WriteLine(OutputHandler.Seprater);
                    Console.WriteLine("Enter Details Again :");
                    Console.WriteLine(OutputHandler.Seprater);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(OutputHandler.Seprater);
                    Console.WriteLine(ex.Message);
                    Console.WriteLine(OutputHandler.Seprater);
                    Console.WriteLine("Enter Details Again :");
                    Console.WriteLine(OutputHandler.Seprater);
                }
            }

        }

        private void HandleMenuChoice(int choice, ref bool exit)
        {
            switch (choice)
            {
                case 1:
                    trainManager.AddTrain();
                    break;
                case 2:
                    BookingRequest();
                    break;
                case 3:
                    trainManager.GetAllTrains();
                    break;
                case 4:
                    bookingManager.GetBookingDetailsByPNR(InputHandler.GetInputForPNRNumber());
                    break;
                case 5:
                    bookingManager.GenerateBookingReport();
                    break;
                case 6:
                    bookingManager.TicketCancellation(InputHandler.GetInputForCancellation());
                    break;
                case 7:
                    exit = true;
                    break;
                default:
                    OutputHandler.PrintMessage("Invalid Choice");
                    break;
            }
        }

        private void BookingRequest()
        {
            var bookingDetails = InputHandler.GetBookingDetails();
            string from = bookingDetails[0];
            string to = bookingDetails[1];
            DateOnly date = DateOnly.Parse(bookingDetails[2]);
            CoachType coachType = (CoachType)Enum.Parse(typeof(CoachType), bookingDetails[3]);
            int seats = int.Parse(bookingDetails[4]);

            bookingManager.HandleBookingFlow(from, to, date, coachType, seats);

        }

        public static Train TrainDetails()
        {
            var (trainNumber, route) = ParseTrainRouteAndTrainNumber();
            List<Coach> coaches = ParseTrainCoaches(trainNumber);
            Train train = new Train(trainNumber, route, coaches);

            return train;
        }

        public static (int trainNumber, Route route) ParseTrainRouteAndTrainNumber()
        {

            var splitedInput = InputHandler.GetInputForTrainRoute();

            int trainNumber = int.Parse(splitedInput[0]);
            string source = splitedInput[1].Split("-")[0];
            string destination = splitedInput[splitedInput.Length - 1].Split("-")[0];

            var route = new Route(source, destination);

            for (int i = 1; i < splitedInput.Length; i++)
            {
                string station = splitedInput[i].Split("-")[0];
                int stationDistance = int.Parse(splitedInput[i].Split("-")[1]);

                route.RouteAndDistanceMap.Add($"{station}", stationDistance);
            }

            return (trainNumber, route);

        }

        public static List<Coach> ParseTrainCoaches(int trainNumber)
        {

            var splitedInput = InputHandler.GetInputForTrainCoaches();
            if (trainNumber != int.Parse(splitedInput[0]))
            {
                throw new InvalidInputExecption("Train Number and Coach Number Must Be Same");
            }

            List<Coach> coaches = new List<Coach>();

            for (int i = 1; i < splitedInput.Length; i++)
            {
                var coachParts = splitedInput[i].Split('-');
                string coachId = coachParts[0];
                int seats = int.Parse(coachParts[1]);

                if (seats > 0 && seats > 72)
                {
                    throw new InvalidInputExecption("Seats Must Be Between 1 to 72");
                }

                CoachType type = coachId.StartsWith("S") ? CoachType.SL :
                                 coachId.StartsWith("B") ? CoachType.A3 :
                                 coachId.StartsWith("A") ? CoachType.A2 :
                                 coachId.StartsWith("H") ? CoachType.A1 :
                                 throw new InvalidInputExecption($"Unknown coach type for ID: {coachId}");

                ValidateCoachIdRange(coachId, type);

                coaches.Add(new Coach(coachId, type, seats));

            }
            if (!coaches.Any(c => c.CoachType == CoachType.SL))
            {
                throw new InvalidInputExecption("At least one Sleeper class (SL) coach is required.");
            }
            return coaches;

        }
        private static void ValidateCoachIdRange(string coachId, CoachType coachType)
        {
            if (!int.TryParse(coachId.Substring(1), out int coachNumber))
                throw new InvalidInputExecption($"Coach ID must end with a number: {coachId}");

            switch (coachType)
            {
                case CoachType.SL:
                    if (coachNumber < 1 || coachNumber > 18)
                        throw new InvalidInputExecption("Sleeper coach ID must be between S1 and S18");
                    break;

                case CoachType.A2:
                case CoachType.A3:
                    if (coachNumber < 1 || coachNumber > 3)
                        throw new InvalidInputExecption("2nd/3rd AC coach ID must be between A1–A3 or B1–B3");
                    break;

                case CoachType.A1:
                    if (coachNumber != 1)
                        throw new InvalidInputExecption("First AC coach ID must be H1");
                    break;
            }
        }

    }
}
