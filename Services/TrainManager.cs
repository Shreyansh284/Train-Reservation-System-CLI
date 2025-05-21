using Train_Reservation_System_CLI.DomainModelGenerator;
using Train_Reservation_System_CLI.Execptions;
using Train_Reservation_System_CLI.IOHandlers;
using Train_Reservation_System_CLI.Models;
using Train_Reservation_System_CLI.Parsers;
using Train_Reservation_System_CLI.Parsers.Model;
using Train_Reservation_System_CLI.Validators;
using static Train_Reservation_System_CLI.Utils.InputUtils;

namespace Train_Reservation_System_CLI.Services;

internal class TrainManager
{
    public static List<Train> Trains = new();

    public void AddTrains()
    {
        const int min = 1, max = 50;

        var input = InputHandler.ReadInput("Enter number of trains to be added: ");
        var count = ParseInt(input);

        if (InputValidator.IsOutOfRange(max, min, count))
            throw new InvalidInputExecption($"Value must be between {min} and {max}");

        for (var i = 0; i < count; i++)
        {
            var train = GetTrainDetails();
            AddSingleTrain(train);
            OutputHandler.PrintMessage("Train Added Successfully");
        }
    }

    private static void AddSingleTrain(Train train)
    {
        Trains.Add(train);
    }

    public static Train GetTrainDetails()
    {
        var trainNumberAndRoute =InputHandler.ReadInput("Enter Train Number and Route: (e.g.,29772 Vadodara-0 Dahod-150 Indore-350)");
        var splitedTrainNumberAndRoute = SplitInput(trainNumberAndRoute);
        if (InputValidator.IsOutOfRange(null, 3, splitedTrainNumberAndRoute.Length))
            throw new InvalidInputExecption("Please Enter Details As Shown In Example");
        var trainNumber = ParseInt(splitedTrainNumberAndRoute[0]);
        var route = TrainParser.ParseTrainRoute(splitedTrainNumberAndRoute);

        var trainCoaches = InputHandler.ReadInput("Enter Train Coaches: ( e.g., 29772 S1-72 S2-72 B1-72 A1-48)");
        var splitedTrainCoaches = SplitInput(trainCoaches);
        if (InputValidator.IsOutOfRange(null, 2, splitedTrainNumberAndRoute.Length))
            throw new InvalidInputExecption("Please Enter Details As Shown In Example");
        var coaches = TrainParser.ParseTrainCoaches(trainNumber, splitedTrainCoaches);

        var train = TrainDomainModelGenerator.GenerateTrainDominModel(new ParsedTrain(trainNumber, route, coaches));

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

    public List<Train> GetTrainsByCoachType(List<Train> trains, CoachType coachType)
    {
        List<Train> matchingTrains = new();
        foreach (var train in trains)
            if (train.HasCoach(coachType) != null)
                matchingTrains.Add(train);
        return matchingTrains;
    }
}