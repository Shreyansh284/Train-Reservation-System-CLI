using Train_Reservation_System_CLI.Execptions;
using Train_Reservation_System_CLI.Models;
using Train_Reservation_System_CLI.Validators;
using static Train_Reservation_System_CLI.Utils.InputUtils;

namespace Train_Reservation_System_CLI.Parsers;

public static class TrainParser
{
    const char Seperator = '-';

    public static Route ParseTrainRoute(string[] routeInput)
    {
        var route = new Route();

        for (var i = 1; i < routeInput.Length; i++)
        {
            var input = routeInput[i].Split(Seperator);
            var station = input[0];
            var stationDistance = ParseInt(input[1]);

            route.AddRouteStation(new RouteStation(station, stationDistance));
        }

        return route;
    }

    public static List<Coach> ParseTrainCoaches(int trainNumber, string[] coachesInput)
    {
        if (trainNumber != ParseInt(coachesInput[0]))
            throw new InvalidInputExecption("Train Number and Coach Number Must Be Same");

        var coaches = new List<Coach>();

        for (var i = 1; i < coachesInput.Length; i++)
        {
            var coachParts = coachesInput[i].Split(Seperator);
            var coachId = coachParts[0];
            var seats = ParseInt(coachParts[1]);

            if (seats > 0 && seats > 72) throw new InvalidInputExecption("Seats Must Be Between 1 to 72");

            var type = GetCoachType(coachId);
            TrainValidator.ValidateCoachIdRange(coachId, type);

            coaches.Add(new Coach(coachId, type, seats));
        }

        if (coaches.All(c => c.CoachType != CoachType.SL))
            throw new InvalidInputExecption("At least one Sleeper class (SL) coach is required.");

        return coaches;
    }

    private static CoachType GetCoachType(string coachId)
    {
        var type = coachId.StartsWith('S') ? CoachType.SL :
            coachId.StartsWith('B') ? CoachType.A3 :
            coachId.StartsWith('A') ? CoachType.A2 :
            coachId.StartsWith('H') ? CoachType.A1 :
            throw new InvalidInputExecption($"Unknown coach type for ID: {coachId}");
        return type;
    }
}