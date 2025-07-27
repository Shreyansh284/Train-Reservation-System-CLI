using Train_Reservation_System_CLI.DomainModelGenerator;
using Train_Reservation_System_CLI.IOHandlers;
using Train_Reservation_System_CLI.Models;
using Train_Reservation_System_CLI.Parsers;
using Train_Reservation_System_CLI.Parsers.Model;
using Train_Reservation_System_CLI.Validators;
using static Train_Reservation_System_CLI.Utils.InputUtils;

namespace Train_Reservation_System_CLI.Services;

internal class TrainManager
{
    public static readonly List<Train> Trains = new();

    public void AddTrains()
    {
        try
        {
            var input = InputHandler.ReadInput("Enter number of trains to be added: ");
            var count = ParseInt(input);
            TrainValidator.ValidateNumberOfTrain(count);
            for (var i = 0; i < count; i++)
            {
                AddTrainFromDetails();
                OutputHandler.PrintMessage("Train Added Successfully");
            }
        }
        catch (Exception e)
        {
            OutputHandler.PrintError(e.Message);
            AddTrains();
        }
    }

    private static void AddTrainFromDetails()
    {
        var train = GetTrainDetails();
        AddSingleTrain(train);
    }

    private static void AddSingleTrain(Train train)
    {
        Trains.Add(train);
    }

    private static Train GetTrainDetails()
    {
        var trainNumberAndRoute = GetTrainNumberAndRouteDetails();
        var coaches = GetCoaches(trainNumberAndRoute.TrainNumber);
        var train = TrainDomainModelGenerator.GenerateTrainDomainModel(new ParsedTrain(trainNumberAndRoute.TrainNumber,
            trainNumberAndRoute.Route, coaches));

        return train;
    }

    private static TrainNumberRouteDTO GetTrainNumberAndRouteDetails()
    {
        try
        {
            var trainNumberAndRoute =
                InputHandler.ReadInput("Enter Train Number and Route: (e.g.,29772 Vadodara-0 Dahod-150 Indore-350)");
            var splitTrainNumberAndRoute = SplitInput(trainNumberAndRoute);
            TrainValidator.ValidateTrainNumberAndRouteInput(splitTrainNumberAndRoute);
            var trainNumber = ParseInt(splitTrainNumberAndRoute[0]);
            var route = TrainParser.ParseTrainRoute(splitTrainNumberAndRoute);
            return new TrainNumberRouteDTO(trainNumber, route);
        }
        catch (Exception e)
        {
            OutputHandler.PrintError(e.Message);
            return GetTrainNumberAndRouteDetails();
        }
    }

    private static List<Coach> GetCoaches(int trainNumber)
    {
        try
        {
            var trainCoaches = InputHandler.ReadInput("Enter Train Coaches: ( e.g., 29772 S1-72 S2-72 B1-72 A1-48)");
            var splitTrainCoaches = SplitInput(trainCoaches);
            TrainValidator.ValidateTrainCoachesInput(trainNumber, splitTrainCoaches);
            return TrainParser.ParseTrainCoaches(trainNumber, splitTrainCoaches);
        }
        catch (Exception e)
        {
            OutputHandler.PrintError(e.Message);
            return GetCoaches(trainNumber);
        }
    }

    public void GetAllTrains()
    {
        OutputHandler.PrintAllTrains(Trains);
    }

    public List<Train> GetTrainsByRoute(string from, string to)
    {
        return Trains.Where(t => t.HasRoute(from, to)).ToList();
    }

    public List<Train> FilterTrainsByCoachType(List<Train> trains, CoachType coachType)
    {
        return trains.Where(train => train.HasCoach(coachType)).ToList();
    }
}