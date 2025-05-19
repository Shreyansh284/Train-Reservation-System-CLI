using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Train_Reservation_System_CLI.Models;
using Train_Reservation_System_CLI.Services.IOHandlers;
using Train_Reservation_System_CLI.Services.Parsers;
using Train_Reservation_System_CLI.Execptions;

namespace Train_Reservation_System_CLI.Services
{
    class TrainManager
    {
        public List<Train> Trains = new List<Train>();

        public void AddTrain()
        {
            int numberOfTrains = InputHandler.ReadInt("Enter Number of Trains to be Added: ",50,1);
            for (int i = 0; i < numberOfTrains; i++)
            {
                var train = GetTrainDetails();
                Trains.Add(train);
            }
        }
        public static Train GetTrainDetails()
        {
            String[] trainNumberAndRoute = InputHandler.ReadString("Enter Train Number and Route: (e.g.,29772 Vadodara-0 Dahod-150 Indore-350)",null,3);
            int trainNumber = int.Parse(trainNumberAndRoute[0]);
            Route route = TrainParser.ParseTrainRoute(trainNumberAndRoute);
            String[] trainCoaches = InputHandler.ReadString("Enter Train Coaches: ( e.g., 29772 S1-72 S2-72 B1-72 A1-48)",null,2);
            List<Coach> coaches = TrainParser.ParseTrainCoaches(trainNumber, trainCoaches);
            Train train = new Train(trainNumber, route, coaches);
            return train;
        }
        public void GetAllTrains()
        {
            OutputHandler.PrintAllTrains(Trains);
        }
        public List<Train> GetTrainsByRoute(string from, string to)
        {
            return Trains.Where(t => t.HasRoute(from, to)).ToList();
        }
        public List<Train> GetTrainsByCoachType(List<Train> trains,CoachType coachType)
        {
            List<Train> matchingTrains = new();
            foreach (var train in trains)
            {
                if (train.HasCoach(coachType) != null)
                    matchingTrains.Add(train);
            }
            return matchingTrains;
        }

    }
}
