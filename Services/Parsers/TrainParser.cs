using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Train_Reservation_System_CLI.Models;
using Train_Reservation_System_CLI.Services.IOHandlers;
using Train_Reservation_System_CLI.Execptions;

namespace Train_Reservation_System_CLI.Services.Parsers
{
    public static class TrainParser
    {
        public static Route ParseTrainRoute(string[] routeInput)
        {
            int trainNumber = int.Parse(routeInput[0]);
            string source = routeInput[1].Split("-")[0];
            string destination = routeInput[routeInput.Length - 1].Split("-")[0];

            var route = new Route(source, destination);

            for (int i = 1; i < routeInput.Length; i++)
            {
                string station = routeInput[i].Split("-")[0];
                int stationDistance = int.Parse(routeInput[i].Split("-")[1]);

                route.RouteAndDistanceMap.Add($"{station}", stationDistance);
            }
            return route;
        }

        public static List<Coach> ParseTrainCoaches(int trainNumber, string[] coachesInput)
        {
            if (trainNumber != int.Parse(coachesInput[0]))
            {
                throw new InvalidInputExecption("Train Number and Coach Number Must Be Same");
            }

            List<Coach> coaches = new List<Coach>();

            for (int i = 1; i < coachesInput.Length; i++)
            {
                var coachParts = coachesInput[i].Split('-');
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
